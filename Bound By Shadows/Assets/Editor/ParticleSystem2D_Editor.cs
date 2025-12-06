using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticleSystem2D))]
public class ParticleSystem2DEditor : Editor
{
    private Editor workingCopyEditor;
    // private ParticleEffectPreset lastPreset = null;

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

        // Flaga wykrywaj?ca, czy u?ytkownik zmieni? referencj? do Assetu Presetu
        bool presetAssetReferenceChanged = newPresetObj != presetProp.objectReferenceValue;

        if (presetAssetReferenceChanged)
        {
            presetProp.objectReferenceValue = newPresetObj;
            serializedObject.ApplyModifiedProperties();
        }

        ParticleEffectPreset currentPresetAsset = presetProp.objectReferenceValue as ParticleEffectPreset;

        // ==========================================================
        // LOGIKA PERSYSTENCJI (Klucz do rozwi?zania problemu resetu)
        // ==========================================================

        if (presetAssetReferenceChanged)
        {
            // KROK 1: Je?li Asset Presetu zosta? zmieniony, nadpisujemy Working Copy
            if (currentPresetAsset != null)
            {
                if (ps.overridePresetData == null)
                {
                    ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                }
                // ZAWSZE kopiujemy z nowego Assetu do Working Copy
                ps.overridePresetData.CopyFrom(currentPresetAsset);
                EditorUtility.SetDirty(ps);
            }
            else // U?ytkownik ustawi? Preset na null
            {
                ps.overridePresetData = null;
            }

            GUI.FocusControl(null);
            Repaint();
        }
        // KROK 2: Je?li Asset Presetu istnieje, ale Working Copy jest null
        // (np. po za?adowaniu sceny lub ponownym klikni?ciu w hierarchii), utwórz kopi?.
        // Dzi?ki temu, je?li ps.overridePresetData ISTNIEJE (trzymaj?c Twoje zmiany), ten blok si? NIE WYKONA.
        else if (currentPresetAsset != null && ps.overridePresetData == null)
        {
            ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
            ps.overridePresetData.CopyFrom(currentPresetAsset);
            EditorUtility.SetDirty(ps);

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
                var newFile = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                newFile.CopyFromSystem(ps);

                AssetDatabase.CreateAsset(newFile, path);
                AssetDatabase.SaveAssets();

                // Ustaw nowo utworzony Asset jako Preset File
                presetProp.objectReferenceValue = newFile;
                serializedObject.ApplyModifiedProperties();

                // Zaktualizuj Working Copy nowymi danymi
                if (ps.overridePresetData == null)
                {
                    ps.overridePresetData = ScriptableObject.CreateInstance<ParticleEffectPreset>();
                }
                ps.overridePresetData.CopyFrom(newFile);

                // USUNI?TO: lastPreset = newFile;

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

            // To pozwala edytowa? tylko KOPI? ustawie?
            workingCopyEditor.OnInspectorGUI();

            EditorGUILayout.Space();

            // Tutaj mo?esz doda? przycisk "Resetuj do Presetu"
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
