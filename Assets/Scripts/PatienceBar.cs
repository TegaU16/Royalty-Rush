using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [SerializeField] private Slider patienceSlider;

    public void SetPatience(float current, float max)
    {
        patienceSlider.maxValue = max;
        patienceSlider.value = current;
    }
}
