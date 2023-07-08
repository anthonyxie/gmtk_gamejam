using UnityEngine;

/**
 *  Extension of Bounded that makes sure no
 * part of the camera leaves the Boundary.
 */
[RequireComponent(typeof(Camera))]
public class BoundedCamera : Bounded
{
    [AutoDefault, ReadOnly]
    public new Camera camera;
    

    // Gets the extent of this camera at the z of the given Boundary, for a camera with perspective.
    private Vector2 PerspectiveExtents {
        get {
            float z = by.Bounds.center.z;
            Vector3 min = camera.ViewportToWorldPoint(new Vector3(0, 0, z));
            Vector3 max = camera.ViewportToWorldPoint(new Vector3(1, 1, z));

            return new Vector2((max.x - min.x) / 2, (max.y - min.y) / 2);
        }
    }

    protected override float GetHorizontalExtent() {
        if (camera.orthographic) {
            return camera.orthographicSize * camera.aspect;
        }

        return PerspectiveExtents.x;
    }

    protected override float GetVerticalExtent() {
        if (camera.orthographic) {
            return camera.orthographicSize;
        }

        return PerspectiveExtents.y;
    }
}