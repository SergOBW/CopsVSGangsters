using Abstract;
using UnityEngine;

namespace Ui.States
{
    public class UiMonoState : MonoBehaviour, IState
    {
        [SerializeField] private bool needCursor;
        protected UiMonoStateMachine currentMonoStateMachine;
        public virtual void EnterState(IStateMachine monoStateMachine)
        {
            currentMonoStateMachine = monoStateMachine as UiMonoStateMachine;
            gameObject.SetActive(true);
            if (needCursor)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } 
        }

        public virtual void UpdateState()
        {
            
        }

        public virtual void ExitState(IState monoState)
        {
            gameObject.SetActive(false);
            currentMonoStateMachine.ChangeState(monoState);
        }
    }
}