using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Manages the focus of the camera as it moves
 * from object/followers etc.
 */
[RequireComponent(typeof(Camera)), RequireComponent(typeof(Follow))]
public class CameraFocuser : AutoMonoBehaviour
{
    // Smoothing function used for transitioning the orthographic size
    private static readonly Utility.LerpFn<float> _lerp = Utility.EaseOut<float>(Mathf.SmoothStep);
    
    public BoundedCamera boundsRef;
    
    [AutoDefault, ReadOnly]
    public Follow followRef;

    [AutoDefault,  ReadOnly]
    public Camera cameraRef;

    public UnityEvent onFocus;


    // Used for restoring camera state
    private List<AreaStackElement> _areaStack = new();
    private OriginalSetup _setup = null;

    // Used for moving the camera's orthographic size using SmoothDamp
    private Coroutine _resizeCoroutine;

    private void OnEnable() {
        _setup = new OriginalSetup(
            followRef.anchor,
            cameraRef.orthographicSize,
            followRef.offset
        );
    }

    #region Public Methods

    /** Focus on the given CameraFocusArea, transitioning to it with the given time */
    public void Focus(CameraFocusArea area, float time = 0.5f) {
        var next = new AreaStackElement(area, time);

        if (_areaStack.Count == 0) {
            _setup.OldFollower = followRef.anchor;
        }

        _areaStack.Add(next);
        FocusInternal(next, next.Time);
    }

    /** Unfocus the most recently-focused thing */
    public void Unfocus() {
        UnfocusInternal(_areaStack.Count - 1);
    }

    /** Unfocus the specific CameraFocusArea specified */
    public void Unfocus(CameraFocusArea area) {
        int idx = _areaStack.FindIndex(a => a.Area == area);
        if (idx < 0) return;
        UnfocusInternal(idx);
    }

    #endregion

    #region Internal logic

    private void FocusInternal(AreaStackElement el, float time) {
        onFocus.Invoke();
        if (boundsRef != null) {
            boundsRef.enabled = false;
        }

        if (_resizeCoroutine != null) {
            StopCoroutine(_resizeCoroutine);
        }

        followRef.anchor = el.Area.transform;
        followRef.offset = Vector3.zero;

        float areaAspect = el.Area.Bounds.size.x / el.Area.Bounds.size.y;
        float cameraAspect = 1f * cameraRef.aspect;

        float targetSize;
        // Need to match camera width to area width
        if (areaAspect > cameraAspect) {
            targetSize = el.Area.Bounds.extents.x / cameraAspect;
        }
        else {
            // need to match camera height to area height

            targetSize = el.Area.Bounds.extents.y;
        }

        _resizeCoroutine = this.AutoLerp(cameraRef.orthographicSize, targetSize, time, _lerp,
            size => cameraRef.orthographicSize = size);
    }

    private void UnfocusInternal(int idx) {
        if (boundsRef != null) {
            boundsRef.enabled = true;
        }

        if (_resizeCoroutine != null) {
            StopCoroutine(_resizeCoroutine);
        }

        if (idx != _areaStack.Count - 1) {
            _areaStack.RemoveAt(idx);
            return;
        }

        if (_areaStack.Count != 1) {
            var el = _areaStack[^1];
            _areaStack.RemoveAt(_areaStack.Count - 1);
            FocusInternal(_areaStack[^1], el.Time);
            return;
        }

        var area = _areaStack[0];
        _areaStack.Clear();

        followRef.anchor = _setup.OldFollower;
        followRef.offset = _setup.OriginalOffset;
        _resizeCoroutine = this.AutoLerp(cameraRef.orthographicSize, _setup.OriginalOrthoSize, area.Time, _lerp,
            size => cameraRef.orthographicSize = size);
    }

    #endregion


    private record AreaStackElement
    {
        public CameraFocusArea Area;
        public float Time;

        public AreaStackElement(CameraFocusArea area, float time) {
            Area = area;
            Time = time;
        }
    }

    private record OriginalSetup
    {
        public Transform OldFollower;
        public float OriginalOrthoSize;
        public Vector3 OriginalOffset;

        public OriginalSetup(Transform oldFollower, float originalOrthoSize, Vector3 originalOffset) {
            OldFollower = oldFollower;
            OriginalOrthoSize = originalOrthoSize;
            OriginalOffset = originalOffset;
        }
    }
}