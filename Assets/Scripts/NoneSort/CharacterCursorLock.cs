using UnityEngine;

public class CharacterCursorLock : MonoBehaviour
{
    private void Start()
    {
        LevelMonoStateMachine.Instance.ChangeState(LevelMonoStateMachine.Instance.levelMonoPlayState);
    }
}
