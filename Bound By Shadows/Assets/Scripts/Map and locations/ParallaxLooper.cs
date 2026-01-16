using UnityEngine;
using System.Collections.Generic;

/**
 * Skrypt odpowiedzialny za efekt paralaksy tła, polegający na przesuwaniu
 * wielu warstw w zależności od ruchu kamery.
 *
 * @author Filip Kudła
 */

public class ParallaxLooper : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public string name = "Warstwa";
        public Transform rootObject;
        [Range(0f, 1f)] public float parallaxSpeed;

        // --- Zmienne wewnętrzne ---
        [HideInInspector] public List<Transform> pieces = new List<Transform>();
        [HideInInspector] public float pieceWidth;
        [HideInInspector] public float totalChainWidth;
    }

    public Transform cameraTransform;
    [Tooltip("Ile dodatkowych ekranów zapasu generować po bokach? (Zalecane 1.0 = 100% szerokości ekranu)")]
    public float bufferMultiplier = 1.0f;
    public List<Layer> layers = new List<Layer>();

    private Vector3 lastCameraPosition;
    private float widthToCover;

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        // Szerokość ekranu w jednostkach świata gry (World Units)
        float screenHeightWorld = 2f * Camera.main.orthographicSize;
        float screenWidthWorld = screenHeightWorld * Camera.main.aspect;
        widthToCover = screenWidthWorld * bufferMultiplier;

        foreach (var layer in layers)
        {
            SetupLayer(layer);
        }
    }

    void SetupLayer(Layer layer)
    {
        if (layer.rootObject == null) return;
        SpriteRenderer sr = layer.rootObject.GetComponent<SpriteRenderer>();

        layer.pieceWidth = sr.bounds.size.x;
        layer.pieces.Clear();
        layer.pieces.Add(layer.rootObject);

        // Ile kopii potrzebujemy na JEDNĄ stronę, aby pokryć połowę żądanej szerokości
        int clonesPerSide = Mathf.CeilToInt((widthToCover * 0.5f) / layer.pieceWidth);

        // Generowanie klonów
        for (int i = 1; i <= clonesPerSide; i++)
        {
            Transform left = Instantiate(layer.rootObject, layer.rootObject.parent);
            left.position = layer.rootObject.position + Vector3.left * (layer.pieceWidth * i);
            layer.pieces.Add(left);

            Transform right = Instantiate(layer.rootObject, layer.rootObject.parent);
            right.position = layer.rootObject.position + Vector3.right * (layer.pieceWidth * i);
            layer.pieces.Add(right);
        }

        // Całkowita szerokość łańcucha (liczba wszystkich elementów * szerokość jednego)
        layer.totalChainWidth = layer.pieces.Count * layer.pieceWidth;
    }

    void LateUpdate()
    {
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;

        foreach (var layer in layers)
        {
            MoveLayerX(layer, deltaX);
        }

        lastCameraPosition = cameraTransform.position;
    }

    void MoveLayerX(Layer layer, float deltaX)
    {
        float moveAmount = deltaX * layer.parallaxSpeed;
        float threshold = layer.totalChainWidth / 2f; // Połowa długości łańcucha

        foreach (Transform piece in layer.pieces)
        {
            // Ruch na X
            Vector3 pos = piece.position;
            pos.x += moveAmount;
            piece.position = pos;

            // Zapętlanie (Przeskok)
            float dist = piece.position.x - cameraTransform.position.x;

            // Jeśli element wyjechał za daleko w lewo -> przenieś na prawy koniec łańcucha
            if (dist < -threshold)
                piece.position += Vector3.right * layer.totalChainWidth;

            // Jeśli element wyjechał za daleko w prawo -> przenieś na lewy koniec łańcucha
            else if (dist > threshold)
                piece.position -= Vector3.right * layer.totalChainWidth;
        }
    }
}