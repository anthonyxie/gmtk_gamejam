using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Utility class that allows Image objects to fade in and out
 * Works on this game object and all children
 */
public class FadeImage : Fadable<Image>
{
    protected override List<Image> TargetComponents => GetComponentsInChildren<Image>().ToList();

    protected override float GetAlpha(Image component)
    {
        return component.color.a;
    }

    protected override void SetAlpha(Image component, float value)
    {
        component.color = new Color(component.color.r, component.color.g, component.color.b, value);
    }
}