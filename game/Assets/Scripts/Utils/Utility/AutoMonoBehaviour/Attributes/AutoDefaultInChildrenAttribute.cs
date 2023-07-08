using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Locates and automatically assigns a component on the current or any child GameObject to the field.
 * Only works if the component inherits from AutoMonoBehaviour instead of MonoBehaviour.
 */
[AttributeUsage(AttributeTargets.Field)]
public class AutoDefaultInChildrenAttribute : Attribute, IAutoAttribute
{
    public bool Apply(Component target, FieldInfo field) {
        object component = target.GetComponentInChildren(field.FieldType);
        if (component.IsUnityNull()) return false;
        field.SetValue(target, component);
        return true;
    }
}