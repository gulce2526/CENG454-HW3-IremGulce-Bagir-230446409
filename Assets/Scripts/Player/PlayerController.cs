using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Mana")]
    [SerializeField] private float maxMana = 200f;
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

        // Set up the Decorator chain
        frostSpell.SetWrappedSpell(baseSpell);
        loveSpell.SetWrappedSpell(frostSpell);
        burnSpell.SetWrappedSpell(loveSpell);

        GameEvents.OnSpellUnlocked += HandleSpellUnlocked;
    }

    private void OnDestroy()
    {
        GameEvents.OnSpellUnlocked -= HandleSpellUnlocked;
    }

    private void Update()
    {
        HandleMovement();
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
        if (currentSpell == null) { Debug.Log("Spell is null!"); return; }

        float manaCost = currentSpell.GetManaCost();
        Debug.Log($"Casting spell {selectedSpellIndex}, manaCost={manaCost}, currentMana={currentMana}");

        if (currentMana < manaCost) { Debug.Log("Not enough mana!"); return; }

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;

        currentSpell.Cast(direction, transform.position);
        currentMana -= manaCost;
        animator.SetTrigger("attack");
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
            animator.SetTrigger("die");
            GameEvents.PlayerDied();
            Debug.Log("Sorcerer defeated!");
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    }

    public void AddMana(float amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
    }
}