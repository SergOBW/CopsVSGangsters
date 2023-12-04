using System;
using System.Collections;
using Abstract.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddMoneyPopup : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button watchReward;
    [SerializeField] private Button buyYan;

    [SerializeField] private GameObject watchRewardGo;
    [SerializeField] private GameObject buyYanGo;
    
    [SerializeField] private TMP_Text moneyAmount;
    
    
    public void Show()
    {
        gameObject.SetActive(true);
        closeButton.onClick.AddListener(Close);
        watchRewardGo.SetActive(AddManager.Instance.CanShowRewardAdd());
        buyYan.onClick.AddListener(TryToBuy);
        watchReward.onClick.AddListener(WatchReward);
        moneyAmount.text = $"+ {EconomyMonoMechanic.Instance.GetMoneyBonus()}";
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (AddManager.Instance.AddAggregator != AddAggregator.YandexGames)
        {
            buyYanGo.SetActive(false);
            return;
        }
        buyYanGo.SetActive(!Inventory.Instance.HasItem("Money Pack"));
    }
    private void WatchReward()
    {
        AddManager.Instance.OnRewarded += OnRewarded;
        AddManager.Instance.ShowRewardAdd();
    }

    private void TryToBuy()
    {
        AddManager.Instance.TryToBuyItem("Money Pack");
    }

    private void OnRewarded()
    {
        AddManager.Instance.OnRewarded -= OnRewarded;
        EconomyMonoMechanic.Instance.DoRewardedAddBonus();
        watchRewardGo.SetActive(AddManager.Instance.CanShowRewardAdd());
    }

    public void Close()
    {
        buyYan.onClick.RemoveListener(TryToBuy);
        watchReward.onClick.RemoveListener(WatchReward);
        closeButton.onClick.RemoveListener(Close);
        gameObject.SetActive(false);
    }
}
