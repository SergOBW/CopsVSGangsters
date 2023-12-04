using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class UiGraphics : MonoBehaviour
{
    [SerializeField] private Button lowButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button highButton;

    private GraphicsQuality _currentGraphicsQuality;

    private void Start()
    {
        GlobalSettings.Instance.OnSettingsChanged += Refresh;
        Refresh();
        highButton.onClick.AddListener(High);
        lowButton.onClick.AddListener(Low);
        mediumButton.onClick.AddListener(Medium);
    }

    private void Refresh()
    {
        _currentGraphicsQuality = GlobalSettings.Instance.graphicsQuality;
        switch (_currentGraphicsQuality)
        {
            case GraphicsQuality.High :
                highButton.interactable = false;
                mediumButton.interactable = true;
                lowButton.interactable = true;
                break;
            case GraphicsQuality.Low :
                highButton.interactable = true;
                mediumButton.interactable = true;
                lowButton.interactable = false;
                break;
            case GraphicsQuality.Medium :
                highButton.interactable = true;
                mediumButton.interactable = false;
                lowButton.interactable = true;
                break;
        }
    }

    private void High()
    {
        GlobalSettings.Instance.ChangeGraphics(GraphicsQuality.High);
    }

    private void Low()
    {
        GlobalSettings.Instance.ChangeGraphics(GraphicsQuality.Low);
    }

    private void Medium()
    {
        GlobalSettings.Instance.ChangeGraphics(GraphicsQuality.Medium);
    }
    
    
}
