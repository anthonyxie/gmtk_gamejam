using UnityEngine;

/**
 * Takes this GameObject and allows it to move with respect
 * to a linked camera depending on the Parallax Factor.
 */
public class Parallax : AutoMonoBehaviour
{
    [AutoDefaultMainCamera, Required]
    public Camera linkedCamera;
    
    [Tooltip("Negative if closer to the camera, positive if behind")]
    [Range(-4f, 4f)]
    public float parallaxFactor = 0f;

    public bool lockY = true;
    public bool lockZ = true;

    private Vector3 _lastCamPosition;

    private void Awake() {
        _lastCamPosition = linkedCamera.transform.position;
    }
    

    void Update() {
        Vector3 curCamPosition = linkedCamera.transform.position;
        Vector3 movement = curCamPosition - _lastCamPosition;
        _lastCamPosition = curCamPosition;

        if (lockY) {
            movement.y = 0;
        }

        if (lockZ) {
            movement.z = 0;
        }
        
        transform.position += movement * parallaxFactor;
    }
}
