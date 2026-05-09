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
            if (onCooldown) return;
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

        private void OnTriggerExit2D(Collider2D other)
        {
            if (onCooldown) return;
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

            foreach (Transform t in target.GetComponentsInChildren<Transform>(true))
                t.gameObject.layer = layerIndex;

            foreach (SpriteRenderer sr in target.GetComponentsInChildren<SpriteRenderer>(true))
                sr.sortingLayerName = sortingLayer;
        }

        public enum Direction { North, South, West, East }
    }
}