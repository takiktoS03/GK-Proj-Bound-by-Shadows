using System.Collections;
using UnityEngine;

/**
 * Filip Kudła
 * Logika ataku aktywowanego w animacji, uzywajaca info o ataku z AttackData
 **/
public class AttackController : MonoBehaviour
{
    //public GameObject attackHitbox;
    //[Header ("Spawn point musi być z prawej!")]
    [SerializeField] private Transform attack01HitboxSpawnPoint;
    private bool canAttack = true;

    public void PerformAttack(AttackData data)
    {
        if (!canAttack) return;

        //UpdateHitboxPosition(data);
        AttackCoroutine(data);
        StartCoroutine(AttackCooldownRoutine(data.cooldown));
    }

    private void AttackCoroutine(AttackData data)
    {
        GameObject hitboxObj = Instantiate(
            data.hitboxPrefab,
            attack01HitboxSpawnPoint.position,
            Quaternion.identity,
            transform);

        // Skala w zaleznosci od kierunku postaci
        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 scale = hitboxObj.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        hitboxObj.transform.localScale = scale;

        AttackHitbox hitbox = hitboxObj.GetComponent<AttackHitbox>();
        hitbox.Init(data.damage, data.knockback, gameObject);
        //yield return new WaitForSeconds(data.duration);
        Destroy(hitboxObj, data.duration);
    }

    IEnumerator AttackCooldownRoutine(float attackCooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void UpdateHitboxPosition(AttackData data)
    {
        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 scale = data.hitboxPrefab.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        data.hitboxPrefab.transform.localScale = scale;
        if (direction == -1) 
        {
            var pos = attack01HitboxSpawnPoint.localPosition.x * direction;
            attack01HitboxSpawnPoint.localPosition = new Vector3(attack01HitboxSpawnPoint.localPosition.x * direction, attack01HitboxSpawnPoint.localPosition.y, attack01HitboxSpawnPoint.localPosition.z);
        }
    }
}
