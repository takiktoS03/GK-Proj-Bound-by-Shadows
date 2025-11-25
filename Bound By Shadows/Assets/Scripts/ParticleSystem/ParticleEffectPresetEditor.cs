using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParticleEffectPreset))]
public class ParticleEffectPresetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- EMISSION SHAPE ---
        var emissionShape = serializedObject.FindProperty("emissionShape");
        EditorGUILayout.PropertyField(emissionShape);

        EditorGUILayout.Space();

        // radius / area – jedno z tych dwóch
        var shape = (EmissionShape)emissionShape.enumValueIndex;

        if (shape == EmissionShape.Point || shape == EmissionShape.Circle)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("emissionRadius"));
        else
            EditorGUILayout.PropertyField(serializedObject.FindProperty("emissionArea"));

        EditorGUILayout.Space();

        // --- AUTOMATYCZNE RYSOWANIE RESZTY ---
        DrawPropertiesExcluding(
            serializedObject,
            "m_Script",         // standard unity field
            "emissionShape",
            "emissionRadius",
            "emissionArea"
        );

        serializedObject.ApplyModifiedProperties();
    }
}