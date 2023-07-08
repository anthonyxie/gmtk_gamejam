using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/**
 * Utility class that allows text objects to fade in and out
 * (if their material allows them to).
 * Works on this game object and all children
 */
public class FadeText : Fadable<TextMeshPro>
{
    protected override List<TextMeshPro> TargetComponents => GetComponentsInChildren<TextMeshPro>().ToList();

    protected override float GetAlpha(TextMeshPro component)
    {
        return component.color.a;
    }

    protected override void SetAlpha(TextMeshPro component, float value)
    {
        component.color = new Color(component.color.r, component.color.g, component.color.b, value);
    }
}