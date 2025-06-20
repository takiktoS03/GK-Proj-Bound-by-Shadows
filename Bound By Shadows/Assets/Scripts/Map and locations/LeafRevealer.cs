//using System.Diagnostics;
using UnityEngine;

public class LeafRevealer : MonoBehaviour
{
    public GameObject hiddenLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszed³ w liœcie!");
            hiddenLocation.SetActive(true);
        }
    }
}