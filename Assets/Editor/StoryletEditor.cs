using System.IO;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Storylet))]
[CanEditMultipleObjects]
public class StoryletEditor : Editor
{
    private static GUILayoutOption _contentBoxHeight = GUILayout.Height(100);
    private static GUILayoutOption _removeButtonHeight = GUILayout.Height(EditorGUIUtility.singleLineHeight);
    private static GUILayoutOption _removeButtonWidth = GUILayout.Width(20);
    
    private SerializedProperty _storyProperty;
    private SerializedProperty _weightProperty;
    private SerializedProperty _repeatableProperty;
    private SerializedProperty _preconditionsProperty;
    private SerializedProperty _contentProperty;
    private SerializedProperty _choicesProperty;

    private void OnEnable()
    {
        _storyProperty = serializedObject.FindProperty("story");
        _weightProperty = serializedObject.FindProperty("Weight");
        _repeatableProperty = serializedObject.FindProperty("Repeatable");
        _preconditionsProperty = serializedObject.FindProperty("Preconditions");
        _contentProperty = serializedObject.FindProperty("content");
        _choicesProperty = serializedObject.FindProperty("Choices");
    }

    public static void CreateAsset<TStorylet>() where TStorylet : ScriptableObject
    {
        TStorylet asset = ScriptableObject.CreateInstance<TStorylet>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(TStorylet).ToString() + ".asset");

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
        EditorGUILayout.PropertyField(_storyProperty);
        EditorGUILayout.PropertyField(_weightProperty);
        EditorGUILayout.PropertyField(_repeatableProperty);
        ShowConditions(_preconditionsProperty);
        EditorGUILayout.PropertyField(_contentProperty,_contentBoxHeight);
        ShowChoices();
        //EditorGUILayout.PropertyField(_choicesProperty, true);

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

    private void ShowChoices()
    {
        EditorGUILayout.PropertyField(_choicesProperty);

        ++EditorGUI.indentLevel;

        if (_choicesProperty.isExpanded)
        {
            for (var i = 0; i < _choicesProperty.arraySize; ++i)
            {
                var choice = _choicesProperty.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(choice);
                
                if(GUILayout.Button(new GUIContent("-", "remove element"), 
                    _removeButtonHeight, _removeButtonWidth))
                {
                    _choicesProperty.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndHorizontal();
                
                ++EditorGUI.indentLevel;
                if (choice.isExpanded)
                {
                    EditorGUILayout.PropertyField(choice.FindPropertyRelative("ChoiceText"));
                    ShowConditions(choice.FindPropertyRelative("Postconditions"));
                }
                --EditorGUI.indentLevel;
            }
            
            if(GUILayout.Button(new GUIContent("+", "add element")))
            {
                _choicesProperty.InsertArrayElementAtIndex(_choicesProperty.arraySize);
            }
        }

        --EditorGUI.indentLevel;
    }

    private void ShowConditions(SerializedProperty conditions)
    {
        EditorGUILayout.PropertyField(conditions);

        ++EditorGUI.indentLevel;

        if (conditions.isExpanded)
        {
            for (var i = 0; i < conditions.arraySize; ++i)
            {
                var precondition = conditions.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.PropertyField(precondition.FindPropertyRelative("Key"), GUIContent.none);
                EditorGUILayout.PropertyField(precondition.FindPropertyRelative("Type"), GUIContent.none);
                EditorGUILayout.PropertyField(precondition.FindPropertyRelative("Value"), GUIContent.none);

                if(GUILayout.Button(new GUIContent("-", "remove element"), 
                    _removeButtonHeight, _removeButtonWidth))
                {
                    conditions.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            if(GUILayout.Button(new GUIContent("+", "add element")))
            {
                conditions.InsertArrayElementAtIndex(conditions.arraySize);
            }
        }

        --EditorGUI.indentLevel;
    }
}
