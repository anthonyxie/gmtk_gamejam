using UnityEngine;

/**
 * Defines a boundary that Bounded objects must stay within.
 */
[RequireComponent(typeof(RectTransform))]
public class Boundary : AutoMonoBehaviour
{
    public Bounds Bounds => rect.GetWorldBounds();

    [AutoDefault, ReadOnly]
    public RectTransform rect;

    private void OnDrawGizmos() {
        var gizBounds = rect.GetWorldBounds();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gizBounds.center, gizBounds.size);
    }
}