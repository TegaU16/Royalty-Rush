using Game.UI;
using UnityEngine;

public class CalculatorManager : MonoBehaviour
{
    public static CalculatorManager Instance;

    [SerializeField] private KeyCode calculatorKey = KeyCode.C;
    [SerializeField] private KeyCode clearCalculatorKey = KeyCode.X;

    [SerializeField] private GameObject calculatorObj;
    private RectTransform calculatorRect;
    private Calculator calculator;

    [SerializeField] private Vector2 calcOffScreen;
    [SerializeField] private Vector2 calcOnScreen;

    private bool isOpen;

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
        calculator = calculatorObj.GetComponent<Calculator>();
        calculatorRect = calculatorObj.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(calculatorKey))
            ToggleCalculator(!isOpen);

        if (isOpen && Input.GetKeyDown(clearCalculatorKey))
            calculator.ClearCalculator();
    }

    public void ToggleCalculator(bool open)
    {
        if (!GameManager.Instance.IsReady) return;

        isOpen = open;
        calculatorObj.SetActive(open);

        if (open)
            UITween.SlideIn(calculatorRect, calcOffScreen, calcOnScreen);
        else
            UITween.SlideIn(calculatorRect, calcOnScreen, calcOffScreen);
    }
}
