using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzleManager : MonoBehaviour
{
    [Header("Konfiguracja")]
    [Tooltip("Kolejność w tej liście determinuje rozwiązanie zagadki")]
    public List<TorchController> torches;
    public GameObject objectToDisappear;

    [Header("Ustawienia")]
    [Tooltip("Czas w sekundach, po którym zagadka się resetuje przy błędzie.")]
    public float resetDelay = 0.5f;

    // Lista przechowująca to, co gracz aktualnie wyklikał
    private List<TorchController> _currentSelection = new List<TorchController>();
    private bool isPuzzleSolved = false;
    private bool isResetting = false; // Blokada klikania podczas resetu

    private void Start()
    {
        foreach (var torch in torches)
        {
            if (torch != null)
                torch.Initialize(this);
        }
    }

    public void OnTorchInteraction(TorchController interactedTorch)
    {
        if (isPuzzleSolved || isResetting) return;

        _currentSelection.Add(interactedTorch);

        if (_currentSelection.Count >= torches.Count)
        {
            CheckSequence();
        }
    }

    private void CheckSequence()
    {
        bool isCorrect = true;

        for (int i = 0; i < torches.Count; i++)
        {
            if (_currentSelection[i] != torches[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            PuzzleSolved();
        }
        else
        {
            StartCoroutine(ResetPuzzleRoutine());
        }
    }

    private void PuzzleSolved()
    {
        isPuzzleSolved = true;
        SoundLibrary.Instance.PlayPuzzleSolved();

        if (objectToDisappear != null)
            objectToDisappear.SetActive(false);
    }

    private IEnumerator ResetPuzzleRoutine()
    {
        isResetting = true;
        yield return new WaitForSeconds(resetDelay);

        foreach (var torch in _currentSelection)
        {
            torch.LightOff();
        }
        SoundLibrary.Instance.PlayTorch();

        _currentSelection.Clear();

        isResetting = false;
    }
}