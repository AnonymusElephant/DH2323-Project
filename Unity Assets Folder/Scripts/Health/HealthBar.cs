using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        if (slider == null)
        {
            Debug.LogError("Slider component is not assigned.");
        }
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider = GetComponentInChildren<Slider>();
        if (slider != null) 
        {
            slider.value = currentValue / maxValue;
        }
        else
        {
            Debug.LogError("Slider component is not assigned.");
        }
    }
    public void SetColor(Color color)
    {
        slider = GetComponentInChildren<Slider>();
        if (slider != null)
        {
            slider.fillRect.GetComponent<Image>().color = color;
        }
        else
        {
            Debug.LogError("Slider component is not assigned.");
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
