using UnityEngine;

/**
 * Given a Boundary, bounds the given object
 * within it. Extend this class with
 * logic to get the left/right and up/down extents
 * so that the object always stays in bounds.
 */
public class Bounded : AutoMonoBehaviour
{

    [Tooltip("The Boundary we're bounded by"), Required]
    public Boundary by;
    

    public bool boundX = true;
    public bool boundY = false;

    /**
     * The number of other classes that are "consuming" these boundaries.
     * If this number is > 0, then this class won't do the bounding and instead
     * let the other class do it.
     */
    private int _consumers;

    // By default, no extents; object will just slide to the center before it stops.
    protected virtual float GetHorizontalExtent() => 0f;
    protected virtual float GetVerticalExtent() => 0f;

    private void LateUpdate() {
        if (_consumers > 0) return;
        
        transform.position = ClampToBoundary(transform.position);
    }

    /** Clamps the given [location] to the boundary given the horizontal and vertical extents */
    public Vector3 ClampToBoundary(Vector3 location) {
        float objHorizontalExtent = GetHorizontalExtent();
        float objVerticalExtent = GetVerticalExtent();

        float xMin = by.Bounds.min.x + objHorizontalExtent;
        float xMax = by.Bounds.max.x - objHorizontalExtent;
        float yMin = by.Bounds.min.y + objVerticalExtent;
        float yMax = by.Bounds.max.y - objVerticalExtent;

        var position = location;
        float targetX;
        if (boundX) {
            // If the boundary is smaller than the extents, choose the center.
            targetX = xMax < xMin ? (xMin + xMax) / 2 : Mathf.Clamp(position.x, xMin, xMax);
        }
        else {
            targetX = position.x;
        }

        float targetY;
        if (boundY) {
            targetY = yMax < yMin ? (yMin + yMax) / 2 : Mathf.Clamp(position.y, yMin, yMax);
        }
        else {
            targetY = position.y;
        }

        return new Vector3(targetX, targetY, position.z);
    }

    public void StartConsuming() {
        _consumers++;
    }

    public void StopConsuming() {
        _consumers--;
    }

    public void SetBoundary(Boundary b) {
        by = b;
    }
}