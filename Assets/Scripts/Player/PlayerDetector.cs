using Interaction;
using Quests.Item;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float detectionDistance = 100f;
    public bool CanInteraction { get;private set; }
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera == null)
        {
            return;
        }

        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        if (Physics.Raycast(ray,out RaycastHit hit,detectionDistance,targetLayerMask))
        {
            CanInteraction = true;
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactable.Interact();
                }
            }
            if (hit.transform.TryGetComponent(out LootItem lootItem))
            {
                lootItem.HighLight();
            }
        }
        else
        {
            CanInteraction = false;
        }
    }
}
