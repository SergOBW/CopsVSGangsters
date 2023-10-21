using System;
using Abstract;
using TMPro;
using UnityEngine;

public enum StateMachineType
{
    Ui,
    Level
}

public class StateMachinesDebugger : MonoBehaviour
{
    [SerializeField] private TMP_Text currentStateText;
    [SerializeField] private TMP_Text prevoiusStateText;
    [SerializeField] private TMP_Text title;

    [SerializeField] private StateMachineType stateMachineType;
    private IStateMachine _monoStateMachine;
    

    private void OnDisable()
    {
        _monoStateMachine.OnStateChangedEvent += UpdateUi;
    }

    private void UpdateUi(IState previousMonoState, IState currentMonoState)
    {
        if (previousMonoState != null)
        {
            prevoiusStateText.text = previousMonoState.GetType().Name;
        }

        if (currentMonoState != null)
        {
            currentStateText.text = currentMonoState.GetType().Name;
        }
    }

    public void Initialize()
    {
        switch (stateMachineType)
        {
            case StateMachineType.Ui:
                _monoStateMachine = UiMonoStateMachine.Instance;
                break;
            case StateMachineType.Level:
                _monoStateMachine = LevelStateMachine.Instance;
                break;
        }
        if (_monoStateMachine != null)
        {
            _monoStateMachine.OnStateChangedEvent += UpdateUi;
            UpdateUi(_monoStateMachine.PreviousState, _monoStateMachine.CurrentState);
            title.text = _monoStateMachine.GetType().ToString();
        }
    }
}
