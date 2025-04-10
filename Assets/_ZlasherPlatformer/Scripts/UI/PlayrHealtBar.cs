using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayrHealtBar : MonoBehaviour
{
    public Image healthBarFill; // Reference to the health bar's fill image
    public float baseShakePower = 1f; // Base power of the shake effect
    public float shakeDuration = 0.5f; // Duration of each shake
    private bool isShaking = false; // To prevent overlapping shake effects

    public Color lowHealthColor = Color.red; // Dark red when health is low
    public Color midHealthColor = Color.yellow; // Yellow when health is mid
    public Color fullHealthColor = Color.white; // White when health is full

    // Method to set the maximum health
    public void SetMaxHealth(int maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f; // Full health
            healthBarFill.color = fullHealthColor; // Set to full health color
        }
    }

    // Method to update the health bar based on current health
    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthBarFill != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBarFill.fillAmount = healthPercentage;

            // Interpolate the color based on health percentage
            if (healthPercentage > 0.5f)
            {
                // Transition from white to yellow
                healthBarFill.color = Color.Lerp(midHealthColor, fullHealthColor, (healthPercentage - 0.5f) * 2f);
            }
            else
            {
                // Transition from yellow to dark red
                healthBarFill.color = Color.Lerp(lowHealthColor, midHealthColor, healthPercentage * 2f);
            }

            // Start shaking if health is below 50%
            if (healthPercentage < 0.5f && !isShaking)
            {
                StartShaking(healthPercentage);
            }
        }
    }

    private void StartShaking(float healthPercentage)
    {
        isShaking = true;

        // Scale the shake power based on how low the health is
        float scaledShakePower = baseShakePower * (1f - healthPercentage) * 5f; // Stronger shake as health decreases

        // Apply the shake effect
        healthBarFill.transform.DOShakePosition(shakeDuration, scaledShakePower, 10, 90, false, true)
            .OnComplete(() =>
            {
                isShaking = false; // Allow the next shake
            });
    }
}
