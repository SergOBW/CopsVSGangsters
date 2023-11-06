using Save;
using TMPro;
using Ui.States;
using UnityEngine;
using UnityEngine.UI;


public class StageItem : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNumberText;
    [SerializeField] private Button interactButton;
    [SerializeField] private Image levelImage;

    [SerializeField] private GameObject[] lockedGameObjects;
    [SerializeField] private GameObject[] unLockedGameObjects;

    [SerializeField] private Slider _moneySlider;
    [SerializeField] private TMP_Text _moneySliderText;

    private int _levelIndex;

    private UiMonoStageState _currentUiMonoStage;

    public void Initialize(int number,UiMonoStageState uiMonoStageState)
    {
        SaveLevel saveLevel = LevelsMonoMechanic.Instance.GetLevelSave(number);
        bool locked = saveLevel.isOpen == 0;
        if (locked)
        {
            foreach (var gameObject in unLockedGameObjects)
            {
                gameObject.SetActive(false);
            }

            foreach (var gameObject in lockedGameObjects)
            {
                gameObject.SetActive(true);
            }
            return;
        }
        foreach (var gameObject in lockedGameObjects)
        {
            gameObject.SetActive(false);
        }
        foreach (var gameObject in unLockedGameObjects)
        {
            gameObject.SetActive(true);
        }

        Sprite levelSprite = LevelsMonoMechanic.Instance.GetMapImage(number);
        
        _levelIndex = number;
        _currentUiMonoStage = uiMonoStageState;
        
        levelNumberText.text = (_levelIndex + 1).ToString();
        if (levelSprite != null)
        {
            levelImage.sprite = levelSprite;
        }
        _moneySlider.minValue = 0;
        _moneySlider.maxValue = EconomyMonoMechanic.Instance.GetMaxMoney(number);
        _moneySlider.value = saveLevel.lootedMoney;
        _moneySliderText.text = $"{saveLevel.lootedMoney} / {EconomyMonoMechanic.Instance.GetMaxMoney(number)}";
        
        interactButton.onClick.AddListener(SelectLevel);
    }

    private void SelectLevel()
    {
        interactButton.onClick.RemoveListener(SelectLevel);
        _currentUiMonoStage.SelectLevel(_levelIndex);
    }
    
}
