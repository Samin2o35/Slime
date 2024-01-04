using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image _image;
    
    // Start is called before the first frame update
    private void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _image.fillAmount = currentHealth / maxHealth;
    }
}
