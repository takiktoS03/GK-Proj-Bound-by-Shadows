using UnityEngine;
using System.Collections;

public class PlayerAttackTrigger : MonoBehaviour
{
    public GameObject attackHitbox;
    public Vector2 offsetRight = new Vector2(0.5f, 0f);
    public Vector2 offsetLeft = new Vector2(-0.5f, 0f);

    void Update()
    {
        UpdateHitboxPosition();
    }

    void UpdateHitboxPosition()
    {
        bool facingRight = transform.localScale.x > 0;
        Vector2 offset = facingRight ? offsetRight : offsetLeft;
        attackHitbox.transform.localPosition = offset;
    }

    public void ActivateHitbox() => StartCoroutine(ActivateHitboxCoroutine());

    private IEnumerator ActivateHitboxCoroutine()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
    }
}
