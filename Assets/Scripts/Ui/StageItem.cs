using TMPro;
using Ui.States;
using UnityEngine;
using UnityEngine.UI;


public class StageItem : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private Button interactButton;
    [SerializeField] private Image levelImage;

    private int _levelIndex;

    private UiMonoStageState _currentUiMonoStage;

    public void Initialize(int number,UiMonoStageState uiMonoStageState, Sprite levelSprite = null)
    {
        _levelIndex = number;
        _currentUiMonoStage = uiMonoStageState;
        levelNumberText.text = (_levelIndex + 1).ToString();
        interactButton.onClick.AddListener(SelectLevel);
        if (levelSprite != null)
        {
            levelImage.sprite = levelSprite;
        }
    }

    private void SelectLevel()
    {
        interactButton.onClick.RemoveListener(SelectLevel);
        _currentUiMonoStage.SelectLevel(_levelIndex);
    }
    
}
