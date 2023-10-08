using UnityEngine;

namespace Abstract
{
    public class DefaultMonoState : IMonoState
    {
        public MonoStateMachine CurrentMonoStateMachine { get; set; }
        public void EnterState(MonoStateMachine monoStateMachine)
        {
            Debug.Log("Hello im default");
            CurrentMonoStateMachine = monoStateMachine;
        }

        public void UpdateState()
        {
            
        }

        public void ExitState(IMonoState monoState)
        {
            Debug.Log("Bye im default");
        }
    }
}