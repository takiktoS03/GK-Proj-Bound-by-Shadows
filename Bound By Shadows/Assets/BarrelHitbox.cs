using UnityEngine;

public class BarrelHitbox : MonoBehaviour
{
     public Barrel parentBarrel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hitbox wykry� co�: " + other.name + " | tag: " + other.tag);
        if (other.CompareTag("PlayerAttack"))
        {
            Debug.Log("Hitbox wykry� atak gracza!");
            parentBarrel.OnHit();
        }
    }
}
