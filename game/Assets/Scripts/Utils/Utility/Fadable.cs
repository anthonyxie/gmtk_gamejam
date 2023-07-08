using System.Collections.Generic;
using UnityEngine;

/**
 * Utility class that allows objects to fade in and out.
 * This class can be concretely implemented to work with pretty
 * much anything.
 */
public abstract class Fadable<T> : AutoMonoBehaviour, IFadable
{
    public enum FadeStartMode
    {
        /// Keep initial alpha on start
        Normal,

        /// Start faded out, and immediately fade in
        FadeInOnStart,

        /// Start faded out, and stay faded out
        KeepTransparentOnStart
    }


    [Tooltip("Whether we should start faded out, fade in immediately, or start faded in.")]
    public FadeStartMode startMode;

    [Tooltip("The time to fade in, in seconds")]
    public float fadeInTime = 1f;

    [Tooltip("The time to fade out, in seconds")]
    public float fadeOutTime = 1f;

    /** (memoized version of TargetComponents) */
    private List<T> _components;

    /** true if we are fading in */
    private bool _fadingIn;

    /** true if we are fading out */
    private bool _fadingOut;

    /** The list of target components to affect */
    protected abstract List<T> TargetComponents { get; }

    private void Start()
    {
        _components = TargetComponents;

        if (startMode != FadeStartMode.Normal) CutOut();

        if (startMode == FadeStartMode.FadeInOnStart) FadeIn();
    }

    private void Update()
    {
        HandleFade(ref _fadingIn, 1f, fadeInTime);
        HandleFade(ref _fadingOut, 0f, fadeOutTime);
    }

    /** Fades this in over the given fade time (to alpha = 1) */
    public void FadeIn()
    {
        _fadingIn = true;
        _fadingOut = false;
    }

    /** Fades this out over the given fade time (to alpha = 0) */
    public void FadeOut()
    {
        _fadingIn = false;
        _fadingOut = true;
    }

    /** Instantly sets this alpha to 1 */
    public void CutIn()
    {
        foreach (var component in _components) SetAlpha(component, 1f);
    }

    /** Instantly sets this alpha to 0 */
    public void CutOut()
    {
        foreach (var component in _components) SetAlpha(component, 0f);
    }

    /** Returns the alpha value of a given component */
    protected abstract float GetAlpha(T component);

    /** Sets the alpha value of a given component */
    protected abstract void SetAlpha(T component, float value);

    /** Actually does the fading */
    private void HandleFade(ref bool isChanging, float target, float time)
    {
        if (!isChanging) return;

        var magnitude = time == 0 ? float.PositiveInfinity : Time.deltaTime / time;
        var didChange = false;

        foreach (var component in _components)
        {
            var curAlpha = GetAlpha(component);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (curAlpha == target) continue;

            didChange = true;
            var direction = Mathf.Sign(target - curAlpha);

            // Ensures we don't overshoot
            var destination = direction < 0
                ? Mathf.Max(curAlpha + direction * magnitude, target)
                : Mathf.Min(curAlpha + direction * magnitude, target);

            SetAlpha(component, destination);
        }

        if (!didChange) isChanging = false;
    }
}