using Abstract;
using UnityEngine;

namespace Ui.States
{
    public class UiMonoState : MonoBehaviour, IState
    {
        protected UiMonoStateMachine currentMonoStateMachine;
        public virtual void EnterState(IStateMachine monoStateMachine)
        {
            currentMonoStateMachine = monoStateMachine as UiMonoStateMachine;
            gameObject.SetActive(true);
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