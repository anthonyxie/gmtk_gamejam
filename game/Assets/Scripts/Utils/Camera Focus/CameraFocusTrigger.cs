using UnityEngine;

public enum CameraFocusTriggerType
{
    FocusWhileWithin,
    FocusOnEnter,
    FocusOnLeave,
    UnfocusOnEnter,
    UnfocusOnLeave,
    None
}

/**
 * Can be attached to anything with a trigger-mode Collider2D
 * to cause the camera to snap to a given [CameraFocusArea].
 * Can also be invoked manually when the [triggerType] is None.
 */
public class CameraFocusTrigger : AutoMonoBehaviour
{
    #region Unity-exposed public fields

    public CameraFocusTriggerType triggerType = CameraFocusTriggerType.FocusWhileWithin;

    [Tooltip("The area to focus on"), Required]
    public CameraFocusArea area;

    [Tooltip("The camera follower to snap"), Required]
    public CameraFocuser focuser;

    [Range(0.01f, 1f)]
    public float resizeTime = 0.5f;

    #endregion

    private Bounded _bounds;
    private Camera _camera;


    #region Unity Events

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>() == null) return;

        switch (triggerType) {
            case CameraFocusTriggerType.FocusOnEnter:
            case CameraFocusTriggerType.FocusWhileWithin:
                focuser.Focus(area, resizeTime);
                break;
            case CameraFocusTriggerType.UnfocusOnEnter:
                focuser.Unfocus(area);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<Player>() == null) return;
        switch (triggerType) {
            case CameraFocusTriggerType.FocusOnLeave:
                focuser.Focus(area, resizeTime);
                break;
            case CameraFocusTriggerType.FocusWhileWithin:
            case CameraFocusTriggerType.UnfocusOnLeave:
                focuser.Unfocus(area);
                break;
        }
    }

    public void ForceFocus() {
        focuser.Focus(area, resizeTime);
    }

    public void ForceUnfocus() {
        focuser.Unfocus(area);
    }

    #endregion
}