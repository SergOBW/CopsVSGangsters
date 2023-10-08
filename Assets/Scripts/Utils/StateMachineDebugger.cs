using Abstract;
using EnemyCore;
using TMPro;
using UnityEngine;

public class StateMachineDebugger : MonoBehaviour
{
    [SerializeField] private TMP_Text previousState;
    [SerializeField] private TMP_Text currentState;

    private EnemyStatsController enemyStatsController;

    private float startedHealth;

    public void Initialize(MonoStateMachine monoStateMachine)
    {
        monoStateMachine.OnStateChangedEvent += OnStateChangedEvent;
        if (monoStateMachine.PreviousMonoState != null)
        {
            OnStateChangedEvent(monoStateMachine.PreviousMonoState,monoStateMachine.CurrentMonoState);
        }
        else
        {
            OnStateChangedEvent(monoStateMachine.CurrentMonoState,monoStateMachine.CurrentMonoState);
        }
    }
    
    private void OnStateChangedEvent(IMonoState arg1, IMonoState arg2)
    {
        if (previousState != null)
        {
            previousState.text = arg1.GetType().Name;
        }

        if (currentState != null)
        {
            currentState.text = arg2.GetType().Name;
        }
    }
    
    private void Update()
    {
        // Поворачиваем канвас так, чтобы он всегда смотрел на главную камеру
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
