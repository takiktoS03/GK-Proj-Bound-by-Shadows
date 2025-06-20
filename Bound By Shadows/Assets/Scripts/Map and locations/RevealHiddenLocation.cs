using System.Diagnostics;
using UnityEngine;

public class RevealHiddenLocation : MonoBehaviour
{

    public GameObject hiddenLocation;

    private void onTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("Trigger wszed?: " + other.name);

        if (other.CompareTag("LeafArea"))
        {
            UnityEngine.Debug.Log("To by? li??!");
            hiddenLocation.SetActive(true);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
