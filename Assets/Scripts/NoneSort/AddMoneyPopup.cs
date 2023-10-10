using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddMoneyPopup : MonoBehaviour
{
    private int moneyToAdd;
    [SerializeField] private Button watchAddsButton;
    [SerializeField] private TMP_Text moneyCountText;
    [SerializeField] private GameObject comeLaterText;
    [SerializeField] private GameObject watchAddText;
    private int lastLevelWathcedAdd;
    private void OnEnable()
    {
        moneyToAdd = LevelsMonoMechanic.Instance.GetLastSavedLevel() * 200 + 1000;
        moneyCountText.text = "+ " + moneyToAdd;
        if (LevelsMonoMechanic.Instance.GetLastSavedLevel() % 2 == 0 && lastLevelWathcedAdd != LevelsMonoMechanic.Instance.GetLastSavedLevel())
        {
            comeLaterText.gameObject.SetActive(false);
            watchAddText.gameObject.SetActive(true);
            watchAddsButton.onClick.AddListener(WatchAdd);
        }
        else
        {
            comeLaterText.gameObject.SetActive(true);
            watchAddText.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        watchAddsButton.onClick.RemoveListener(WatchAdd);
        AddManager.Instance.OnRewardedAddClosed -= AddClosed;
    }

    private void WatchAdd()
    {
        lastLevelWathcedAdd = LevelsMonoMechanic.Instance.GetLastSavedLevel();
        watchAddText.gameObject.SetActive(false);
        comeLaterText.gameObject.SetActive(true);
        watchAddsButton.onClick.RemoveListener(WatchAdd);
        AddManager.Instance.OnRewardedAddClosed += AddClosed;
        AddManager.Instance.ShowRewardAdd();
    }

    private void AddClosed()
    {
        EconomyMonoMechanic.Instance.AddMoney(moneyToAdd);
        AddManager.Instance.OnRewardedAddClosed -= AddClosed;
    }
}
