using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Base class for scripts that allows a bunch of convenient attributes
 * to be used that automatically default, find, and verify fields.
 */
public abstract class AutoMonoBehaviour : MonoBehaviour
{
    protected virtual void OnValidate() {
        foreach (var field in GetType().GetFields()) {
            object value = field.GetValue(this);
            if (!value.IsUnityNull() && !field.GetType().IsArray) continue;

            var attributes = field.GetCustomAttributes(true);
            bool applied =
                attributes.Any(attr => attr is IAutoAttribute auto && auto.Apply(this, field));
            bool required = attributes.Any(attr => attr is RequiredAttribute);

            if (!applied && required && isActiveAndEnabled) {
                Debug.LogWarning(
                    $@"<b><color=""red"">Warning!</color></b> Field <b>{field.Name} ({field.FieldType.Name})</b> in {name}'s {GetType().Name} component is marked as required, but is missing!",
                    this);
            }
        }
    }
}