using EthanTheHero;
using UnityEngine;

/*
 Skrypt do wyœwietlania myœli bohatera po wejœciu do specjalnej strefy.
 - Pokazuje podany tekst nad g³ow¹ postaci (np. dialog, przemyœlenie).
 - Bazuje na systemie `ThoughtUI`, który zarz¹dza widocznoœci¹ napisu.
 - Tekst jest wyœwietlany na kilka sekund i automatycznie znika.

 Autor: Julia Bigaj
*/

public class TriggerThoughtZone : MonoBehaviour
{
    [TextArea]
    public string thought = "Nie ma innego wyjścia... \r\nChyba musze po prostu tam skoczyć i zobaczyć co się stanie";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.wallSlidingEnabled = false;

            TextUI thoughtUI = FindObjectOfType<TextUI>();
            if (thoughtUI != null)
            {
                thoughtUI.ShowTextDialog(thought, 3f, 1); // pokaż na 3 sekundy
            }
        }
    }
}
