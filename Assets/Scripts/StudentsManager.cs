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
        // Initilizing the variables at start of the game
        studentsCollected = 0;
        busHealth = 100;
        studentsCollectedText.text = "Students on Bus: " + studentsCollected.ToString();
        busHealthText.text = "Bus Health: " + busHealth.ToString();
        UpdateUI();
        winningPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Show winning panel when 30 students are collected
        if (studentsCollected >= 30)
        {
            audioSource.PlayOneShot(winningSound);
            winningPanel.SetActive(true);
        }
    }

    // Collision detection with building and car
    private float lastDamageTime;
    private float damageCooldown = 0.5f; // Half a second delay

    void OnCollisionEnter(Collision collision)
    {
        // Prevents multiple damage instances in a short time
        if (Time.time - lastDamageTime < damageCooldown)
            return; 

        if (collision.gameObject.CompareTag("HealthLowObject"))
        {

            crashSound.Play();
            busHealth -= 10;
            // Update last damage time
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


    // Trigger detection with road signs to assign students to the bus
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

    // Funtion to hide students
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

    // Function to update the UI
    void UpdateUI()
    {
        studentsCollectedText.text = "Students on Bus: " + studentsCollected;

        Debug.Log("Students in Bus: " + studentsCollected);

        busHealthText.text = "Bus Health: " + busHealth;
    }

}
