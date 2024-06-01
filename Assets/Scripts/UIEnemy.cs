using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour
{
    public Image imageHealth;

    public void ShowImageHealth(int currentHealth, int maxHealth)
    {
        imageHealth.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}
