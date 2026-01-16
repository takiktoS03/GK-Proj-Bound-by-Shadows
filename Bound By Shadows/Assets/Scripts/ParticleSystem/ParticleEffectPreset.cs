using UnityEngine;
using System.Reflection;

/**
 * Autorski preset efektu cząsteczek oparty o ScriptableObject.
 * Skrypt przechowuje kompletną konfigurację systemu cząsteczek,
 * w tym parametry emisji, fizyki, kolizji oraz wyglądu cząsteczek.
 *
 * Presety umożliwiają łatwe tworzenie, edycję i ponowne użycie efektów
 * wizualnych w różnych miejscach projektu bez duplikowania danych.
 * Klasa wspiera kopiowanie ustawień pomiędzy presetami
 * oraz bezpośrednią synchronizację z systemem cząsteczek.
 *
 * @author Julia Bigaj
 */

[CreateAssetMenu(fileName = "ParticleEffectPreset", menuName = "Particles/Effect Preset")]
public class ParticleEffectPreset : ScriptableObject
{
    public Material particleMaterial;
    public Sprite particleSprite;

    [Header("Basic parameters")]
    public float particleSize = 0.2f;
    public int emissionRate = 20;
    public float particleLifetime = 2f;

    [Header("Speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float directionAngle = 90f;
    public float spread = 30f;

    [Header("Emission shape")]
    public EmissionShape emissionShape = EmissionShape.Point;
    public float emissionRadius = 1f;
    public Vector2 emissionArea = new Vector2(2f, 1f);

    [Header("Color and scaling")]
    public Gradient colorOverLifetime;
    public AnimationCurve scaleOverLifetime;
    public AnimationCurve alphaOverLifetime;

    [Header("Physics")]
    public bool enableGravity = true;
    public Vector2 gravity = new Vector2(0f, -9.81f);
    public float airResistance = 0.98f;
    public float bounceFactor = 0.5f;
    public Vector2 wind = Vector2.zero;

    [Header("Collision")]
    public bool useCollision = false;
    public ParticleCollisionMode collisionMode = ParticleCollisionMode.Stop;
    public LayerMask collisionMask = ~0;

    /**
     * @brief Kopiuje dane z innego presetu.
     *
     * @param other Preset ?ród?owy
     */
    public void CopyFrom(ParticleEffectPreset other) => CopyFromPreset(other);

    /**
     * @brief Kopiuje dane presetu do systemu cz?steczek.
     *
     * @param system Docelowy system cz?steczek
     */
    public void CopyTo(ParticleSystem2D system) => CopyToSystem(system);

    /**
     * @brief Inicjalizuje domy?lne warto?ci gradientów i krzywych.
     */
    private void OnEnable()
    {
        if (colorOverLifetime == null || colorOverLifetime.colorKeys.Length == 0)
        {
            colorOverLifetime = new Gradient
            {
                colorKeys = new[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                },
                alphaKeys = new[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            };
        }

        if (scaleOverLifetime == null || scaleOverLifetime.keys.Length == 0)
            scaleOverLifetime = AnimationCurve.Linear(0, 1, 1, 1);

        if (alphaOverLifetime == null || alphaOverLifetime.keys.Length == 0)
            alphaOverLifetime = AnimationCurve.Linear(0, 1, 1, 0);
    }


    public void CopyFromPreset(ParticleEffectPreset other)
    {
        CopyFieldsFrom(other, this);
    }


    public void CopyToSystem(ParticleSystem2D system)
    {
        CopyFieldsFrom(this, system);
    }

    /**
     * @brief Kopiuje dane z systemu cz?steczek do presetu.
     *
     * @param sys System cz?steczek
     */
    public void CopyFromSystem(ParticleSystem2D sys)
    {
        CopyFieldsFrom(sys, this);
    }

    /**
     * @brief Kopiuje dane z jednego obiektu do drugiego.
     *
     * @param source Obiekt ?ród?owy
     * @param target Obiekt docelowy
     */
    private static void CopyFieldsFrom(object source, object target)
    {
        var flags = BindingFlags.Public | BindingFlags.Instance;

        foreach (var field in source.GetType().GetFields(flags))
        {
            var targetField = target.GetType().GetField(field.Name, flags);
            if (targetField != null)
                targetField.SetValue(target, field.GetValue(source));
        }

        foreach (var prop in source.GetType().GetProperties(flags))
        {
            if (!prop.CanRead || !prop.CanWrite) continue;

            var targetProp = target.GetType().GetProperty(prop.Name, flags);
            if (targetProp != null)
                targetProp.SetValue(target, prop.GetValue(source));
        }
    }
}
