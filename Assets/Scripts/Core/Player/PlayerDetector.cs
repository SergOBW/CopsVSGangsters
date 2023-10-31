using System;
using Interaction;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerDetector : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private LayerMask occlusionLayers;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float detectionDistance = 100f;

    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] private StarterAssetsInputs _starterAssetsInputs;
    public IInteractable CurrentInteractable
    {
        get => _interactable;
    }
    private IInteractable _interactable;
    private FirstPersonController _firstPersonController;
    private void Start()
    {
        _firstPersonController = GetComponent<FirstPersonController>();
        _camera = _firstPersonController.GetCamera();
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
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
                if (!AddManager.Instance.isMobile)
                {
                    if (_starterAssetsInputs.loot)
                    {
                        if (interactable.CanInteract())
                        {
                            interactable.Interact();
                        }
                    }
                }
                else
                {
                    if (interactable.CanInteract())
                    {
                        interactable.Interact();
                    }
                }
                

                _interactable = interactable;
            }
        }
        else
        {
            _interactable = null;
        }
    }

    private void LateUpdate()
    {
        
        if (AddManager.Instance.isMobile)
        {
            Ray ray2 = new Ray(_camera.transform.position, _camera.transform.forward);
            if (Physics.Raycast(ray2,out RaycastHit hit2,float.MaxValue,enemyLayerMask))
            {
                _starterAssetsInputs.AutoShooting(true);
            }
            else
            {
                _starterAssetsInputs.AutoShooting(false);
            }
        }
    }
}
