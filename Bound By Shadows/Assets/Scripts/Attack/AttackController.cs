using System.Collections;
using UnityEngine;

/**
 * @class AttackController
 * @brief Klasa odpowiedzialna za zarządzanie atakiem postaci (tworzeniem hitboxów, cooldownem).
 * 
 * Wykorzystywana razem z obiektami typu AttackData, zawiera logikę instancjowania prefabów ataku
 * oraz obsługę czasu odnowienia między atakami.
 * 
 * @author Filip Kudła
 */
public class AttackController : MonoBehaviour
{
    /**
     * @brief Punkt w którym pojawi się hitbox ataku.
     */
    [Header ("Lista Spawn-Pointów Ataków Postaci")]
    [SerializeField] private Transform attackHitboxSpawnPoint;

    private bool canAttack = true;

    /**
     * @brief Wywołuje atak na podstawie danych z AttackData.
     * 
     * @param data Obiekt przechowujący informacje o ataku (prefab, obrażenia, knockback, cooldown).
     */
    public void PerformAttack(AttackData data)
    {
        if (!canAttack) return;
        
        AttackMethod(data);
        StartCoroutine(AttackCooldownRoutine(data.cooldown));
    }

    /**
     * @brief Tworzy obiekt hitboxa i ustawia jego parametry.
     * 
     * @param data Dane ataku do zaaplikowania hitboxowi.
     */
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

    /**
     * @brief Coroutine blokująca możliwość ataku na czas cooldownu.
     * 
     * @param attackCooldown Czas w sekundach przez który atak jest niedostępny.
     */
    IEnumerator AttackCooldownRoutine(float attackCooldown)
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}

