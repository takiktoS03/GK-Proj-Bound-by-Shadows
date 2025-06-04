using UnityEngine;
using System.Collections;

/**
 * Filip Kudła
 * Logika Hitboxu aktywowanego w momencie ataku gracza
 **/

public class PlayerAttackTrigger : MonoBehaviour
{
    public GameObject attackHitbox;

    void Update()
    {
        UpdateHitboxPosition();
    }

    void UpdateHitboxPosition()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        Vector3 scale = attackHitbox.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        attackHitbox.transform.localScale = scale;
    }

    public void ActivateHitbox() => StartCoroutine(ActivateHitboxCoroutine());

    private IEnumerator ActivateHitboxCoroutine()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
    }
}
