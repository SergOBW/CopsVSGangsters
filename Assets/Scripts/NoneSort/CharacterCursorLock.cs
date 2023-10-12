using UnityEngine;

public class CharacterCursorLock : MonoBehaviour
{
    private void Start()
    {
        LevelStateMachine.Instance.ChangeState(LevelStateMachine.Instance.levelMonoPlayState);
    }
}
