using UnityEngine;
using UnityEngine.UI;

public class PlayrHealtBar : MonoBehaviour
{
    public Image healthBarFill; // Reference to the health bar's fill image

    // Method to set the maximum health
    public void SetMaxHealth(int maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f; // Full health
        }
    }

    // Method to update the health bar based on current health
    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
