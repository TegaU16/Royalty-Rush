using System.Collections;
using Game.Audio;
using Game.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_5 = new(0.5f);
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    public static GameManager Instance;

    [SerializeField] private int waitTime;
    [SerializeField] private TextMeshProUGUI countdownText;
    private CanvasGroup countdownCG;

    [SerializeField] private AudioClip countDown;

    [SerializeField] private RectTransform calculatorSection;
    [SerializeField] private RectTransform cameraToggleSection;

    private CanvasGroup calculatorCG;
    private CanvasGroup cameraCG;

    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsReady = false;

        countdownCG = countdownText.gameObject.GetComponent<CanvasGroup>();
        calculatorCG = calculatorSection.gameObject.GetComponent<CanvasGroup>();
        cameraCG = cameraToggleSection.gameObject.GetComponent<CanvasGroup>();

        CameraController.Instance.OnIntroComplete += StartCountdown;
    }

    private void StartCountdown()
    {
        AudioManager.Instance.PlaySFX(countDown);
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        yield return _waitForSeconds0_5;

        int count = waitTime;
        countdownText.gameObject.SetActive(true);
        
        for (int i = 0; i < waitTime; i++)
        {
            if (i == 0)
                UITween.PopIn(calculatorSection, calculatorCG);
            else if (i == 1)
                UITween.PopIn(calculatorSection, calculatorCG);
            else
                UITween.PopIn(cameraToggleSection, cameraCG);

            UITween.PopIn(countdownText.rectTransform, countdownCG);
            countdownText.text = count.ToString();
            count--;

            yield return _waitForSeconds1;
        }

        UITween.PopIn(countdownText.rectTransform, countdownCG);
        countdownText.text = "GO!";
        yield return _waitForSeconds1;

        countdownText.gameObject.SetActive(false);
        IsReady = true;
    }
}
