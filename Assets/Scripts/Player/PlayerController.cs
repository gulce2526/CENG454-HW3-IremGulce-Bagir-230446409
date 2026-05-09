using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Mana")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float manaRegenRate = 5f;
    private float currentMana;

    [Header("Spells")]
    [SerializeField] private BaseSpell baseSpell;
    [SerializeField] private FrostDecorator frostSpell;
    [SerializeField] private LoveDecorator loveSpell;
    [SerializeField] private BurnDecorator burnSpell;

    private ISpellEffect[] spells;
    private int selectedSpellIndex = 0;
    private int unlockedSpellCount = 1;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxMana => maxMana;
    public float CurrentMana => currentMana;
    public bool IsDead => currentHealth <= 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        currentMana = maxMana;

        spells = new ISpellEffect[] { baseSpell, frostSpell, loveSpell, burnSpell };

        GameEvents.OnSpellUnlocked += HandleSpellUnlocked;
    }

    private void OnDestroy()
    {
        GameEvents.OnSpellUnlocked -= HandleSpellUnlocked;
    }

    private void Update()
    {
        HandleMovement();
        HandleManaRegen();
        HandleSpellSelection();
        HandleSpellCast();
    }

    private void HandleMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isRun", moveInput.magnitude > 0);

        if (moveInput.x != 0)
            transform.localScale = new Vector3(
                moveInput.x > 0 ? 0.35f : -0.35f, 0.35f, 1);
    }

    private void HandleManaRegen()
    {
        if (currentMana < maxMana)
            currentMana = Mathf.Min(maxMana,
                currentMana + manaRegenRate * Time.deltaTime);
    }

    private void HandleSpellSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSpellIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedSpellCount > 1) selectedSpellIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedSpellCount > 2) selectedSpellIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && unlockedSpellCount > 3) selectedSpellIndex = 3;
    }

    private void HandleSpellCast()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        ISpellEffect currentSpell = spells[selectedSpellIndex];
        if (currentSpell == null) return;

        float manaCost = 0f;
        if (currentSpell is BaseSpell bs) manaCost = bs.GetManaCost();
        else if (currentSpell is SpellDecorator sd) manaCost = sd.GetManaCost();

        if (currentMana < manaCost) return;

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;

        currentSpell.Cast(direction, transform.position);
        currentMana -= manaCost;
    }

    private void HandleSpellUnlocked(int spellLevel)
    {
        unlockedSpellCount = spellLevel + 1;
        selectedSpellIndex = spellLevel;
        Debug.Log($"Spell {spellLevel} unlocked!");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        currentHealth -= amount;
        if (IsDead)
        {
            GameEvents.PlayerDied();
            Debug.Log("Sorcerer defeated!");
        }
    }

    public void AddMana(float amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
    }
}