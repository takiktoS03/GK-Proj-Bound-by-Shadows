using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    [Header("Komponenty")]
    public Light2D torchLight;

    [HideInInspector] public bool IsLit { get; private set; }

    private TorchPuzzleManager _manager;
    private bool playerIsNear;

    public void Initialize(TorchPuzzleManager manager)
    {
        _manager = manager;
        LightOff();
    }

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.F))
        {
            if (!IsLit && _manager != null)
            {
                LightOn();
                _manager.OnTorchInteraction(this);
            }
        }
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

    public void LightOn()
    {
        IsLit = true;
        SoundLibrary.Instance.PlayTorch();
        if (torchLight != null) torchLight.enabled = true;
    }

    public void LightOff()
    {
        IsLit = false;
        if (torchLight != null) torchLight.enabled = false;
    }
}