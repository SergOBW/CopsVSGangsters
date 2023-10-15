using DestroyIt;
using TMPro;
using UnityEngine;

public class BoomDoor : Interactable
{
    [SerializeField] private Transform bombSlot;
    [SerializeField] private GameObject bombPrefab;

    private bool _isUsed;
    

    public override void Interact()
    {
        base.Interact();
        if (!_isUsed)
        {
            _isUsed = true;
            GameObject gameObject = Instantiate(bombPrefab, bombSlot);
            gameObject.GetComponent<Bomb>().Setup(this);
            outline.enabled = false;
        }
    }
    
    public override bool CanInteract()
    {
        return !_isUsed;
    }

    public void Boom()
    {
        GetComponent<Destructible>().Destroy();
    }
}
