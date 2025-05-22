using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image durationImage;
    public GameObject useButton;

    public static UIManager Instance;

    void Awake()
    { 
        Instance = this;
    }

    public void ShowUseButton(bool show)
    {
        useButton.SetActive(show);
    }

    public void SetDurationFill(float t)
    {
        durationImage.fillAmount = Mathf.Clamp01(t);
    }
}
