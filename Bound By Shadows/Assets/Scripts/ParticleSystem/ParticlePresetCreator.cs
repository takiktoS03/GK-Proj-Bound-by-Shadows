using UnityEngine;
using UnityEditor;

public static class ParticlePresetCreator
{
    [MenuItem("Assets/Create/Particle System 2D/Effect Preset")]
    public static void CreatePreset()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Create Particle Effect Preset",
            "NewParticlePreset",
            "asset",
            "Choose location"
        );

        if (string.IsNullOrEmpty(path))
            return;

        var preset = ScriptableObject.CreateInstance<ParticleEffectPreset>();
        AssetDatabase.CreateAsset(preset, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = preset;
    }
}
