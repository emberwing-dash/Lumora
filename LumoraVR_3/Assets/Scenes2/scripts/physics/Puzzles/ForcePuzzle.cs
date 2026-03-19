using UnityEngine;

public class ForcePuzzle : MonoBehaviour
{
    public Rigidbody block;
    public float requiredForce = 20f;

    private bool solved = false;
    public DoorController door;

    public void ApplyForce(float force)
    {
        if (solved) return;

        block.AddForce(Vector3.forward * force, ForceMode.Impulse);

        if (force >= requiredForce)
        {
            solved = true;
            door.OpenDoor();
            RoomManager.Instance.CompleteRoom();
        }
    }
}