using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * Utility class that allows 3D objects to fade in and out
 * (if their material allows them to).
 * Works on this game object and all children
 */
public class Fade3D : Fadable<Material>
{
    protected override List<Material> TargetComponents =>
        GetComponentsInChildren<Renderer>().Select(r => r.material).ToList();

    protected override float GetAlpha(Material component)
    {
        return component.color.a;
    }

    protected override void SetAlpha(Material component, float value)
    {
        component.color = new Color(component.color.r, component.color.g, component.color.b, value);
    }
}