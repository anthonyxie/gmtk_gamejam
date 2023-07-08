using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Provides a BUNCH of utility methods. */
public static class Utility
{
    /** Function type. Linearly interpolates a type T between two values, from and to, according to a percent (0 to 1), t. */
    public delegate T LerpFn<T>(T from, T to, float t);

    /** Function type. Takes in a value of type T and does something with it. */
    public delegate void LerpValueCallback<in T>(T currentValue);

    /** Function type. Just does something. */
    public delegate void Thunk();

    /** For convenience, defines the in-out-eased lerp function on floats */
    public static LerpFn<float> EaseInOutF => EaseInOut<float>(Mathf.Lerp);

    /** Coroutine that waits [seconds] seconds and then calls [fn]. */
    public static IEnumerator WaitThen(float seconds, Thunk fn) {
        yield return new WaitForSeconds(seconds);
        fn();
    }

    /** Coroutine that waits for [first] to finish and then calls [fn] */
    public static IEnumerator WaitThen(Coroutine first, Thunk fn) {
        yield return first;
        fn();
    }

    /**
     * Coroutine linearly interpolates a value T from [from] to [to] over [totalSeconds] seconds,
     * calling [lerp] to do that, and calling [callback] once per frame to apply that value.
     */
    public static IEnumerator Lerp<T>(T from, T to, float totalSeconds, LerpFn<T> lerp,
        LerpValueCallback<T> callback) {
        var currTime = 0f;
        while (currTime < totalSeconds) {
            currTime += Time.deltaTime;

            var value = lerp(from, to, currTime / totalSeconds);
            callback(value);
            yield return null;
        }
    }

    /** Wraps a linear-interpolation function so that it eases in instead of linearly interpolates. */
    public static LerpFn<T> EaseIn<T>(LerpFn<T> fn) {
        return (from, to, f) => fn(from, to, f * f);
    }

    /** Wraps a linear-interpolation function so that it eases out instead of linearly interpolates. */
    public static LerpFn<T> EaseOut<T>(LerpFn<T> fn) {
        return (from, to, f) => fn(from, to, 1 - (1 - f) * (1 - f));
    }

    /** Wraps a linear-interpolation function so that it eases in and out instead of linearly interpolates. */
    public static LerpFn<T> EaseInOut<T>(LerpFn<T> fn) {
        var easedIn = EaseIn(fn);
        var easedOut = EaseOut(fn);
        return (from, to, f) => fn(easedIn(from, to, f), easedOut(from, to, f), f);
    }

    /**     
     * Calculates the distance between world points [a] and [b]
     * on a sphere centered at world point [origin] with radius [r]
     */
    public static float SphericalDistance(Vector3 origin, Vector3 a, Vector3 b, float r) {
        return r * Mathf.Acos(Vector3.Dot(a - origin, b - origin) / (r * r));
    }
}

/**
 * Provides utility functions that are extensions on
 * other classes
 */
public static class UtilityExtensions
{
    private static readonly Dictionary<GameObject, Vector3> _originalScales = new();

    /** Calls the given [fn] after a number of [seconds] */
    public static Coroutine WaitThen(this MonoBehaviour behaviour, float seconds, Utility.Thunk fn) {
        return behaviour.StartCoroutine(Utility.WaitThen(seconds, fn));
    }

    /** Calls the given [fn] after the [first] coroutine finishes */
    public static Coroutine WaitThen(this MonoBehaviour behaviour, Coroutine first, Utility.Thunk fn) {
        return behaviour.StartCoroutine(Utility.WaitThen(first, fn));
    }

    /**
     * Automatically lerps [from] a value [to] another over [totalSeconds] time, using the provided
     * [lerp] function and assigning the value using a [callback].
     */
    public static Coroutine AutoLerp<T>(this MonoBehaviour behaviour, T from, T to, float totalSeconds,
        Utility.LerpFn<T> lerp, Utility.LerpValueCallback<T> callback) {
        return behaviour.StartCoroutine(Utility.Lerp(from, to, totalSeconds, lerp, callback));
    }

    /**
     * Returns true if the given [worldPosition] is inside this [camera]'s frustum
     * (i.e. if it is in view)
     */
    public static bool IsInView(this Camera camera, Vector3 worldPosition) {
        var position = camera.WorldToScreenPoint(worldPosition);

        return position.x > 0 && position.y > 0 && position.z > 0
               && position.x < camera.pixelWidth && position.y < camera.pixelHeight;
    }

    /** Returns _a_ vector that is orthogonal to this [vector] */
    public static Vector3 OrthogonalVector(this Vector3 vector) {
        var cross = Vector3.Cross(vector, Vector3.up);
        if (cross.magnitude == 0) return Vector3.Cross(vector, Vector3.forward);

        return cross;
    }

    /** Calculates the rectangle in world space of this RectTransform. */
    public static Bounds GetWorldBounds(this RectTransform t) {
        var corners = new Vector3[4];
        t.GetWorldCorners(corners);

        var width = corners[2].x - corners[1].x;
        var height = corners[1].y - corners[0].y;
        var center = new Vector3(corners[0].x + width / 2, corners[0].y + height / 2, t.position.z);
        var size = new Vector3(width, height, 0);

        return new Bounds(center, size);
    }

    /**
     * Sets this gameObject to be "visible" or "invisible" by setting
     * its local scale to Vector3.zero.
     * Keeps track of previous scales and can restore them, too!
     */
    public static void SetVisible(this GameObject gameObject, bool visible) {
        var localScale = gameObject.transform.localScale;
        switch (visible) {
            case false when localScale != Vector3.zero:
                _originalScales[gameObject] = localScale;
                localScale = Vector3.zero;
                gameObject.transform.localScale = localScale;
                break;

            case true when localScale == Vector3.zero && _originalScales.ContainsKey(gameObject):
                gameObject.transform.localScale = _originalScales[gameObject];
                _originalScales.Remove(gameObject);
                break;
        }
    }

    /**
     * Returns true if this [gameObject] is "visible," i.e.
     * has a localScale not equal to zero.
     */
    public static bool IsVisible(this GameObject gameObject) {
        return gameObject.transform.localScale != Vector3.zero;
    }

    /**
     * Returns a world-space boundary that encapsulates all the
     * renderers inside or on the given gameObject
     */
    public static Bounds? GetWorldBounds(this GameObject gameObject) {
        var boundary = new Bounds();
        var hasBoundary = false;

        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
            if (!hasBoundary) {
                hasBoundary = true;
                boundary = renderer.bounds;
            }
            else {
                boundary.Encapsulate(renderer.bounds);
            }

        return hasBoundary ? boundary : null;
    }

    public static bool Encapsulates2D(this Bounds bounds, Bounds other) {
        return bounds.Contains2D(other.max) && bounds.Contains2D(other.min);
    }
    
    public static bool Contains2D(this Bounds bounds, Vector2 other) {
        Bounds temp = new Bounds(new Vector2(bounds.center.x, bounds.center.y), bounds.size);
        return temp.Contains(other);
    }
}