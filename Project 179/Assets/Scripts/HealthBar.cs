using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void SetMaxHealth (float health) 
    {
        slider.maxValue = health;
        slider.value = health;

    }

    public void SetHealth(float health)
    {
        StartCoroutine(DecreaseHealthOverTime(health));
    }
    // Gradually decrease health for smoother look
    private IEnumerator DecreaseHealthOverTime(float currentHealth)
    {
        float startHealth = slider.value;
        float timer = 0f;
        float duration = 0.5f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newHealth = Mathf.Lerp(startHealth, currentHealth, timer / duration);
            slider.value = newHealth;
            yield return null;
        }

        slider.value = currentHealth;
    }
}
