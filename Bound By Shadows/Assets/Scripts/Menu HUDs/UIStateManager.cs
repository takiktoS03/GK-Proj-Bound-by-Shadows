using UnityEngine;

/**
 * @class UIStateManager
 * @brief Przechowuje globalny stan interfejsu u¿ytkownika (UI).
 *
 * Umo¿liwia innym komponentom sprawdzenie, czy jakiœ panel UI jest aktualnie otwarty.
 * Mo¿e byæ u¿ywany np. do tymczasowego wy³¹czenia sterowania postaci¹, gdy gracz korzysta z interfejsu.
 *
 * Klasa statyczna — dostêpna globalnie bez potrzeby tworzenia instancji.
 *
 * @author Julia Bigaj
 */
public static class UIStateManager
{
    /// @brief Czy UI (np. ekwipunek, skrzynia, dialog) jest aktualnie otwarty.
    public static bool isUIOpen = false;
}
