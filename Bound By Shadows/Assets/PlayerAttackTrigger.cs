using UnityEngine;
using System.Collections;

public class PlayerAttackTrigger : MonoBehaviour
{
    public GameObject attackHitbox;
    public Vector2 offsetRight = new Vector2(0.5f, 0f);
    public Vector2 offsetLeft = new Vector2(-0.5f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            StartCoroutine(ActivateHitbox());
        }
    }

    void UpdateHitboxPosition()
    {
        // zak?adamy ?e obrót gracza to scale.x = 1 (prawo), -1 (lewo)
        bool facingRight = transform.localScale.x > 0;
        Vector2 offset = facingRight ? offsetRight : offsetLeft;

        attackHitbox.transform.localPosition = offset;
    }

    IEnumerator ActivateHitbox()
    {
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackHitbox.SetActive(false);
    }
}
