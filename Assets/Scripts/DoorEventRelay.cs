using UnityEngine;

public class DoorEventRelay : MonoBehaviour
{
    public StudentsManager studentsManager;
    public AudioSource doorSound;

    public void HideStudent()
    {
        studentsManager.HideStudents();
    }

    public void PlayDoorSound()
    {
        doorSound.Play();
    }
}
