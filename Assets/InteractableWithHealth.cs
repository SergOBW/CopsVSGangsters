using DefaultNamespace;
using Interaction;
using UnityEngine;

public class InteractableWithHealth : Interactable
{
    [SerializeField] protected float startedHealth = 1f;
    private float _health;
    protected bool isDead;
    protected override void Initialize()
    {
        base.Initialize();
        _health = 0;
    }

    public override void Interact()
    {
        if (!isDead)
        {
            if (_health < startedHealth)
            {
                _health += Time.deltaTime;
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
        outline.enabled = false;
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
