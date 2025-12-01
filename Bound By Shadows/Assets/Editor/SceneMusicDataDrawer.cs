using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MusicManager.SceneMusicData))]
public class SceneMusicDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var sceneGUIDProp = property.FindPropertyRelative("sceneGUID");
        var musicProp = property.FindPropertyRelative("music");
        var volumeProp = property.FindPropertyRelative("volume");

        Rect sceneRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Pobierz SceneAsset odpowiadaj?cy GUID
        SceneAsset sceneAsset = null;
        if (!string.IsNullOrEmpty(sceneGUIDProp.stringValue))
        {
            string path = AssetDatabase.GUIDToAssetPath(sceneGUIDProp.stringValue);
            sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        }

        // Wy?wietl pole sceny
        var selectedScene = (SceneAsset)EditorGUI.ObjectField(sceneRect, "Scene", sceneAsset, typeof(SceneAsset), false);

        if (selectedScene != null)
        {
            string path = AssetDatabase.GetAssetPath(selectedScene);
            sceneGUIDProp.stringValue = AssetDatabase.AssetPathToGUID(path);
        }
        else
        {
            sceneGUIDProp.stringValue = "";
        }

        // Muzyka
        Rect musicRect = new Rect(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(musicRect, musicProp);

        // Volume
        Rect volumeRect = new Rect(position.x, position.y + 40, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(volumeRect, volumeProp);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 60f; // trzy pola
    }
}
