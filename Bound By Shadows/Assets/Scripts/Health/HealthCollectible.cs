using UnityEngine;

/**
 * Skrypt pozwalajacy zbierac graczowi mikstury zycia 
 * Autor: Filip Kudla
 */
public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Health>().Heal(healthValue);
        gameObject.SetActive(false);
    }
}
