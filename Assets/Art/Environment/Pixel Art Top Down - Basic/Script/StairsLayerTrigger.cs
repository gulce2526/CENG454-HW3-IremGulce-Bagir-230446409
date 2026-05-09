using System.Collections;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class StairsLayerTrigger : MonoBehaviour
    {
        public Direction direction;
        [Space]
        public string layerUpper;
        public string sortingLayerUpper;
        [Space]
        public string layerLower;
        public string sortingLayerLower;

        [Space]
        [Tooltip("Seconds to block re-triggering after a layer change.")]
        public float transitionCooldown = 0.4f;

        private bool onCooldown = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || onCooldown) return;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            bool goingToUpper = false;
            bool goingToLower = false;

            // Velocity tells us which way the player is ACTUALLY moving,
            // so it works regardless of collider offset or pivot position.
            switch (direction)
            {
                case Direction.South:
                    // Upper platform is to the north  → moving north (velocity.y > 0) = going up
                    goingToUpper = rb.linearVelocity.y > 0.01f;
                    goingToLower = rb.linearVelocity.y < -0.01f;
                    break;
                case Direction.North:
                    // Upper platform is to the south  → moving south (velocity.y < 0) = going up
                    goingToUpper = rb.linearVelocity.y < -0.01f;
                    goingToLower = rb.linearVelocity.y > 0.01f;
                    break;
                case Direction.West:
                    // Upper platform is to the east   → moving east  (velocity.x > 0) = going up
                    goingToUpper = rb.linearVelocity.x > 0.01f;
                    goingToLower = rb.linearVelocity.x < -0.01f;
                    break;
                case Direction.East:
                    // Upper platform is to the west   → moving west  (velocity.x < 0) = going up
                    goingToUpper = rb.linearVelocity.x < -0.01f;
                    goingToLower = rb.linearVelocity.x > 0.01f;
                    break;
            }

            if (goingToUpper)
            {
                Debug.Log($"[StairsLayerTrigger] Going UP → '{layerUpper}'");
                StartCoroutine(TransitionWithCooldown(other.gameObject, layerUpper, sortingLayerUpper));
            }
            else if (goingToLower)
            {
                Debug.Log($"[StairsLayerTrigger] Going DOWN → '{layerLower}'");
                StartCoroutine(TransitionWithCooldown(other.gameObject, layerLower, sortingLayerLower));
            }
        }

        // Exit is kept as a safety net in case the player barely
        // drifts across the trigger with near-zero velocity on Enter.
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || onCooldown) return;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            bool goingToUpper = false;
            bool goingToLower = false;

            switch (direction)
            {
                case Direction.South:
                    goingToUpper = rb.linearVelocity.y > 0.01f;
                    goingToLower = rb.linearVelocity.y < -0.01f;
                    break;
                case Direction.North:
                    goingToUpper = rb.linearVelocity.y < -0.01f;
                    goingToLower = rb.linearVelocity.y > 0.01f;
                    break;
                case Direction.West:
                    goingToUpper = rb.linearVelocity.x > 0.01f;
                    goingToLower = rb.linearVelocity.x < -0.01f;
                    break;
                case Direction.East:
                    goingToUpper = rb.linearVelocity.x < -0.01f;
                    goingToLower = rb.linearVelocity.x > 0.01f;
                    break;
            }

            if (goingToUpper)
                StartCoroutine(TransitionWithCooldown(other.gameObject, layerUpper, sortingLayerUpper));
            else if (goingToLower)
                StartCoroutine(TransitionWithCooldown(other.gameObject, layerLower, sortingLayerLower));
        }

        private IEnumerator TransitionWithCooldown(GameObject target, string layer, string sortingLayer)
        {
            onCooldown = true;
            SetLayerAndSortingLayer(target, layer, sortingLayer);
            yield return new WaitForSeconds(transitionCooldown);
            onCooldown = false;
        }

        private void SetLayerAndSortingLayer(GameObject target, string layer, string sortingLayer)
        {
            int layerIndex = LayerMask.NameToLayer(layer);
            if (layerIndex == -1)
            {
                Debug.LogError($"[StairsLayerTrigger] Physics layer '{layer}' not found!");
                return;
            }

            if (!System.Array.Exists(SortingLayer.layers, sl => sl.name == sortingLayer))
            {
                Debug.LogError($"[StairsLayerTrigger] Sorting layer '{sortingLayer}' not found!");
                return;
            }

            // Change physics layer on root AND every child (bones, body parts, etc.)
            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
                t.gameObject.layer = layerIndex;

            // Change sorting layer on every SpriteRenderer in the hierarchy
            foreach (SpriteRenderer sr in target.GetComponentsInChildren<SpriteRenderer>(true))
                sr.sortingLayerName = sortingLayer;

            Debug.Log($"[StairsLayerTrigger] '{target.name}' → Layer: '{layer}' | Sorting Layer: '{sortingLayer}'");
        }

        public enum Direction { North, South, West, East }
    }
}