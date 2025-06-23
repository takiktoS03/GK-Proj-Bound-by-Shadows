//using System.Diagnostics;
using UnityEngine;

/* Ten skrypt zarz�dza dzia�aniem d�wigni aktywowanej przez gracza.
   - Gdy gracz znajduje si� w zasi�gu i naci�nie "F", d�wignia zmienia sw�j stan (on/off) i uruchamia odpowiedni� animacj�.
   - Informacja o stanie d�wigni jest przechowywana w zmiennej `leverIsOn`.
   - W przysz�o�ci funkcja `CheckRiddle()` mo�e by� wywo�ywana do sprawdzania poprawno�ci zagadki logicznej (LeverRiddle).

   Autor: Julia Bigaj
*/

public class LeafRevealer : MonoBehaviour
{
    public GameObject hiddenLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszed� w li�cie!");
            hiddenLocation.SetActive(true);
        }
    }
}