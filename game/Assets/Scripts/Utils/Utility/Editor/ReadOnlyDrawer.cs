using UnityEditor;
using UnityEngine;


/**
 * https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/
 * This is the magic behind the ReadOnlyAttribute. Makes the attribute
 * read-only in editor mode, and red (but read-writable) in play mode.
 * Useful for debugging.
 */
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var previousGUIState = GUI.enabled;
        var previousGUIColor = GUI.contentColor;

        using (new EditorGUI.DisabledScope(Application.isPlaying))
        {
            GUI.enabled = Application.isPlaying;
            GUI.contentColor = Application.isPlaying ? Color.red : previousGUIColor;
            EditorGUI.PropertyField(position, property, label);
            GUI.contentColor = previousGUIColor;
            GUI.enabled = previousGUIState;
        }
    }
}