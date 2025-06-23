using UnityEngine;

/*
 Skrypt do wy�wietlania my�li bohatera po wej�ciu do specjalnej strefy.
 - Pokazuje podany tekst nad g�ow� postaci (np. dialog, przemy�lenie).
 - Bazuje na systemie `ThoughtUI`, kt�ry zarz�dza widoczno�ci� napisu.
 - Tekst jest wy�wietlany na kilka sekund i automatycznie znika.

 Autor: Julia Bigaj
*/

public class TriggerThoughtZone : MonoBehaviour
{
    [SerializeField] GameObject player;
    [TextArea]
    public string thought = "Nie ma innego wyj�cia... \r\nChyba musze po prostu tam skoczy� i zobaczy� co si� stanie";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //player.GetComponent<PlayerMovement>().disable;
            ThoughtUI thoughtUI = FindObjectOfType<ThoughtUI>();
            if (thoughtUI != null)
            {
                thoughtUI.ShowThought(thought, 3f); // poka� na 3 sekundy
            }
        }
    }
}
