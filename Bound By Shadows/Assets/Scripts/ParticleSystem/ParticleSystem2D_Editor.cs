using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(ParticleSystem2D))]
public class ParticleSystem2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ParticleSystem2D ps = (ParticleSystem2D)target;

        // sorting layers
        var layers = SortingLayer.layers.Select(l => l.name).ToArray();
        SerializedProperty sortingLayerProp = serializedObject.FindProperty("sortingLayer");
        SerializedProperty orderProp = serializedObject.FindProperty("orderInLayer");

        int index = Mathf.Max(0, System.Array.IndexOf(layers, sortingLayerProp.stringValue));

        EditorGUILayout.LabelField("Rendering", EditorStyles.boldLabel);
        index = EditorGUILayout.Popup("Sorting Layer", index, layers);
        sortingLayerProp.stringValue = layers[index];

        orderProp.intValue = EditorGUILayout.IntField("Order in Layer", orderProp.intValue);

        GUILayout.Space(10);

        // preset
        EditorGUILayout.PropertyField(serializedObject.FindProperty("preset"));

        GUILayout.Space(10);

        if (ps.preset != null)
        {
            if (GUILayout.Button("Apply Preset"))
            {
                ps.ApplyPreset();
                EditorUtility.SetDirty(ps);
            }
        }

        GUILayout.Space(10);
        DrawPropertiesExcluding(serializedObject, "preset");

        serializedObject.ApplyModifiedProperties();
    }
}
