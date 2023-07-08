using UnityEngine;
using UnityEngine.Serialization;

/**
 * Smoothly follow another GameObject
 */
public class Follow : AutoMonoBehaviour
{
    #region Unity-editable public fields

    [Tooltip("The transform this will be anchored to"), Required]
    public Transform anchor;

    [Tooltip("If specified, we will make sure that we don't move outside of the boundaries specified.")]
    public Bounded respectBoundaries;

    [Tooltip("How 'loose' we are. The lower this number, the faster we snap.")]
    [FormerlySerializedAs("tension")]
    [Range(0.01f, 0.3f)]
    public float looseness = 0.1f;

    [Tooltip("The maximum speed at which we'll snap")]
    public float maxSpeed = 100;

    [Header("Direction locking")]
    [Tooltip("Don't follow on the X axis")]
    public bool lockX = false;

    [Tooltip("Don't follow on the Y axis")]
    public bool lockY = false;

    [Tooltip("Don't follow on the Z axis")]
    public bool lockZ = true;

    public Vector3 offset = Vector3.zero;

    #endregion

    // Used for SmoothDamp
    private Vector3 _velocity;

    // Needed because checking != null on GameObjects is slow (i.e. shouldn't happen in Update).
    private bool _isrespectBoundariesNotNull;

    private Vector3 Target {
        get {
            Vector3 target = anchor.position + offset;
            if (_isrespectBoundariesNotNull && respectBoundaries.enabled) {
                target = respectBoundaries.ClampToBoundary(target);
            }

            return target;
        }
    }

    #region Unity events

    private void OnEnable() {
        _isrespectBoundariesNotNull = respectBoundaries != null;
        if (respectBoundaries != null) {
            respectBoundaries.StartConsuming();
        }

        SetWithLock(Target);
    }

    private void OnDisable() {
        if (respectBoundaries != null) {
            respectBoundaries.StopConsuming();
        }
    }

    private void Update() {
        Vector3 next = Vector3.SmoothDamp(transform.position, Target, ref _velocity, looseness, maxSpeed,
            Time.deltaTime);

        SetWithLock(next);
    }

    #endregion

    #region Public methods

    public void SetAnchor(Transform newAnchor) {
        anchor = newAnchor;
    }

    public void SetOffset(Vector3 newOffset) {
        offset = newOffset;
    }

    #endregion

    #region Private helper functions

    private void SetWithLock(Vector3 position) {
        if (lockX) position.x = transform.position.x;
        if (lockY) position.y = transform.position.y;
        if (lockZ) position.z = transform.position.z;

        transform.position = position;
    }

    #endregion
}