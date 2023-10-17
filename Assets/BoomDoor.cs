using System.Collections.Generic;
using DestroyIt;
using UnityEngine;

public class BoomDoor : Interactable
{
    [SerializeField] private Transform[] bombSlot;
    [SerializeField] private GameObject bombPrefab;
    private List<Bomb> _bombs = new List<Bomb>();

    private bool _isUsed;

    [SerializeField] private bool canBeBoom;

    public override void Initialize()
    {
        _bombs = new List<Bomb>();
        outline = GetComponent<Outline>();
        if (canBeBoom)
        {
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineWidth = 5;
            outline.OutlineColor = Color.yellow;
        }
        else outline.enabled = false;
    }


    public override void Interact()
    {
        base.Interact();
        if (!_isUsed && canBeBoom)
        {
            _isUsed = true;
            for (int i = 0; i < bombSlot.Length; i++)
            {
                GameObject gameObject = Instantiate(bombPrefab, bombSlot[i]);
                if (gameObject.TryGetComponent(out Bomb bomb))
                {
                    _bombs.Add(bomb);
                }
            }

            if (_bombs.Count > 0)
            {
                _bombs[0].Setup(this);
            }
            else
            {
                Debug.LogError("Something went wrong!");
            }
            outline.enabled = false;
        }
    }
    
    public override bool CanInteract()
    {
        if (canBeBoom)
        {
            return !_isUsed;
        }
        return false;
    }

    public void Boom()
    {
        GetComponent<Destructible>().Destroy();
    }

    public void HandleAllBombs(float timer)
    {
        foreach (var bomb in _bombs)
        {
            bomb.HandleTime(timer);
        }
    }
}
