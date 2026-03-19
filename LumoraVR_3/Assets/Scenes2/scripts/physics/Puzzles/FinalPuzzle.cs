using UnityEngine;

public class FinalPuzzle : MonoBehaviour
{
    public DoorController door;

    public void SelectFormula(string formula)
    {
        if (formula == "KE")
        {
            Debug.Log("Correct Formula!");
            door.OpenDoor();
            RoomManager.Instance.CompleteRoom();
        }
        else
        {
            Debug.Log("Wrong Formula!");
        }
    }
}