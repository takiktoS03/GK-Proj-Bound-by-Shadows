using UnityEngine;

/**
 * @class AttackData
 * @brief ScriptableObject przechowujące dane dotyczące ataku.
 *
 * Ten obiekt zawiera informacje o nazwie ataku, jego obrażeniach,
 * sile odrzutu, czasie odnowienia, oraz prefabie hitboxa i czasie jego trwania.
 * Używany w systemie walki do konfiguracji poszczególnych ataków.
 *
 * @author Filip Kudła
 */
[CreateAssetMenu(fileName = "NewAttackData", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    /// @brief Nazwa ataku
    [Header ("Rodzaj ataku")]
    public string attackName;
    [Space(10)]

    [Header ("Parametry ataku")]
    /// @brief Ilość obrażeń zadawanych przez atak.
    public float damage;
    /// @brief Siła odrzutu zadawana przeciwnikowi.
    public float knockback;
    /// @brief Czas odnowienia ataku w sekundach.
    public float cooldown;
    [Space(10)]

    [Header ("Parametry Hitboxa")]
    /// @brief Prefab obiektu hitboxa generowanego w momencie ataku.
    public GameObject hitboxPrefab;
    /// @brief Czas trwania hitboxa w sekundach.
    public float duration;
}

