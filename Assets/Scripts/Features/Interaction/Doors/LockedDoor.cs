using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class LockedDoor : InteractableWithHealth
{
    [SerializeField] private Vector3 rotateVector = new (0, -140, 0);
    [SerializeField] private Vector3 moveVector;

    [SerializeField] private Transform handleTransform;

    private void Awake()
    {
        if (handleTransform == null)
        {
            handleTransform = transform;
        }
    }

    protected override void Handle()
    {
        base.Handle();
        Debug.Log("Lock completed");
        handleTransform.DORotate(rotateVector,1f,RotateMode.LocalAxisAdd);
        if (moveVector != Vector3.zero)
        {
            handleTransform.DOLocalMove(moveVector,1);
        }

        if (TryGetComponent(out NavMeshObstacle navMeshObstacle))
        {
            navMeshObstacle.enabled = false;
        }
    }
}
