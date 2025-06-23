//using System.Diagnostics;
using UnityEngine;

/* Ten skrypt zarz¹dza dzia³aniem dŸwigni aktywowanej przez gracza.
   - Gdy gracz znajduje siê w zasiêgu i naciœnie "F", dŸwignia zmienia swój stan (on/off) i uruchamia odpowiedni¹ animacjê.
   - Informacja o stanie dŸwigni jest przechowywana w zmiennej `leverIsOn`.
   - W przysz³oœci funkcja `CheckRiddle()` mo¿e byæ wywo³ywana do sprawdzania poprawnoœci zagadki logicznej (LeverRiddle).

   Autor: Julia Bigaj
*/

public class LeafRevealer : MonoBehaviour
{
    public GameObject hiddenLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszed³ w liœcie!");
            hiddenLocation.SetActive(true);
        }
    }
}