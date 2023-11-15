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

    [SerializeField] private RawImage yanIcon;
    [SerializeField] private TMP_Text moneyAmount;
    
    
    public void Show()
    {
        gameObject.SetActive(true);
        closeButton.onClick.AddListener(Close);
        buyYanGo.SetActive(!Inventory.Instance.HasItem("Money Pack"));
        watchRewardGo.SetActive(true);
        buyYan.onClick.AddListener(TryToBuy);
        watchReward.onClick.AddListener(WatchReward);
        moneyAmount.text = $"+ {EconomyMonoMechanic.Instance.GetMoneyBonus()}";
        if (AddManager.Instance.AddAggregator == AddAggregator.YandexGames)
        {
            StartCoroutine(DownLoadImage(AddManager.Instance.currencyImage));
        }
    }
    
    IEnumerator DownLoadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
            yanIcon.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
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
    }

    public void Close()
    {
        buyYan.onClick.RemoveListener(TryToBuy);
        watchReward.onClick.RemoveListener(WatchReward);
        closeButton.onClick.RemoveListener(Close);
        gameObject.SetActive(false);
    }
}
