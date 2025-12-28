using UnityEngine;
using System.Collections.Generic;

public class ParallaxLooper : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public string name = "Nazwa Warstwy";
        public Transform rootObject;
        [Range(0f, 1f)] public float parallaxMultiplier;

        // Zmienne prywatne do obsługi logiki tej konkretnej warstwy
        [HideInInspector] public Transform[] segments;
        [HideInInspector] public float spriteWidth;
    }

    [Header("Ustawienia Ogólne")]
    public Transform cameraTransform;
    public float bufferPercent = 0.01f;

    [Header("Lista Warstw")]
    public List<ParallaxLayer> layers = new List<ParallaxLayer>();

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        // Inicjalizacja każdej warstwy z listy
        foreach (var layer in layers)
        {
            SetupLayer(layer);
        }
    }

    void SetupLayer(ParallaxLayer layer)
    {
        if (layer.rootObject == null) return;

        SpriteRenderer sr = layer.rootObject.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError($"Warstwa {layer.name} nie ma SpriteRenderera!");
            return;
        }

        layer.spriteWidth = sr.bounds.size.x;

        layer.segments = new Transform[2];
        layer.segments[0] = layer.rootObject;

        Transform clone = Instantiate(layer.rootObject, layer.rootObject.parent);
        clone.position = layer.rootObject.position + Vector3.right * layer.spriteWidth;

        layer.segments[1] = clone;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        lastCameraPosition = cameraTransform.position;

        foreach (var layer in layers)
        {
            MoveLayer(layer, delta);
        }
    }

    void MoveLayer(ParallaxLayer layer, Vector3 delta)
    {
        if (layer.rootObject == null) return;

        foreach (Transform segment in layer.segments)
        {
            segment.position += delta * layer.parallaxMultiplier;
        }

        // Logika nieskończonego przewijania (check bounds)
        float buffer = layer.spriteWidth * bufferPercent;

        foreach (Transform segment in layer.segments)
        {
            float distance = cameraTransform.position.x - segment.position.x;

            if (distance > layer.spriteWidth + buffer)
            {
                // Przesuwamy w prawo o 2 szerokości (bo mamy 2 segmenty)
                segment.position += Vector3.right * layer.spriteWidth * 2;
            }
            else if (distance < -layer.spriteWidth - buffer)
            {
                segment.position -= Vector3.right * layer.spriteWidth * 2;
            }
        }
    }
}