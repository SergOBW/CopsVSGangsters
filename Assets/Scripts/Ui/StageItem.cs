using TMPro;
using Ui.States;
using UnityEngine;
using UnityEngine.UI;


public class StageItem : MonoBehaviour
{
    // Main image
    [SerializeField] private Sprite unlockSprite;
    [SerializeField] private Sprite lockSprite;
    
    // Starts
    [SerializeField] private Sprite unlockStarSprite;
    [SerializeField] private Sprite lockStarSprite;

    [SerializeField] private Image mainImage;

    [SerializeField] private Transform startsGrid;

    [SerializeField] private TMP_Text numberText;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button interactButton;

    private int levelNumber;

    private UiMonoStageState _currentUiMonoStage;

    public void Initialize(int lockStars, int unlockStars, bool isLock, int number, UiMonoStageState uiMonoStageState, bool isBoss = false)
    {
        if (isLock)
        {
            mainImage.sprite = lockSprite;
            startsGrid.gameObject.SetActive(false);
            numberText.gameObject.SetActive(false);
            titleText.gameObject. SetActive(false);
            interactButton.interactable = false;
            return;
        }
        
        titleText.gameObject.SetActive(isBoss);
        
        startsGrid.gameObject.SetActive(true);
        numberText.gameObject.SetActive(true);
        
        mainImage.sprite = unlockSprite;
        
        levelNumber = number;
        numberText.text = (levelNumber + 1).ToString();

        _currentUiMonoStage = uiMonoStageState;
        interactButton.interactable = true;
        interactButton.onClick.AddListener(SelectLevel);

        if (lockStars == 3)
        {
            return;
        }

        for (int l = 0; l < unlockStars; l++)
        {
            GameObject startsGameObject = Instantiate(new GameObject(),startsGrid);
            Image starsImage = startsGameObject.AddComponent<Image>();
            starsImage.sprite = unlockStarSprite;
        }
        
        for (int l = 0; l < lockStars; l++)
        {
            GameObject startsGameObject = Instantiate(new GameObject(),startsGrid);
            Image starsImage = startsGameObject.AddComponent<Image>();
            starsImage.sprite = lockStarSprite;
        }
        
    }

    private void SelectLevel()
    {
        interactButton.onClick.RemoveListener(SelectLevel);
        _currentUiMonoStage.SelectLevel(levelNumber);
    }

    public void SetActiveInteractionFalse()
    {
        interactButton.interactable = false;
    }
    
}
