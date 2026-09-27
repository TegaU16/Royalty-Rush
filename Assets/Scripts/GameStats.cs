using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    public int Score { get; private set; }

    public int CustomersServed { get; private set; }
    public int CustomersSatisfied { get; private set; }
    public int CustomersAngry { get; private set; }
    public int CustomersKilled { get; private set; }

    public int CorrectPayments { get; private set; }
    public int IncorrectPayments { get; private set; }

    public float TotalRevenueProcessed { get; private set; }
    public float TotalRoyaltiesPaid { get; private set; }

    [SerializeField] private int satisfiedScore = 100;
    [SerializeField] private int angryScore = -50;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CustomerSatisfied(Payment payment)
    {
        CustomersServed++;
        CustomersSatisfied++;
        CorrectPayments++;

        TotalRevenueProcessed += payment.revenue;
        TotalRoyaltiesPaid += payment.CorrectPayment;

        Score += satisfiedScore;
    }

    public void CustomerAngry()
    {
        CustomersServed++;
        CustomersAngry++;
        IncorrectPayments++;

        Score += angryScore;
    }

    public void CustomerKilled()
    {
        CustomersKilled++;
    }

    public void DisplayStats()
    {

    }
}
