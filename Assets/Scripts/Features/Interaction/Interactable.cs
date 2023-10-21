using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(Outline))]
    public abstract class Interactable  : MonoBehaviour , IInteractable
    {
        protected Outline outline;

        private void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            outline = GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
        }

        public abstract void Interact();

        public abstract float GetHealthNormalized();

        public abstract bool CanInteract();
    }
}