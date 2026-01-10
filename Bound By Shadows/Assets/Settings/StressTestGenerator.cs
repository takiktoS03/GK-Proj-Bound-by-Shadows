using UnityEngine;

public class StressTestGenerator : MonoBehaviour
{
    [Header("Settings")]
    public GameObject tilePrefab;
    public int count = 2000;
    public float spacing = 1.5f; // Odstęp między kafelkami

    void Start()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Przypisz prefab kafelka!");
            return;
        }

        int rowSize = Mathf.CeilToInt(Mathf.Sqrt(count));
        Vector3 startPos = transform.position; // Punkt startowy to pozycja generatora

        for (int i = 0; i < count; i++)
        {
            // Obliczenie offsetu (przesunięcia) względem generatora
            float xOffset = (i % rowSize) * spacing;
            float yOffset = (i / rowSize) * spacing;

            Vector3 spawnPos = startPos + new Vector3(xOffset, yOffset, 0);

            // Instancjonowanie jako dziecko generatora (ostatni parametr 'transform')
            GameObject obj = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

            // Wymuszenie unikalności materiału dla SRP Batchera
            var effect = obj.GetComponent<DissolveEffect>();
            if (effect != null)
            {
                // Ustawiamy losową wartość, żeby MaterialPropertyBlock miał co robić
                effect.UpdateMaterial(Random.Range(0.0f, 1.0f));
            }
        }
    }
}