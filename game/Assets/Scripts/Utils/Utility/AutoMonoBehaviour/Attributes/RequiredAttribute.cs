using System;

/**
 * Marks the given field as Required, and will surface a warning in the console proactively
 * if it is not filled out in the editor.
 * 
 * Only works if the component inherits from AutoMonoBehaviour instead of MonoBehaviour.
 */
[AttributeUsage(AttributeTargets.Field)]
public class RequiredAttribute : Attribute
{ }