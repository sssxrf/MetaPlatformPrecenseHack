using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MobileUIManager : MonoBehaviour
{
    public static MobileUIManager Instance { get; private set; }

    #region SerializeField
    [SerializeField] private TMP_Text _messagesOnMobile;
    [SerializeField] private GameObject _wholeUI;

    [SerializeField] private GameObject _foodmode;
    [SerializeField] private GameObject _mapmode;

    // Score bar
    public int targetScore { get; set; } = 20; // The score needed to fill the progress bar.
    private int currentScore = 0;
    public Image progressBar;

    // Count down
    public float totalTime { get; set; }  = 90; // Total time in seconds (1 minute 30 seconds)
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool startCountdown;

    #endregion

    public GameObject Foodmode => _foodmode;
    public GameObject Mapmode => _mapmode;

    void Awake()
    {
       
        if (Instance == null)
        {
   
            Instance = this;
            
        }
        else
        {
            // If an instance already exists and it's not this one, destroy this one
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        remainingTime = totalTime;
        UpdateTimerDisplay();
        UpdateProgressBar();
    }

    void Update()
    {
        if (startCountdown)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                remainingTime = 0;
                TimerEnded();
            }
        }
        
    }

    public void UpdateMessages(string message)
    {
        _messagesOnMobile.text = message;
    }

    public void SetVisibleofUI(bool value)
    {
        _wholeUI.SetActive(value);
    }


    public void ChangeScore(int scoreToChange)
    {
        currentScore += scoreToChange;
        currentScore = Mathf.Clamp(currentScore, 0, targetScore); // Ensure score doesn't exceed max.
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = (float)currentScore / targetScore;
        }
        else
        {
            Debug.LogError("Progress bar image is not set.");
        }
    }

    public void StartCountDown()
    {
        startCountdown = true;
        remainingTime = totalTime;
    }


    private void UpdateTimerDisplay()
    {
        // Format the remaining time as mm:ss
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimerEnded()
    {
        startCountdown = false;
        Debug.Log("Game End!");
    }

}
