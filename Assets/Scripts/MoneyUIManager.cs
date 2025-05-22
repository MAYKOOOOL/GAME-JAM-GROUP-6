using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyUIManager : MonoBehaviour
{
    public TextMeshProUGUI progressText;
    public Slider progressSlider;

    public int goal = 5;

    public void UpdateUI(int current)
    {
        progressText.text = $"Money Bags: {current} / {goal}";
        if (progressSlider != null)
        {
            progressSlider.maxValue = goal;
            progressSlider.value = current;
        }
    }
}
