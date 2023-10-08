using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonWithCursor : Button
{
    public event Action OnPointerDownEvent;
    public override void OnPointerDown(PointerEventData eventData)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OnPointerDownEvent?.Invoke();
    }
}