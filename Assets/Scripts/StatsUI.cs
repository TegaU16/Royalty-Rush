using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text satisfiedText;
    [SerializeField] private TMP_Text angryText;
    [SerializeField] private TMP_Text killedText;
    [SerializeField] private TMP_Text correctPaymentsText;
    [SerializeField] private TMP_Text incorrectPaymentsText;

    private void OnEnable()
    {
        GameStats stats = GameStats.Instance;

        scoreText.text = "Score: " + stats.Score.ToString("N0");

        satisfiedText.text = stats.CustomersSatisfied.ToString();
        angryText.text = stats.CustomersAngry.ToString();
        killedText.text = stats.CustomersKilled.ToString();

        correctPaymentsText.text = stats.CorrectPayments.ToString();
        incorrectPaymentsText.text = stats.IncorrectPayments.ToString();
    }
}
