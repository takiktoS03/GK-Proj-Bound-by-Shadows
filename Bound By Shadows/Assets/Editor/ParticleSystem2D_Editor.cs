using System.Linq;
using UnityEditor;
using UnityEngine;

/**
 * @class ParticleSystem2DEditor
 * @brief Custom Inspector dla klasy ParticleSystem2D.
 *
 * Skrypt rozszerza Inspector systemu cz?steczek 2D o obs?ug? presetów,
 * kopii roboczej ustawie? (working copy) oraz parametrów renderowania.
 * Zapobiega resetowaniu ustawie? podczas edycji w edytorze.
 *
 * @author Julia Bigaj
 */
[CustomEditor(typeof(ParticleSystem2D))]
public class ParticleSystem2DEditor : Editor
{

    private Editor workingCopyEditor;

    /**
     * @brief Rysuje niestandardowy interfejs Inspector.
     *
     * Obs?uguje:
     * - ustawienia renderowania
     * - wybór presetu
     * - tworzenie nowego presetu
     * - edycj? kopii roboczej presetu
     */
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ParticleSystem2D ps = (ParticleSystem2D)target;

        // --- Rendering ---
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

        // --- Preset ---
        SerializedProperty presetProp = serializedObject.FindProperty("preset");

        EditorGUILayout.LabelField("Preset", EditorStyles.boldLabel);
        var newPresetObj = EditorGUILayout.ObjectField(
            "Preset File",
            presetProp.objectReferenceValue,
            typeof(ParticleEffectPreset),
            false
        );

        bool presetAssetReferenceChanged = newPresetObj != presetProp.objectReferenceValue;

        if (presetAssetReferenceChanged)
        {
            presetProp.objectReferenceValue = newPresetObj;
            serializedObject.ApplyModifiedProperties();
        }

        ParticleEffectPreset currentPresetAsset = presetProp.objectReferenceValue as ParticleEffectPreset;

        // Synchronizacja kopii roboczej z assetem presetu
        if (presetAssetReferenceChanged)
        {
            if (currentPresetAsset != null)
            {
                if (ps.overridePresetData == null)
                {
                    ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                }
                ps.overridePresetData.CopyFrom(currentPresetAsset);
                EditorUtility.SetDirty(ps);
            }
            else
            {
                ps.overridePresetData = null;
            }

            GUI.FocusControl(null);
            Repaint();
        }
        else if (currentPresetAsset != null && ps.overridePresetData == null)
        {
            ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
            ps.overridePresetData.CopyFrom(currentPresetAsset);
            EditorUtility.SetDirty(ps);

            GUI.FocusControl(null);
            Repaint();
        }

        EditorGUILayout.Space();

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
                var newFile = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                newFile.CopyFromSystem(ps);

                AssetDatabase.CreateAsset(newFile, path);
                AssetDatabase.SaveAssets();

                presetProp.objectReferenceValue = newFile;
                serializedObject.ApplyModifiedProperties();

                if (ps.overridePresetData == null)
                {
                    ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                }
                ps.overridePresetData.CopyFrom(newFile);

                Selection.activeObject = newFile;
            }
        }

        EditorGUILayout.Space();

        if (ps.overridePresetData != null)
        {
            EditorGUILayout.LabelField("Preset Settings (Working Copy)", EditorStyles.boldLabel);

            if (workingCopyEditor == null || workingCopyEditor.target != ps.overridePresetData)
                workingCopyEditor = CreateEditor(ps.overridePresetData);

            workingCopyEditor.OnInspectorGUI();

            EditorGUILayout.Space();

            GUI.color = Color.yellow;
            if (GUILayout.Button("Reset Settings to Preset Default"))
            {
                var presetFile = presetProp.objectReferenceValue as ParticleEffectPreset;
                if (presetFile != null)
                {
                    ps.overridePresetData.CopyFrom(presetFile);
                    EditorUtility.SetDirty(ps.overridePresetData);
                    Repaint();
                }
            }
            GUI.color = Color.white;
        }

        serializedObject.ApplyModifiedProperties();
    }

    /**
     * @brief Aktualizuje wszystkie obiekty ParticleSystem2D w scenie
     * korzystaj?ce z danego presetu.
     *
     * @param preset Preset, którego zmiany maj? zosta? zastosowane
     */
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
