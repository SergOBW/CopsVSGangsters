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
    private MonoStateMachine _monoStateMachine;

    private void OnEnable()
    {
        switch (stateMachineType)
        {
            case StateMachineType.Ui:
                _monoStateMachine = UiMonoStateMachine.Instance;
                break;
            case StateMachineType.Level:
                _monoStateMachine = LevelMonoStateMachine.Instance;
                break;
        }
        if (_monoStateMachine != null)
        {
            _monoStateMachine.OnStateChangedEvent += UpdateUi;
            UpdateUi(_monoStateMachine.PreviousMonoState, _monoStateMachine.CurrentMonoState);
            title.text = _monoStateMachine.GetType().ToString();
        }
    }

    private void OnDisable()
    {
        _monoStateMachine.OnStateChangedEvent += UpdateUi;
    }

    private void UpdateUi(IMonoState previousMonoState, IMonoState currentMonoState)
    {
        if (previousMonoState != null)
        {
            prevoiusStateText.text = previousMonoState.ToString();
        }

        if (currentMonoState != null)
        {
            currentStateText.text = currentMonoState.ToString();
        }
    }
}
