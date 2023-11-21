using Abstract.Inventory;
using DG.Tweening;
using EnemyCore;
using UnityEngine;

public class DrilledDoor : InteractableWithHealth
{
    [SerializeField] private GameObject drillPrefab;
    [SerializeField] private Transform drillTransform;
    [SerializeField] private Vector3 vector3 = new Vector3(0, -140, 0);

    [SerializeField] private bool needSpawnWave;

    [SerializeField] private bool needBigDrill;

    protected override void Handle()
    {
        base.Handle();
        GameObject gameObject = Instantiate(drillPrefab, drillTransform);
        gameObject.GetComponent<Drill>().StartDrill(this);
        if (needSpawnWave)
        {
            EnemyHandleMechanic.Instance.SpawnEnemyWave();
        }
    }

    public override bool CanInteract()
    {
        bool isAllOk = base.CanInteract();
        if (needBigDrill && isAllOk)
        {
            isAllOk = Inventory.Instance.HasItem("Big drill");
            if (!isAllOk)
            {
                UiMonoStateMachine.Instance.ShowNoBigDrillPopup();
            }
            else
            {
                UiMonoStateMachine.Instance.HideNoBigDrillPopup();
            }
        }
        return isAllOk;
    }

    public void DrillCompleted()
    {
        transform.DORotate(vector3,1f,RotateMode.LocalAxisAdd);
    }
}

