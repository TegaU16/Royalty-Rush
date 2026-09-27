using TMPro;
using UnityEngine;

public class PaymentUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipientText;
    [SerializeField] private TMP_Text revenueText;
    [SerializeField] private TMP_Text royaltyText;

    [SerializeField] private TMP_InputField paymentInput;
    [SerializeField] private KeyCode submitKey = KeyCode.Return;

    private Payment currentPayment;
    private Book book;

    private void Start()
    {
        book = GetComponentInParent<Book>();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsReady) return;

        if (Input.GetKeyDown(submitKey))
            SubmitPayment();
    }

    public void SetPayment(Payment payment)
    {
        currentPayment = payment;

        recipientText.text = payment.recipientName;
        revenueText.text = $"Revenue\n<color=#ff0000>${payment.revenue:F2}</color>";
        royaltyText.text = $"Royalty\n<color=#ff0000>{payment.royaltyPercent:F2}%</color>";

        paymentInput.text = "";
    }

    public void SubmitPayment()
    {
        if (currentPayment == null) return;

        if (!float.TryParse(paymentInput.text, out float enteredAmount))
        {
            book.FinishPayment(false, currentPayment);
            currentPayment = null;
            return;
        }

        float correctAmount = currentPayment.CorrectPayment;
        bool paymentCorrect = Mathf.Abs(enteredAmount - correctAmount) < 0.01f;

        book.FinishPayment(paymentCorrect, currentPayment);

        currentPayment = null;
    }
}
