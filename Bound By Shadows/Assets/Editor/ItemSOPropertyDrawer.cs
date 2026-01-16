using UnityEditor;
using UnityEngine;


/**
 * Niestandardowy PropertyDrawer dla typu ItemSO,
 * umożliwiający tworzenie nowych assetów ItemSO bezpośrednio z poziomu Inspector’a.
 *
 * @author Julia Bigaj
 */
[CustomPropertyDrawer(typeof(ItemSO))]
public class ItemSOPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool hasObject = property.objectReferenceValue != null;
        return hasObject
            ? EditorGUIUtility.singleLineHeight + 4
            : EditorGUIUtility.singleLineHeight * 2 + 10;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect fieldRect = new Rect(
            position.x,
            position.y,
            position.width,
            EditorGUIUtility.singleLineHeight
        );

        Object oldValue = property.objectReferenceValue;

        // Rysujemy pole ObjectField
        Object newValue = EditorGUI.ObjectField(fieldRect, label, oldValue, typeof(ItemSO), false);

        // ------------------------------------------------------
        //  TRIGGER tylko gdy Unity stworzy NOWY element listy!
        //  (gdy oldValue == null oraz newValue == null NA START)
        // ------------------------------------------------------
        if (oldValue == null && property.objectReferenceValue == null)
        {
            if (IsNewListItem(property))
            {
                // Zostawiamy puste — poprawne zachowanie
            }
        }

        property.objectReferenceValue = newValue;

        // Je?eli pusty ? dodaj przycisk CREATE
        if (property.objectReferenceValue == null)
        {
            Rect buttonRect = new Rect(
                position.x,
                position.y + EditorGUIUtility.singleLineHeight + 4,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            if (GUI.Button(buttonRect, "Create new ItemSO"))
            {
                string path = EditorUtility.SaveFilePanelInProject(
                    "Create Item",
                    "New Item",
                    "asset",
                    "Select save location"
                );

                if (!string.IsNullOrEmpty(path))
                {
                    ItemSO newItem = ScriptableObject.CreateInstance<ItemSO>();
                    AssetDatabase.CreateAsset(newItem, path);
                    AssetDatabase.SaveAssets();

                    property.objectReferenceValue = newItem;
                }
            }
        }

        EditorGUI.EndProperty();
    }
    private bool IsNewListItem(SerializedProperty property)
    {
        if (!property.propertyPath.Contains("Array.data"))
            return false;

        SerializedProperty root = property.GetArrayPropertyRoot();
        int index = property.GetArrayIndex();

        return index == root.arraySize - 1;
    }

}

public static class SerializedPropertyArrayExtensions
{
    // Pobiera indeks elementu w tablicy: Array.data[x]
    public static int GetArrayIndex(this SerializedProperty property)
    {
        string path = property.propertyPath;
        int start = path.IndexOf("Array.data[") + "Array.data[".Length;
        int end = path.IndexOf("]", start);
        string number = path.Substring(start, end - start);
        int index;
        int.TryParse(number, out index);
        return index;
    }

    // Pobiera root tablicy (np. Chest.items)
    public static SerializedProperty GetArrayPropertyRoot(this SerializedProperty property)
    {
        string path = property.propertyPath;

        int arrayIndex = path.IndexOf(".Array.data[");
        if (arrayIndex < 0) return property; // fallback

        string rootPath = path.Substring(0, arrayIndex);

        SerializedObject obj = property.serializedObject;
        return obj.FindProperty(rootPath);
    }
}

