using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Deck))]
[CanEditMultipleObjects]
public class DeckEditor : Editor
{
    private SerializedProperty StoryletsProperty;

    private void OnEnable()
    {
        StoryletsProperty = serializedObject.FindProperty("Storylets");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(StoryletsProperty);

        ++EditorGUI.indentLevel;

        if (StoryletsProperty.isExpanded)
        {
            for (var i = 0; i < StoryletsProperty.arraySize; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.PropertyField(StoryletsProperty.GetArrayElementAtIndex(i), GUIContent.none);

                if(GUILayout.Button(new GUIContent("-", "remove element")))
                {
                    StoryletsProperty.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            if(GUILayout.Button(new GUIContent("+", "add element")))
            {
                StoryletsProperty.InsertArrayElementAtIndex(StoryletsProperty.arraySize);
            }
        }

        --EditorGUI.indentLevel;
        
        serializedObject.ApplyModifiedProperties();
    }
}
