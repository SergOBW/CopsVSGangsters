using UnityEngine;

public class EnemyFbiVisualsController : EnemyVisualsController
{
    [SerializeField] private GameObject[] _hairCut;
    protected override void ChooseRandomVisual()
    {
        base.ChooseRandomVisual();
        foreach (var gameObject in _hairCut)
        {
            gameObject.SetActive(false);
        }
        
        _hairCut[index].SetActive(true);
    }
}
