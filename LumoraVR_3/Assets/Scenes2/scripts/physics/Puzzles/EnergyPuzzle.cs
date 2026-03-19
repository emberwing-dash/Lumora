using UnityEngine;

public class EnergyPuzzle : MonoBehaviour
{
    public PuzzleManager puzzleManager;

    public void ChooseAnswer(int index)
    {
        puzzleManager.SubmitAnswer(index);
    }
}