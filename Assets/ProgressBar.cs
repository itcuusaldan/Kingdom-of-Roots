using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fill;


    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        fill.fillAmount = 1f;
    }
    
    public void UpdateFill(float value)
    {
        fill.fillAmount = Mathf.Clamp(value, 0f, 1f);
    }
}
