using UnityEngine;

public class MotionPuzzle : MonoBehaviour
{
    public PuzzleManager puzzleManager;

    // Called by VR button clicks
    public void ChooseAnswer(int index)
    {
        puzzleManager.SubmitAnswer(index);
    }
}