using System.Collections;
using UnityEngine;

public class SwordAbilityController : MonoBehaviour
{
    [Header("Ustawienia Umiejętności")]
    public KeyCode activationKey = KeyCode.R;
    public float abilityDuration = 10f;
    public float cooldownDuration = 5f;
    public float damageBonus = 0.2f;

    [Header("Wygląd Miecza (Buff)")]
    [ColorUsage(true, true)]
    public Color buffColor = Color.red;
    public float intensity = 4f;

    private SpriteRenderer _spriteRenderer;
    private Material _swordMaterial;
    private AttackController _attackController;
    private Color _originalColor;
    private float _originalIntensity;

    private bool _isAbilityActive = false;
    private bool _onCooldown = false;

    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    private static readonly int EmissionIntensityID = Shader.PropertyToID("_EmissionIntensity");

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _attackController = GetComponent<AttackController>();
        _swordMaterial = _spriteRenderer.material;

        if (_swordMaterial.HasProperty(EmissionColorID))
            _originalColor = _swordMaterial.GetColor(EmissionColorID);

        if (_swordMaterial.HasProperty(EmissionIntensityID))
            _originalIntensity = _swordMaterial.GetFloat(EmissionIntensityID);
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationKey) && !_isAbilityActive && !_onCooldown)
        {
            StartCoroutine(ActivateAbilityRoutine());
        }
    }

    private IEnumerator ActivateAbilityRoutine()
    {
        _isAbilityActive = true;

        _attackController.damageMultiplier = 1.0f + damageBonus;
        ApplyMaterialProperties(buffColor, intensity);

        yield return new WaitForSeconds(abilityDuration);

        _attackController.damageMultiplier = 1.0f;
        ApplyMaterialProperties(_originalColor, _originalIntensity);

        _isAbilityActive = false;
        _onCooldown = true;

        yield return new WaitForSeconds(cooldownDuration);
        _onCooldown = false;
    }

    private void ApplyMaterialProperties(Color color, float intensity)
    {
        if (_swordMaterial != null)
        {
            _swordMaterial.SetColor(EmissionColorID, color);
            _swordMaterial.SetFloat(EmissionIntensityID, intensity);
        }
    }
}