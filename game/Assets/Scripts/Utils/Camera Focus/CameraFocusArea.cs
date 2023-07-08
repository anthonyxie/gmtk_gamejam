using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


/**
 * Defines an area used by [CameraFocusTrigger] to focus the
 * camera on something. Will show up as a blue rectangle in
 * the editor. Does nothing by itself.
 */
[RequireComponent(typeof(RectTransform))]
public class CameraFocusArea : AutoMonoBehaviour
{
    public Bounds Bounds => rect.GetWorldBounds();


    [AutoDefault, ReadOnly]
    public RectTransform rect;


    private void OnDrawGizmos() {
        var gizBounds = GetComponent<RectTransform>().GetWorldBounds();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(gizBounds.center, gizBounds.size);
    }
}