using System;
using UnityEngine;
using UnityEngine.UI;

public class UiSwitcher : MonoBehaviour
{
    private bool isEnable;

    [SerializeField] private Image backGroundImage;
    
    [SerializeField] private Button enableButton;
    [SerializeField] private Button disableButton;

    [SerializeField] private Sprite backGroundEnableSprite;
    [SerializeField] private Sprite backGroundDisableSprite;

    public event Action<bool> OnSwitcherStatusChanged;

    public void Initialize(bool startingState)
    {
        Setup(startingState);
        enableButton.onClick.AddListener(Enable);
        disableButton.onClick.AddListener(Disable);
    }
    public void Setup(bool _currentStatus)
    {
        isEnable = _currentStatus;
        if (isEnable)
        {
            backGroundImage.sprite = backGroundEnableSprite;
            enableButton.gameObject.SetActive(false);
            disableButton.gameObject.SetActive(true);
        }
        else
        {
            backGroundImage.sprite = backGroundDisableSprite;
            enableButton.gameObject.SetActive(true);
            disableButton.gameObject.SetActive(false);
        }
        OnSwitcherStatusChanged?.Invoke(isEnable);
    }
    
    public void Deinitialize()
    {
        enableButton.onClick.RemoveListener(Enable);
        disableButton.onClick.RemoveListener(Disable);
    }

    private void Enable()
    {
        isEnable = true;
        Setup(isEnable);
    }
    private void Disable()
    {
        isEnable = false;
        Setup(isEnable);
    }
}
