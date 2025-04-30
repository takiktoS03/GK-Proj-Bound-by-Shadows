using UnityEngine;

public class BarrelHitbox : MonoBehaviour
{
     public Barrel parentBarrel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hitbox wykry³ coœ: " + other.name + " | tag: " + other.tag);
        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("Hitbox wykry³ atak gracza!");
            parentBarrel.OnHit();
        }
    }
}
