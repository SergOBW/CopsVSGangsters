using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMoneyPopup : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    public void Show()
    {
        gameObject.SetActive(true);
        closeButton.onClick.AddListener(Close);
    }
    
    public void Close()
    {
        closeButton.onClick.RemoveListener(Close);
        gameObject.SetActive(false);
    }
}
