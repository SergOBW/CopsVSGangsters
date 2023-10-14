using Interaction;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float detectionDistance = 100f;
    public IInteractable CurrentInteractable
    {
        get => _interactable;
    }
    private IInteractable _interactable;
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
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (Input.GetKey(KeyCode.F))
                {
                    interactable.Interact();
                }

                _interactable = interactable;
            }
        }
        else
        {
            _interactable = null;
        }
    }
}
