using System;
using System.Collections;
using Game.Audio;
using Game.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds2 = new WaitForSeconds(2f);
    private static readonly WaitForSeconds _waitForSeconds0_5 = new(0.5f);
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    public static GameManager Instance;

    [SerializeField] private GameTimer gameTimer;

    [SerializeField] private int waitTime;
    [SerializeField] private TextMeshProUGUI countdownText;
    private CanvasGroup countdownCG;

    [SerializeField] private AudioClip countDown;

    [SerializeField] private RectTransform calculatorSection;
    [SerializeField] private RectTransform cameraToggleSection;
    [SerializeField] private RectTransform timerSection;
    [SerializeField] private RectTransform swordSection;

    private CanvasGroup calculatorCG;
    private CanvasGroup cameraCG;
    private CanvasGroup timerCG;
    private CanvasGroup swordCG;

    [Header("Warnings")]
    public RectTransform bookModeSwing;
    private CanvasGroup bookModeSwingCG;

    public RectTransform charges;
    private CanvasGroup chargesCG;

    public event Action OnReady;

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
        timerCG = timerSection.gameObject.GetComponent<CanvasGroup>();
        swordCG = swordSection.gameObject.GetComponent<CanvasGroup>();
        bookModeSwingCG = bookModeSwing.gameObject.GetComponent<CanvasGroup>();
        chargesCG = charges.gameObject.GetComponent<CanvasGroup>();

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
                UITween.PopIn(swordSection, swordCG);
            else
                UITween.PopIn(cameraToggleSection, cameraCG);

            UITween.PopIn(countdownText.rectTransform, countdownCG);
            countdownText.text = count.ToString();
            count--;

            yield return _waitForSeconds1;
        }

        UITween.PopIn(timerSection, timerCG);
        UITween.PopIn(countdownText.rectTransform, countdownCG);
        countdownText.text = "GO!";
        yield return _waitForSeconds1;

        countdownText.gameObject.SetActive(false);
        IsReady = true;
        gameTimer.StartTimer();

        OnReady?.Invoke();
    }

    public IEnumerator BookModeSwing()
    {
        UITween.PopIn(bookModeSwing, bookModeSwingCG);

        yield return _waitForSeconds2;

        UITween.FadeOut(bookModeSwingCG);
    }

    public IEnumerator Charges()
    {
        UITween.PopIn(charges, chargesCG);

        yield return _waitForSeconds2;

        UITween.FadeOut(chargesCG);
    }
}
