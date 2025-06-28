using UnityEngine;

/**
 * @class UIStateManager
 * @brief Przechowuje globalny stan interfejsu użytkownika (UI).
 *
 * Umożliwia innym komponentom sprawdzenie, czy jakiś panel UI jest aktualnie otwarty.
 * Może być używany np. do tymczasowego wyłączenia sterowania postacią, gdy gracz korzysta z interfejsu.
 *
 * Klasa statyczna — dostępna globalnie bez potrzeby tworzenia instancji.
 *
 * @author Julia Bigaj
 */
public static class UIStateManager
{
    /// @brief Czy UI (np. ekwipunek, skrzynia, dialog) jest aktualnie otwarty.
    public static bool isUIOpen = false;
}

