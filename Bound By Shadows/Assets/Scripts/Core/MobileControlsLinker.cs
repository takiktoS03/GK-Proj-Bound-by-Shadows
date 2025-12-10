using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobileControlsLinker : MonoBehaviour
{
    [Header("Setup")]
    public GameObject visualsContainer;

    [Header("Mobile Inputs")]
    public Joystick joystick;
    public Button attackButton;
    public Button dashButton;

    [Header("Pokazuj kontrolki w edytorze")]
    public bool showInEditor = true;
    private void Awake()
    {
        bool isMobile = Application.platform == RuntimePlatform.Android;
        bool isEditorTesting = Application.isEditor && showInEditor;

        if (!isMobile && !isEditorTesting)
        {
            // Jeśli to PC (build) i nie testujemy -> niszczymy cały obiekt
            Destroy(gameObject);
            return;
        }
        if (visualsContainer != null) visualsContainer.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "BootScene" || scene.name == "Intro")
        {
            visualsContainer.SetActive(false); // Ukryj UI w menu
            return;
        }

        // Włącz UI w grze
        visualsContainer.SetActive(true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var movement = player.GetComponent<EthanTheHero.PlayerMovement>();
            movement.joystick = this.joystick;

            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(player.GetComponent<EthanTheHero.PlayerAttackMethod>().MobileAttackInput);

            dashButton.onClick.RemoveAllListeners();
            dashButton.onClick.AddListener(player.GetComponent<EthanTheHero.PlayerMovement>().MobileDashInput);

            Debug.Log($"Mobile UI linked to player in scene: {scene.name}");
        }
    }
}