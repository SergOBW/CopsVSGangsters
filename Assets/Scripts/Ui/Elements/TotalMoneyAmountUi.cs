using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TotalMoneyAmountUi : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private AddMoneyPopup _addMoneyPopup;
    [SerializeField] private Button addMoneyButton;
    private void OnEnable()
    {
        EconomyMonoMechanic.Instance.OnMoneyAmountChanged += Refresh;
        Refresh(EconomyMonoMechanic.Instance.GetCurrentMoney());
        addMoneyButton.onClick.AddListener(OpenAddMoneyPopup);
    }

    private void OnDisable()
    {
        _addMoneyPopup.Close();
        EconomyMonoMechanic.Instance.OnMoneyAmountChanged -= Refresh;
        addMoneyButton.onClick.RemoveListener(OpenAddMoneyPopup);
    }

    private void Refresh(float value)
    {
        moneyText.text = ((int)value).ToString();
    }

    private void OpenAddMoneyPopup()
    {
        _addMoneyPopup.Show();
    }
}
