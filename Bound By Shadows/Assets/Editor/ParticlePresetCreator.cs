using UnityEngine;
using UnityEditor;
/**
 * @class ParticlePresetCreator
 * @brief Narz?dzie edytora do tworzenia presetów efektów cz?steczek.
 *
 * Skrypt dodaje opcj? do menu Unity umo?liwiaj?c? utworzenie nowego
 * assetu ParticleEffectPreset w wybranej lokalizacji projektu.
 *
 * @author Julia Bigaj
 */
public static class ParticlePresetCreator
{
    /**
     * @brief Tworzy nowy preset efektu cz?steczek.
     *
     * Otwiera okno zapisu pliku, tworzy ScriptableObject typu
     * ParticleEffectPreset i zapisuje go jako asset w projekcie.
     */
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
