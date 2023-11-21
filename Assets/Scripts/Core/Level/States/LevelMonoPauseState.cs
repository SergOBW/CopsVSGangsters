using Abstract;

namespace Level.States
{
    public class LevelMonoPauseState : LevelMonoState
    {
        public override void EnterState(IStateMachine monoStateMachine)
        {
            base.EnterState(monoStateMachine);
            SoundMonoMechanic.Instance.DisableSound();
        }
    }
}