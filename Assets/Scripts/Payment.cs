using System;

[Serializable]
public class Payment
{
    public string recipientName;
    public float revenue;
    public float royaltyPercent;

    public float CorrectPayment => revenue * (royaltyPercent / 100f);

    public Payment(string recipientName, float revenue, float royaltyPercent)
    {
        this.recipientName = recipientName;
        this.revenue = revenue;
        this.royaltyPercent = royaltyPercent;
    }
}
