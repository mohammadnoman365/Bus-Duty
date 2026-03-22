using UnityEngine;

public class BusControl : MonoBehaviour
{
    public Animator DoorLeft;
    public Animator DoorRight;
    public Animator StopSign1;
    public Animator StopSign3;

    private bool isOpen = false; 

    public void Open(bool open)
    {
        string action = open ? "Open" : "Close";

        DoorLeft.SetTrigger(action);
        DoorRight.SetTrigger(action);
        StopSign1.SetTrigger(action);
        StopSign3.SetTrigger(action);

        isOpen = open; 
    }

    public void ToggleDoor()
    {
        Open(!isOpen);
    }
}
