using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float timeToDrain;
    [SerializeField] private Gradient healthBarGradient;
    private float target = 1f;
    private Image image;
    private Color newHealthBarColor;
    private Coroutine drainHealthBarCoroutine;
    
    private void Start()
    {
        image = GetComponent<Image>();
        image.color = healthBarGradient.Evaluate(target);
        CheckHealthBarGradient();
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
        drainHealthBarCoroutine = StartCoroutine(DrainHealthBar());
        CheckHealthBarGradient();
    }

    private IEnumerator DrainHealthBar()
    {
        float fillAmount = image.fillAmount;
        Color currentColor = image.color;
        float elapsedTime = 0f;
        
        while(elapsedTime < timeToDrain) 
        {
            elapsedTime += Time.deltaTime;
            //lerp the fill amount
            image.fillAmount = Mathf.Lerp(fillAmount, target, elapsedTime / timeToDrain);

            //lerp the color based on gradient
            image.color = Color.Lerp(currentColor, newHealthBarColor, (elapsedTime / timeToDrain));
            yield return null;
        }
    }

    private void CheckHealthBarGradient()
    {
        newHealthBarColor = healthBarGradient.Evaluate(target);
    }
}
