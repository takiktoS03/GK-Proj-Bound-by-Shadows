using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public GameObject image1;
    public GameObject image2;

    void Awake()
    {
        image1.SetActive(false);
        image2.SetActive(false);
    }

    void Start()
    {
        MusicManager.Instance.StopMusic();
        MusicManager.Instance.PlayIntroAudio();

        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        image1.SetActive(true);
        yield return new WaitForSeconds(10f);
        image1.SetActive(false);

        image2.SetActive(true);
        yield return new WaitForSeconds(16f);
        image2.SetActive(false);

        SceneManager.LoadScene("Level 1 - Cave");
    }
}
