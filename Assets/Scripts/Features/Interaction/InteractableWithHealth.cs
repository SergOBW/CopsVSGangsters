using Abstract.Inventory;
using DefaultNamespace;
using Interaction;
using UnityEngine;

public class InteractableWithHealth : Interactable
{
    [SerializeField] protected float startedHealth = 1f;
    private float _health;
    protected bool isDead;
    private bool _hasBoostItem;
    protected override void Initialize()
    {
        base.Initialize();
        _health = 0;
        _hasBoostItem = Inventory.Instance.HasItem("Tactical Gloves");
    }

    public override void Interact()
    {
        if (!isDead)
        {
            if (_health < startedHealth)
            {
                if (_hasBoostItem)
                {
                    _health += Time.deltaTime * 2;
                }
                else
                {
                    _health += Time.deltaTime;
                }
            }
            else
            {
                isDead = true;
                Handle();
            }
        }
    }

    protected virtual void Handle()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public override float GetHealthNormalized()
    {
        if (startedHealth == 0)
        {
            return 0;
        }
        return SergOBWUtils.GetNormalizeNumber(_health,0,startedHealth);
    }

    public override bool CanInteract()
    {
        return !isDead;
    }
}
