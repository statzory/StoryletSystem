using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;


[CustomEditor(typeof(Storylet))]
[CanEditMultipleObjects]
public class StoryletEditor : Editor
{
    private SerializedProperty StoryProperty;
    private SerializedProperty WeightProperty;
    private SerializedProperty RepeatableProperty;
    private SerializedProperty PreconditionsProperty;
    private SerializedProperty ContentProperty;
    private SerializedProperty ChoicesProperty;

    private void OnEnable()
    {
        StoryProperty = serializedObject.FindProperty("story");
        WeightProperty = serializedObject.FindProperty("Weight");
        RepeatableProperty = serializedObject.FindProperty("Repeatable");
        PreconditionsProperty = serializedObject.FindProperty("Preconditions");
        ContentProperty = serializedObject.FindProperty("content");
        ChoicesProperty = serializedObject.FindProperty("Choices");
    }

    public static void CreateAsset<Storylet>() where Storylet : ScriptableObject
    {
        Storylet asset = ScriptableObject.CreateInstance<Storylet>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(Storylet).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    
    public override void OnInspectorGUI()
    {
        var storylet = (Storylet)target;

        EditorGUI.BeginChangeCheck();
        
        storylet.CardArt = (Sprite) EditorGUILayout.ObjectField(new GUIContent("Card Art"), storylet.CardArt, typeof(Sprite), false);
        EditorGUILayout.PropertyField(StoryProperty);
        EditorGUILayout.PropertyField(WeightProperty);
        EditorGUILayout.PropertyField(RepeatableProperty);
        ShowPreconditions();
        EditorGUILayout.PropertyField(ContentProperty,GUILayout.Height(100));
        EditorGUILayout.PropertyField(ChoicesProperty, true);

        serializedObject.ApplyModifiedProperties();
        
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(storylet);
            AssetDatabase.SaveAssets();
            Repaint();
        }
    }
    
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        var storylet = (Storylet)target;

        if (storylet == null || storylet.CardArt == null)
            return null;
        
        Texture2D tex = new Texture2D (width, height);
        EditorUtility.CopySerialized (storylet.CardArt.texture, tex);

        return tex;
    }

    private void ShowPreconditions()
    {
        EditorGUILayout.PropertyField(PreconditionsProperty);

        ++EditorGUI.indentLevel;

        if (PreconditionsProperty.isExpanded)
        {
            for (var i = 0; i < PreconditionsProperty.arraySize; ++i)
            {
                var precondition = PreconditionsProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.PropertyField(precondition.FindPropertyRelative("Key"), GUIContent.none);
                EditorGUILayout.PropertyField(precondition.FindPropertyRelative("Type"), GUIContent.none);
                EditorGUILayout.PropertyField(precondition.FindPropertyRelative("Value"), GUIContent.none);

                if(GUILayout.Button(new GUIContent("-", "remove element")))
                {
                    PreconditionsProperty.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            if(GUILayout.Button(new GUIContent("+", "add element")))
            {
                PreconditionsProperty.InsertArrayElementAtIndex(PreconditionsProperty.arraySize);
            }
        }

        --EditorGUI.indentLevel;
    }
}
