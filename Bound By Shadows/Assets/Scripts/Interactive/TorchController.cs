using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    public int torchID;
    public Light2D torchLight;

    private bool playerIsNear = false;
    private bool isLit = false;

    // dost?pne z zewn?trz
    public bool IsLit => isLit;

    // numer kolejno?ci klikni?cia
    public int orderIndex = -1;

    private void Start()
    {
        if (torchLight != null)
            torchLight.enabled = false;
    }

    private void Update()
    {
        if (playerIsNear && !isLit && Input.GetKeyDown(KeyCode.F))
        {
            LightTorch();
        }
    }

    private void LightTorch()
    {
        isLit = true;

        if (torchLight != null)
            torchLight.enabled = true;

        SoundLibrary.Instance.PlayTorch();

        // zapisz kolejno?? zapalenia
        orderIndex = TorchPuzzleManager.Instance.GetNextOrderIndex();

        // powiadom puzzle manager
        TorchPuzzleManager.Instance.TorchLit(torchID);
    }

    public void ResetTorch()
    {
        isLit = false;
        orderIndex = -1;

        if (torchLight != null)
            torchLight.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsNear = false;
    }
}
