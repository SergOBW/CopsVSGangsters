namespace Interaction
{
    public interface IInteractable
    {
        public void Interact();

        public float GetHealthNormalized();
        bool CanInteract();
    }
}