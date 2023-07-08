using System;
using System.Reflection;
using UnityEngine;

/**
 * Locates and automatically assigns the main camera to the field.
 * Only works if the component inherits from AutoMonoBehaviour instead of MonoBehaviour.
 */
[AttributeUsage(AttributeTargets.Field)]
public class AutoDefaultMainCameraAttribute : Attribute, IAutoAttribute
{
    public bool Apply(Component target, FieldInfo field) {
        if (field.FieldType != typeof(Camera)) return false;
        field.SetValue(target, Camera.main);
        return true;
    }
}