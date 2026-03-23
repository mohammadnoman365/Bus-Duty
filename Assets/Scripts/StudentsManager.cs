using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StudentsManager : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI studentsCollectedText;
    public TextMeshProUGUI busHealthText;
    public GameObject collisionText;

    [Header("Students")]
    public GameObject[] students;
    private string studentTag = "NoTagAssign";

    public AudioSource crashSound;
    public GameObject doorButton;

    public AudioSource audioSource;
    public GameObject winningPanel;
    public AudioClip winningSound;
    public GameObject gameOverPanel;
    public AudioClip gameOverSound;

    int studentsCollected;
    int busHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doorButton.SetActive(false);
        studentsCollected = 0;
        busHealth = 100;
        studentsCollectedText.text = "Students on Bus: " + studentsCollected.ToString();
        busHealthText.text = "Bus Health: " + busHealth.ToString();
        UpdateUI();
        winningPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (studentsCollected >= 30)
        {
            audioSource.PlayOneShot(winningSound);
            winningPanel.SetActive(true);
        }
    }

    private float lastDamageTime;
    private float damageCooldown = 0.5f;

    void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return; 

        if (collision.gameObject.CompareTag("HealthLowObject"))
        {

            crashSound.Play();
            busHealth -= 10;
            lastDamageTime = Time.time; 

            collisionText.SetActive(true);
            Invoke(nameof(HideCollisionText), 1f);
            UpdateUI();

            if (busHealth <= 0)
            {
                audioSource.PlayOneShot(gameOverSound);
                gameOverPanel.SetActive(true);
            }
        }
    }

    void HideCollisionText()
    {
        collisionText.SetActive(false); 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BusRoadSign1"))
        {
            doorButton.SetActive(true);
            studentTag = "Students1";
        }
        else if (other.CompareTag("BusRoadSign2"))
        {
            doorButton.SetActive(true);
            studentTag = "Students2";
        }
        else if (other.CompareTag("BusRoadSign3"))
        {
            doorButton.SetActive(true);
            studentTag = "Students3";
        }
        else if (other.CompareTag("BusRoadSign4"))
        {
            doorButton.SetActive(true);
            studentTag = "Students4";
        }
        else if (other.CompareTag("BusRoadSign5"))
        {
            doorButton.SetActive(true);
            studentTag = "Students5";
        }
        else if (other.CompareTag("BusRoadSign6"))
        {
            doorButton.SetActive(true);
            studentTag = "Students6";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BusRoadSign1") || other.CompareTag("BusRoadSign2") || other.CompareTag("BusRoadSign3") ||
            other.CompareTag("BusRoadSign4") || other.CompareTag("BusRoadSign5") || other.CompareTag("BusRoadSign6"))
        {
            doorButton.SetActive(false);
            studentTag = "NoTagAssign";
        }
    }

    public void HideStudents()
    {
        students = GameObject.FindGameObjectsWithTag(studentTag);
        
        foreach (GameObject student in students)
        {
            student.SetActive(false);
        }

        studentsCollected += students.Length;

        UpdateUI();
    }

    void UpdateUI()
    {
        studentsCollectedText.text = "Students on Bus: " + studentsCollected;

        Debug.Log("Students in Bus: " + studentsCollected);

        busHealthText.text = "Bus Health: " + busHealth;
    }

}
