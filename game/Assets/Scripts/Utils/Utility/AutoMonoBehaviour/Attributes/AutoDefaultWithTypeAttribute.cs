using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

/**
 * Locates and automatically assigns the first GameObject
 * in the scene with a component of the given type to this field.
 * 
 * Only works if the component inherits from AutoMonoBehaviour instead of MonoBehaviour.
 */
[AttributeUsage(AttributeTargets.Field)]
public class AutoDefaultWithTypeAttribute : Attribute, IAutoAttribute
{
    public bool Apply(Component target, FieldInfo field) {
        object found = Object.FindObjectOfType(field.FieldType);
        if (found.IsUnityNull()) return false;
        field.SetValue(target, found);
        return true;
    }
}