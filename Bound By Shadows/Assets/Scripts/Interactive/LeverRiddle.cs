using UnityEngine;

public class LeverRiddle : MonoBehaviour
{
    [SerializeField] private GameObject hiddenTiles;
    [SerializeField] private GameObject lever1;
    [SerializeField] private GameObject lever2;
    [SerializeField] private GameObject lever3;

    private bool lever1On;
    private bool lever2On;
    private bool lever3On;

    public void CheckCorrectness()
    {
        hiddenTiles.SetActive(false);
        lever1On = lever1.GetComponent<LeverTrigger>().leverIsOn;
        lever2On = lever2.GetComponent<LeverTrigger>().leverIsOn;
        lever3On = lever3.GetComponent<LeverTrigger>().leverIsOn;

        if (lever1On && !lever2On && lever3On)
        {
            hiddenTiles.SetActive(true);
            // dzwiek przesuwania kamienia
            SoundManager.Instance?.PlayStone();
        }
    }
}
