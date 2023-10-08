using TMPro;
using UnityEngine;

public class MoneyAmountUi : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    private void OnEnable()
    {
        EconomyMechanic.Instance.OnMoneyAmountChanged += Refresh;
        Refresh(EconomyMechanic.Instance.GetCurrentMoney());
    }

    private void OnDisable()
    {
        EconomyMechanic.Instance.OnMoneyAmountChanged -= Refresh;
    }

    private void Refresh(float value)
    {
        moneyText.text = ((int)value).ToString();
    }
}
