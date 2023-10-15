using System;
using DG.Tweening;
using Interaction;
using UnityEngine;

public class DrilledDoor : Interactable
{
    [SerializeField] private GameObject drillPrefab;
    [SerializeField] private Transform drillTransform;
    [SerializeField] private Vector3 vector3 = new Vector3(0, -140, 0);
    private bool _isUsed;
    public override void Interact()
    {
        if (!_isUsed)
        {
            GameObject gameObject = Instantiate(drillPrefab, drillTransform);
            gameObject.GetComponent<Drill>().StartDrill(this);
            _isUsed = true;
            outline.enabled = false;
        }
    }

    public override bool CanInteract()
    {
        return !_isUsed;
    }

    public void DrillCompleted()
    {
        transform.DORotate(vector3,1f,RotateMode.LocalAxisAdd);
    }
}

[RequireComponent(typeof(Outline))]
public class Interactable  : MonoBehaviour , IInteractable
{
    protected Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public virtual void Interact()
    {
        
    }

    public virtual bool CanInteract()
    {
        return true;
    }
}
