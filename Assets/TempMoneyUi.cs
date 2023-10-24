using Quests.LootMoney;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempMoneyUi : MonoBehaviour
{
    [SerializeField] private Slider moneySlider;
    [SerializeField] private TMP_Text moneyText;

    private int maxMoney;
    private void OnEnable()
    {
        EconomyMonoMechanic.Instance.OnTempMoneyAmountChanged += OnTempMoneyAmountChanged;
        OnTempMoneyAmountChanged(EconomyMonoMechanic.Instance.GetCurrentTempMoney());
        maxMoney = 0;
        LootMoneyItem[] lootMoneyItems = FindObjectsOfType<LootMoneyItem>();
        foreach (var lootMoneyItem in lootMoneyItems)
        {
            maxMoney += lootMoneyItem.GetMoneyAmount();
        }
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

        moneyText.text = obj.ToString();
    }
}
