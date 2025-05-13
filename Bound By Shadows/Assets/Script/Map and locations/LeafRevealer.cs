using System.Diagnostics;
using UnityEngine;

public class LeafRevealer : MonoBehaviour
{
    public GameObject hiddenLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Gracz wszed? w li?cie!");
            hiddenLocation.SetActive(true);
        }
    }
}
