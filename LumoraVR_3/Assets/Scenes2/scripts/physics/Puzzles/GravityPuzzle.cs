using UnityEngine;

public class GravityPuzzle : MonoBehaviour
{
    public Rigidbody floatingObject;
    public DoorController door;

    private bool solved = false;

    public void SetCorrectWeight()
    {
        if (solved) return;

        solved = true;

        // Simulate gravity stabilization
        floatingObject.useGravity = true;

        door.OpenDoor();
        RoomManager.Instance.CompleteRoom();
    }
}