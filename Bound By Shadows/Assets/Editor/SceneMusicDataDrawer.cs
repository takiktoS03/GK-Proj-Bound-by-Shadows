using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MusicManager.SceneMusicData))]
public class SceneMusicDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var sceneNameProp = property.FindPropertyRelative("sceneName");
        var musicProp = property.FindPropertyRelative("music");
        var volumeProp = property.FindPropertyRelative("volume");

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = 2f;

        // --- POLE SCENY ---
        Rect sceneRect = new Rect(position.x, position.y, position.width, lineHeight);

        // Jeśli scena została wcześniej zapisana, spróbuj załadować SceneAsset
        SceneAsset sceneAsset = null;

        if (!string.IsNullOrEmpty(sceneNameProp.stringValue))
        {
            string[] guids = AssetDatabase.FindAssets(sceneNameProp.stringValue + " t:Scene");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            }
        }

        // Pole wyboru sceny jako obiekt SceneAsset
        SceneAsset selectedScene = (SceneAsset)EditorGUI.ObjectField(
            sceneRect, "Scene", sceneAsset, typeof(SceneAsset), false
        );

        // Jeśli użytkownik przypisał scenę → zapisz nazwę sceny
        if (selectedScene != null)
        {
            sceneNameProp.stringValue = selectedScene.name;
        }
        else
        {
            sceneNameProp.stringValue = "";
        }

        // --- POLE MUZYKI ---
        Rect musicRect = new Rect(position.x, position.y + lineHeight + padding, position.width, lineHeight);
        EditorGUI.PropertyField(musicRect, musicProp);

        // --- POLE GŁOŚNOŚCI ---
        Rect volumeRect = new Rect(position.x, position.y + 2 * (lineHeight + padding), position.width, lineHeight);
        EditorGUI.PropertyField(volumeRect, volumeProp);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3 + 6f;
    }
}
