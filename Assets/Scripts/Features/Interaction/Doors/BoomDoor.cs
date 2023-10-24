using System.Collections.Generic;
using DestroyIt;
using EnemyCore;
using UnityEngine;

public class BoomDoor : InteractableWithHealth
{
    [SerializeField] private Transform[] bombSlot;
    [SerializeField] private GameObject bombPrefab;
    private List<Bomb> _bombs = new List<Bomb>();

    [SerializeField] private bool canBeBoom;
    [SerializeField] private bool needToSpawnWave;

    protected override void Initialize()
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
        else
        {
            outline.enabled = false;
            isDead = true;
        }
    }

    protected override void Handle()
    {
        base.Handle();
        if (needToSpawnWave)
        {
            EnemyHandleMechanic.Instance.SpawnEnemyWave();
        }
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
