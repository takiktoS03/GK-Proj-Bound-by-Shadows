using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticleSystem2D))]
public class ParticleSystem2DEditor : Editor
{
    private Editor workingCopyEditor;
    private ParticleEffectPreset lastPreset = null;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ParticleSystem2D ps = (ParticleSystem2D)target;

        // ----------------------------
        // RENDERING
        // ----------------------------
        var layers = SortingLayer.layers.Select(l => l.name).ToArray();
        SerializedProperty sortingLayerProp = serializedObject.FindProperty("sortingLayer");
        SerializedProperty orderProp = serializedObject.FindProperty("orderInLayer");

        int index = Mathf.Max(0, System.Array.IndexOf(layers, sortingLayerProp.stringValue));
        EditorGUILayout.LabelField("Rendering", EditorStyles.boldLabel);
        index = EditorGUILayout.Popup("Sorting Layer", index, layers);
        sortingLayerProp.stringValue = layers[index];
        orderProp.intValue = EditorGUILayout.IntField("Order in Layer", orderProp.intValue);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxParticles"));

        EditorGUILayout.Space();

        // ----------------------------
        // PRESET
        // ----------------------------
        SerializedProperty presetProp = serializedObject.FindProperty("preset");

        EditorGUILayout.LabelField("Preset", EditorStyles.boldLabel);
        var newPresetObj = EditorGUILayout.ObjectField(
    "Preset File",
    presetProp.objectReferenceValue,
    typeof(ParticleEffectPreset),
    false
);

        if (newPresetObj != presetProp.objectReferenceValue)
        {
            presetProp.objectReferenceValue = newPresetObj;
            serializedObject.ApplyModifiedProperties();
        }


        ParticleEffectPreset newPreset = presetProp.objectReferenceValue as ParticleEffectPreset;

        // =============== TU JEST KLUCZ ===============
        // wykrywanie zmiany preset file
        if (newPreset != lastPreset)
        {
            if (newPreset != null)
            {
                ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                ps.overridePresetData.CopyFrom(newPreset);
                EditorUtility.SetDirty(ps);
            }
            else
            {
                ps.overridePresetData = null;
            }

            lastPreset = newPreset;

            GUI.FocusControl(null);
            Repaint();
        }

        EditorGUILayout.Space();

        // ---------------------------------------------
        // CREATE NEW PRESET
        // ---------------------------------------------
        if (GUILayout.Button("Create New Preset"))
        {
            string folderPath = "Assets/Presets";
            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder("Assets", "Presets");

            string path = EditorUtility.SaveFilePanelInProject(
                "Create Particle Effect Preset",
                "NewParticlePreset",
                "asset",
                "Enter preset name:",
                folderPath
            );

            if (!string.IsNullOrEmpty(path))
            {
                // tworzymy i kopiujemy z OBIEKTU
                var newFile = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                newFile.CopyFromSystem(ps);

                AssetDatabase.CreateAsset(newFile, path);
                AssetDatabase.SaveAssets();

                presetProp.objectReferenceValue = newFile;
                serializedObject.ApplyModifiedProperties();

                ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                ps.overridePresetData.CopyFrom(newFile);

                lastPreset = newFile;

                Selection.activeObject = newFile;
            }
        }

        EditorGUILayout.Space();

        // ---------------------------------------------
        // WORKING COPY
        // ---------------------------------------------
        if (ps.overridePresetData != null)
        {
            EditorGUILayout.LabelField("Preset Settings (Working Copy)", EditorStyles.boldLabel);

            if (workingCopyEditor == null || workingCopyEditor.target != ps.overridePresetData)
                workingCopyEditor = CreateEditor(ps.overridePresetData);

            workingCopyEditor.OnInspectorGUI();

            EditorGUILayout.Space();

            GUI.color = Color.cyan;
            if (GUILayout.Button("SAVE SETTINGS TO PRESET"))
            {
                var presetFile = presetProp.objectReferenceValue as ParticleEffectPreset;

                Undo.RecordObject(presetFile, "Save Preset");
                presetFile.CopyFrom(ps.overridePresetData);
                EditorUtility.SetDirty(presetFile);
                AssetDatabase.SaveAssets();

                UpdateAllSceneObjectsUsingPreset(presetFile);
            }
            GUI.color = Color.white;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateAllSceneObjectsUsingPreset(ParticleEffectPreset preset)
    {
        var all = Object.FindObjectsByType<ParticleSystem2D>(FindObjectsSortMode.None);

        foreach (var ps in all)
        {
            if (ps.preset == preset)
            {
                ps.overridePresetData.CopyFrom(preset);
                EditorUtility.SetDirty(ps.overridePresetData);
                EditorUtility.SetDirty(ps);
            }
        }
    }
}
