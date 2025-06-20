using UnityEngine;


[CreateAssetMenu(fileName = "NewAttackData", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header ("Rodzaj ataku")]
    public string attackName;
    [Space(10)]

    [Header ("Parametry ataku")]
    public float damage;
    public float knockback;
    public float cooldown;
    [Space(10)]

    [Header ("Parametry Hitboxa")]
    public GameObject hitboxPrefab;
    //public float activationDelay;
    public float duration;
}
