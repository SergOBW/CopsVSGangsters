using EnemyCore;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WaveUi : MonoBehaviour
{
    [SerializeField] private TMP_Text currentWave;
    [FormerlySerializedAs("currentEnemyCount")] [SerializeField] private TMP_Text currentEnemyCountText;
    [SerializeField] private Image fillImage;

    private void OnEnable()
    {
        InvokeRepeating("UpdateUi",0,0.2f);
    }
    
    public void UpdateUi()
    {
        int maxEnemyCount = EnemyHandleMechanic.Instance.StartedEnemyCount;
        int currentEnemyCount = EnemyHandleMechanic.Instance.GetEnemyCount();
        currentEnemyCountText.text = currentEnemyCount.ToString();
        fillImage.fillAmount = (float)currentEnemyCount / maxEnemyCount;
    }
}
