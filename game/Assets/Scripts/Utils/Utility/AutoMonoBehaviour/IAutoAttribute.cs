using System.Reflection;
using UnityEngine;

/**
 * Template for the various Auto* attributes that automatically
 * locate and apply default values to fields
 */
public interface IAutoAttribute
{
    public bool Apply(Component target, FieldInfo field);
}