using UnityEngine;

public class TorchPuzzleManager : MonoBehaviour
{
    public static TorchPuzzleManager Instance;

    public int[] correctOrder;
    public GameObject objectToAppear;

    private TorchController[] torches;
    private int currentOrder = 0;

    public int GetNextOrderIndex()
    {
        return currentOrder++;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        torches = FindObjectsOfType<TorchController>();
    }

    public void TorchLit(int torchID)
    {
        if (AllTorchesLit())
        {
            if (CheckOrder())
            {
                Debug.Log("Zagadka rozwi?zana!");

                if (objectToAppear != null)
                    objectToAppear.SetActive(false);

                SoundManager.Instance.PlaySound(SoundManager.Instance.puzzleSolvedSound);
            }
            else
            {
                Debug.Log("Z?a kolejno?? - reset!");
                ResetAllTorches();
            }
        }
    }

    private bool AllTorchesLit()
    {
        foreach (var t in torches)
        {
            if (!t.IsLit)
                return false;
        }
        return true;
    }

    private bool CheckOrder()
    {
        // posortowane latarnie wed?ug kolejno?ci klikni?cia
        TorchController[] sorted = (TorchController[])torches.Clone();
        System.Array.Sort(sorted, (a, b) => a.orderIndex.CompareTo(b.orderIndex));

        // porównujemy torchID z correctOrder
        for (int i = 0; i < sorted.Length; i++)
        {
            if (sorted[i].torchID != correctOrder[i])
                return false;
        }

        return true;
    }

    private void ResetAllTorches()
    {
        currentOrder = 0;

        foreach (var t in torches)
            t.ResetTorch();
    }
}
