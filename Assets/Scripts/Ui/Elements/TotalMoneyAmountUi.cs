using TMPro;
using UnityEngine;

public class TotalMoneyAmountUi : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    private void OnEnable()
    {
        EconomyMonoMechanic.Instance.OnMoneyAmountChanged += Refresh;
        Refresh(EconomyMonoMechanic.Instance.GetCurrentMoney());
    }

    private void OnDisable()
    {
        EconomyMonoMechanic.Instance.OnMoneyAmountChanged -= Refresh;
    }

    private void Refresh(float value)
    {
        moneyText.text = ((int)value).ToString();
    }
}
