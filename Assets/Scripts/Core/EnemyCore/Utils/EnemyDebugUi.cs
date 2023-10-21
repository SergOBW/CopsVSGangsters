using EnemyCore;
using EnemyCore.States;
using TMPro;
using UnityEngine;

public class EnemyDebugUi : MonoBehaviour
{
    [SerializeField] private TMP_Text previousStateText;
    [SerializeField] private TMP_Text currentStatText;
    [SerializeField] private TMP_Text disnatceText;
    [SerializeField] private TMP_Text healthText;

    public void Initialize(EnemyMonoStateMachine machine)
    {
        machine.GetComponent<EnemyStatsController>().OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float obj)
    {
        healthText.text = obj.ToString();
    }

    private void OnStateChanged(EnemyState previousEnemyState, EnemyState currentEnemyState)
    {
        previousStateText.text = previousEnemyState.ToString();
        currentStatText.text = currentEnemyState.ToString();
    }

    private void OnDistanceChanged(float distance)
    {
        disnatceText.text = distance.ToString();
    }
}
