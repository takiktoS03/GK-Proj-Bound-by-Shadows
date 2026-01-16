using UnityEngine;

/**
 * Statyczna klasa przechowująca globalną informację o stanie interfejsu użytkownika,
 * wykorzystywaną do blokowania lub modyfikowania logiki gry.
 *
 * @author Julia Bigaj
 */

public static class UIStateManager
{
    /// @brief Czy UI (np. ekwipunek, skrzynia, dialog) jest aktualnie otwarty.
    public static bool isUIOpen = false;
}

