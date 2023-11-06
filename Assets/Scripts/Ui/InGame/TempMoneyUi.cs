using Quests.LootMoney;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempMoneyUi : MonoBehaviour
{
    [SerializeField] private Slider moneySlider;
    [SerializeField] private TMP_Text moneyText;

    private float maxMoney;
    private void OnEnable()
    {
        EconomyMonoMechanic.Instance.OnTempMoneyAmountChanged += OnTempMoneyAmountChanged;
        maxMoney = EconomyMonoMechanic.Instance.GetCurrentMoneyToWin();
        OnTempMoneyAmountChanged(EconomyMonoMechanic.Instance.GetCurrentTempMoney());
    }

    private void OnDisable()
    {
        EconomyMonoMechanic.Instance.OnTempMoneyAmountChanged -= OnTempMoneyAmountChanged;
    }

    private void OnTempMoneyAmountChanged(float obj)
    {
        moneySlider.maxValue = maxMoney;
        moneySlider.minValue = 0;
        moneySlider.value = obj;

        moneyText.text = ((int)obj).ToString();
    }
}
