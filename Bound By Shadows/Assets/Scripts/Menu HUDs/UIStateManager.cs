using UnityEngine;

/* Przechowuje globalny stan UI (czy interfejs jest aktualnie otwarty).
   - Wykorzystywany do blokowania sterowania gracza podczas otwartych paneli.

   Autor: Julia Bigaj
*/

public static class UIStateManager
{
    public static bool isUIOpen = false;
}
