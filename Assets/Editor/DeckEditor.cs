using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Deck))]
[CanEditMultipleObjects]
public class DeckEditor : Editor
{
    private static float _weightLabelWidthRatio = 0.5f;
    private static GUILayoutOption _removeButtonHeight = GUILayout.Height(EditorGUIUtility.singleLineHeight);
    private static GUILayoutOption _removeButtonWidth = GUILayout.Width(20);
    
    private SerializedProperty _storyletsProperty;
    private SerializedProperty _weightsProperty;

    private void OnEnable()
    {
        _storyletsProperty = serializedObject.FindProperty("storylets");
        _weightsProperty = serializedObject.FindProperty("weights");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_storyletsProperty);

        ++EditorGUI.indentLevel;

        if (_storyletsProperty.isExpanded)
        {
            for (var i = 0; i < _storyletsProperty.arraySize; ++i)
            {
                var storylet = _storyletsProperty.GetArrayElementAtIndex(i);
                var weight = _weightsProperty.GetArrayElementAtIndex(i);
                
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.PropertyField(storylet, GUIContent.none);
                
                
                EditorGUIUtility.labelWidth *= _weightLabelWidthRatio;
                EditorGUILayout.PropertyField(weight, new GUIContent("Weight:"));
                EditorGUIUtility.labelWidth = 0f;
                
                if(GUILayout.Button(new GUIContent("-", "remove element"), 
                    _removeButtonHeight, _removeButtonWidth))
                {
                    if (storylet.objectReferenceValue != null)
                        _storyletsProperty.DeleteArrayElementAtIndex(i);
                    
                    _storyletsProperty.DeleteArrayElementAtIndex(i);
                    _weightsProperty.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            if(GUILayout.Button(new GUIContent("+", "add element")))
            {
                _storyletsProperty.InsertArrayElementAtIndex(_storyletsProperty.arraySize);
                _weightsProperty.InsertArrayElementAtIndex(_weightsProperty.arraySize);
            }
        }

        --EditorGUI.indentLevel;

        serializedObject.ApplyModifiedProperties();
    }
}
