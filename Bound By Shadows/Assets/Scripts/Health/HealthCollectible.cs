using UnityEngine;

/**
 * @class HealthCollectible
 * @brief Skrypt odpowiedzialny za przedmiot przywracaj�cy zdrowie graczowi.
 *
 * Po zebraniu przez gracza, przywraca okre�lon� ilo�� zdrowia i usuwa si� ze sceny.
 *
 * @author Filip Kud�a
 */
public class HealthCollectible : MonoBehaviour
{
    /// @brief Ilo�� zdrowia do przywr�cenia.
    [SerializeField] private float healthValue;

    /**
     * Wykrywa wej�cie gracza i przywraca mu zdrowie.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Health>().Heal(healthValue);
        gameObject.SetActive(false);
    }
}
