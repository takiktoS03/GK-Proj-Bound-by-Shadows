using UnityEditor;
using UnityEngine;

/**
 * Niestandardowy edytor Inspector dla obiektu ItemSO,
 * umożliwiający dynamiczne wyświetlanie pól w zależności od typu przedmiotu.
 *
 * @author Julia Bigaj
 */
[CustomEditor(typeof(ItemSO))]
public class ItemSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemSO item = (ItemSO)target;

        // Podstawowe pola (zawsze widoczne)
        item.itemName = EditorGUILayout.TextField("Item Name", item.itemName);
        item.icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.icon, typeof(Sprite), false);
        item.itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", item.itemType);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Hotbar Settings", EditorStyles.boldLabel);
        item.canBeInHotbar = EditorGUILayout.Toggle("Can Be In Hotbar", item.canBeInHotbar);

        EditorGUILayout.Space(10);

        // ?? RÓ?NE POLA W ZALE?NO?CI OD TYPU ITEMU

        // =============================================
        //                LETTER
        // =============================================
        if (item.itemType == ItemType.Letter)
        {
            EditorGUILayout.LabelField("Letter Settings", EditorStyles.boldLabel);

            item.hasImagePreview = true;
            EditorGUILayout.LabelField("Image Preview:");
            item.imagePreview = (Sprite)EditorGUILayout.ObjectField(item.imagePreview, typeof(Sprite), false);

            // blokujemy tekstowe preview – wy??czone ca?kowicie
            item.hasTextPreview = false;
            item.textPreview = "";
        }

        // =============================================
        //                POTION
        // =============================================
        else if (item.itemType == ItemType.HealPotion)
        {
            EditorGUILayout.LabelField("Potion Settings", EditorStyles.boldLabel);

            // Heal Amount
            item.healAmount = EditorGUILayout.IntField("Heal Amount", item.healAmount);

            // Text Preview
            item.hasTextPreview = true;
            EditorGUILayout.LabelField("Text Preview:");
            item.textPreview = EditorGUILayout.TextArea(item.textPreview, GUILayout.Height(60));

            // Potion NIE MA obrazka preview
            item.hasImagePreview = false;
            item.imagePreview = null;
        }

        // =============================================
        //           STAMINA POTION
        // =============================================
        else if (item.itemType == ItemType.StaminaPotion)
        {
            EditorGUILayout.LabelField("Stamina Potion Settings", EditorStyles.boldLabel);

            // Stamina Amount
            item.staminaAmount = EditorGUILayout.IntField("Stamina Amount", item.staminaAmount);

            // Text Preview
            item.hasTextPreview = true;
            EditorGUILayout.LabelField("Text Preview:");
            item.textPreview = EditorGUILayout.TextArea(item.textPreview, GUILayout.Height(60));

            // Brak image preview
            item.hasImagePreview = false;
            item.imagePreview = null;
        }

        // =============================================
        //             DEFAULT (inne typy)
        // =============================================
        else
        {
            EditorGUILayout.LabelField("General Item Settings", EditorStyles.boldLabel);

            // pe?na kontrola
            item.hasTextPreview = EditorGUILayout.Toggle("Has Text Preview", item.hasTextPreview);
            if (item.hasTextPreview)
                item.textPreview = EditorGUILayout.TextArea(item.textPreview, GUILayout.Height(60));

            item.hasImagePreview = EditorGUILayout.Toggle("Has Image Preview", item.hasImagePreview);
            if (item.hasImagePreview)
                item.imagePreview = (Sprite)EditorGUILayout.ObjectField("Image Preview", item.imagePreview, typeof(Sprite), false);
        }

        // zapis zmian
        if (GUI.changed)
            EditorUtility.SetDirty(item);
    }
}
