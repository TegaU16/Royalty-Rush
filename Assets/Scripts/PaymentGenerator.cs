using UnityEngine;

public class PaymentGenerator : MonoBehaviour
{
    public static PaymentGenerator Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private static readonly string[] RecipientNames =
    {
        "DJ Greg",
        "Pixel Pete",
        "Artist Anna",
        "Composer Mike",
        "Writer Sarah",
        "Sound Guy Steve"
    };

    public Payment GeneratePayment(int difficulty)
    {
        string recipient = RecipientNames[Random.Range(0, RecipientNames.Length)];

        float revenue;
        float royaltyPercent;

        switch (difficulty)
        {
            case 0:
                revenue = Random.Range(100, 1000);
                royaltyPercent = Random.Range(1, 10) * 1f;
                break;

            case 1:
                revenue = Random.Range(100, 5000);
                royaltyPercent = Random.Range(1, 20);
                break;

            default:
                revenue = Random.Range(100, 10000);
                royaltyPercent = Random.Range(1, 25);
                break;
        }

        revenue = Mathf.Round(revenue / 10f) * 10f;

        return new Payment(
            recipient,
            revenue,
            royaltyPercent
        );
    }
}
