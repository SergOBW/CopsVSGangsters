using Abstract;
using UnityEngine;

namespace Ui.States
{
    public class UiMonoState : MonoBehaviour, IMonoState
    {
        protected UiMonoStateMachine currentMonoStateMachine;
        public virtual void EnterState(MonoStateMachine monoStateMachine)
        {
            currentMonoStateMachine = monoStateMachine as UiMonoStateMachine;
            gameObject.SetActive(true);
        }

        public virtual void UpdateState()
        {
            
        }

        public virtual void ExitState(IMonoState monoState)
        {
            gameObject.SetActive(false);
            currentMonoStateMachine.ChangeState(monoState);
        }
    }
}