    using System;
    using UnityEngine;

    public class ShowColliderBoundary : AutoMonoBehaviour
    {
        public bool always = true;
        public Color color = Color.white;

        [ReadOnly, AutoDefault]
        public Collider2D collider;


        private void OnDrawGizmosSelected() {
            if (always) return;
            Draw();
        }

        private void OnDrawGizmos() {
            if (!always) return;
            Draw();
        }

        private void Draw() {
            Gizmos.color = color;
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }