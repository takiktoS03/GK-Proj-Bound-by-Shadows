using UnityEngine;

/**
 * @class UIStateManager
 * @brief Przechowuje globalny stan interfejsu u�ytkownika (UI).
 *
 * Umo�liwia innym komponentom sprawdzenie, czy jaki� panel UI jest aktualnie otwarty.
 * Mo�e by� u�ywany np. do tymczasowego wy��czenia sterowania postaci�, gdy gracz korzysta z interfejsu.
 *
 * Klasa statyczna � dost�pna globalnie bez potrzeby tworzenia instancji.
 *
 * @author Julia Bigaj
 */
public static class UIStateManager
{
    /// @brief Czy UI (np. ekwipunek, skrzynia, dialog) jest aktualnie otwarty.
    public static bool isUIOpen = false;
}
