using TMPro;
using Ui.States;
using UnityEngine;
using UnityEngine.UI;


public class StageItem : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private Button interactButton;

    private int _levelIndex;

    private UiMonoStageState _currentUiMonoStage;

    public void Initialize(int number,UiMonoStageState uiMonoStageState)
    {
        _levelIndex = number;
        _currentUiMonoStage = uiMonoStageState;
        levelNumberText.text = (_levelIndex + 1).ToString();
        interactButton.onClick.AddListener(SelectLevel);
    }

    private void SelectLevel()
    {
        interactButton.onClick.RemoveListener(SelectLevel);
        _currentUiMonoStage.SelectLevel(_levelIndex);
    }
    
}
