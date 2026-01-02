using UnityEditor;
using UnityEngine;

/**
 * @class ParticleEffectPresetEditor
 * @brief Custom Inspector dla klasy ParticleEffectPreset.
 *
 * Skrypt odpowiada za niestandardowe rysowanie inspektora presetu cz?steczek.
 * Dynamicznie wy?wietla pola emisji (promie? lub obszar) w zale?no?ci
 * od wybranego typu emisji.
 *
 * @author Julia Bigaj
 */
[CustomEditor(typeof(ParticleEffectPreset))]
public class ParticleEffectPresetEditor : Editor
{
    /**
     * @brief Rysuje niestandardowy interfejs Inspector.
     *
     * Wy?wietla pole wyboru kszta?tu emisji oraz odpowiednie parametry
     * (promie? lub obszar), a nast?pnie automatycznie rysuje pozosta?e
     * w?a?ciwo?ci presetu.
     */
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- EMISSION SHAPE ---
        var emissionShape = serializedObject.FindProperty("emissionShape");
        EditorGUILayout.PropertyField(emissionShape);

        EditorGUILayout.Space();

        // Wy?wietlanie odpowiedniego parametru w zale?no?ci od kszta?tu emisji
        var shape = (EmissionShape)emissionShape.enumValueIndex;

        if (shape == EmissionShape.Point || shape == EmissionShape.Circle)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("emissionRadius"));
        else
            EditorGUILayout.PropertyField(serializedObject.FindProperty("emissionArea"));

        EditorGUILayout.Space();

        // Automatyczne rysowanie pozosta?ych pól bez duplikacji
        DrawPropertiesExcluding(
            serializedObject,
            "m_Script",
            "emissionShape",
            "emissionRadius",
            "emissionArea"
        );

        serializedObject.ApplyModifiedProperties();
    }
}