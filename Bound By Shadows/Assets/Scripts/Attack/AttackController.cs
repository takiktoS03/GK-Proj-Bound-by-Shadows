using System.Collections;
using UnityEngine;

/**
 * Logika ataku aktywowanego w animacji, używająca info o ataku z AttackData
 * 
 * Autor: Filip Kudła
 **/
public class AttackController : MonoBehaviour
{
    [Header ("Lista Spawn-Pointów Ataków Postaci")]
    [SerializeField] private Transform attackHitboxSpawnPoint;

    private bool canAttack = true;

    public void PerformAttack(AttackData data)
    {
        if (!canAttack) return;
        
        AttackMethod(data);
        StartCoroutine(AttackCooldownRoutine(data.cooldown));
    }

    private void AttackMethod(AttackData data)
    {
        GameObject hitboxObj = Instantiate(
            data.hitboxPrefab,
            attackHitboxSpawnPoint.position,
            Quaternion.identity,
            transform);

        // Skala w zależności od kierunku postaci
        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 scale = hitboxObj.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        hitboxObj.transform.localScale = scale;

        AttackHitbox hitbox = hitboxObj.GetComponent<AttackHitbox>();
        hitbox.Init(data.damage, data.knockback, gameObject);
        Destroy(hitboxObj, data.duration);
    }

    IEnumerator AttackCooldownRoutine(float attackCooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
