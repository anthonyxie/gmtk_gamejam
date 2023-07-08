using UnityEngine;

/**
 * Extension of Bounded that makes sure
 * no part of the objects leaves the Boundary.
 */
[RequireComponent(typeof(Collider2D))]
public class Bounded2DObject : Bounded
{
    [AutoDefault]
    public new Collider2D collider;
    
    protected override float GetHorizontalExtent() => collider.bounds.extents.x;


    protected override float GetVerticalExtent() => collider.bounds.extents.y;
}