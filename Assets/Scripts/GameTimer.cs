using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float durationInSeconds = 180f;

    private float remainingTime;
    private bool isRunning;

    public bool IsRunning => isRunning;
    public bool IsFinished => remainingTime <= 0f;

    private void Awake()
    {
        remainingTime = durationInSeconds;
        UpdateDisplay();
    }

    private void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isRunning = false;

            UpdateDisplay();
            TimerFinished();
            return;
        }

        UpdateDisplay();
    }

    public void StartTimer()
    {
        remainingTime = durationInSeconds;
        isRunning = true;
        UpdateDisplay();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        remainingTime = durationInSeconds;
        isRunning = false;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = $"{minutes}:{seconds:00}";
    }

    private void TimerFinished()
    {
        Debug.Log("Timer finished!");
    }
}