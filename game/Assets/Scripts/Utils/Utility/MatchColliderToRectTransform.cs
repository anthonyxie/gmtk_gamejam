using UnityEngine;

/**
 * Matches the size of the given BoxCollider2D to the size of
 * the associated RectTransform. This is especially helpful
 * for adding MouseEvents to UI!
 */
[RequireComponent(typeof(RectTransform)), RequireComponent(typeof(BoxCollider2D))]
[ExecuteAlways]
public class MatchColliderToRectTransform : AutoMonoBehaviour
{
    [AutoDefault, ReadOnly]
    public RectTransform rect;

    [AutoDefault, ReadOnly]
    public new BoxCollider2D collider;

    private void Update() {
        collider.size = new Vector2(rect.rect.size.x, rect.rect.size.y);
    }
}