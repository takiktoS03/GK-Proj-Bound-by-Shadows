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
    [SerializeField] GameObject player;
    [TextArea]
    public string thought = "Nie ma innego wyjœcia... \r\nChyba musze po prostu tam skoczyæ i zobaczyæ co siê stanie";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //player.GetComponent<PlayerMovement>().disable;
            ThoughtUI thoughtUI = FindObjectOfType<ThoughtUI>();
            if (thoughtUI != null)
            {
                thoughtUI.ShowThought(thought, 3f); // poka¿ na 3 sekundy
            }
        }
    }
}
