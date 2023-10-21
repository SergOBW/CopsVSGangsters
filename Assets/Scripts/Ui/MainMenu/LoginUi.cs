using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Yandex.Plugins.Login;

public class LoginUi : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private Button openLoginPopup;
    [SerializeField] private Button closeLoginPopup;
    [SerializeField] private GameObject loginPopup;

    private void OnEnable()
    {
        LoginManager.Instance.OnAvatarChanged += OnAvatarChanged;
        LoginManager.Instance.OnNickNameChanged += OnNickNameChanged;
        loginPopup.SetActive(false);
        if (!LoginManager.Instance.isLogin)
        {
            openLoginPopup.onClick.AddListener(OpenLoginPopup);
            closeLoginPopup.onClick.AddListener(CloseLoginPopup);
            loginButton.onClick.AddListener(LoginManager.Instance.Login);
        }
        else
        {
            openLoginPopup.interactable = false;
        }
    }

    private void OnDisable()
    {
        LoginManager.Instance.OnAvatarChanged -= OnAvatarChanged;
        LoginManager.Instance.OnNickNameChanged -= OnNickNameChanged;
        loginButton.onClick.RemoveListener(LoginManager.Instance.Login);
        openLoginPopup.onClick.RemoveListener(OpenLoginPopup);
        closeLoginPopup.onClick.RemoveListener(CloseLoginPopup);
    }
    
    private void OnNickNameChanged(string obj)
    {
        mainText.GetComponent<MultiLanguageText>().enabled = false;
        mainText.text = obj;
        loginButton.interactable = false;
        openLoginPopup.interactable = false;
        loginPopup.SetActive(false);
    }

    private void OnAvatarChanged(string obj)
    {
        StartCoroutine(DownLoadImage(obj));
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
            _rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

    private void OpenLoginPopup()
    {
        loginPopup.SetActive(true);
    }
    
    private void CloseLoginPopup()
    {
        loginPopup.SetActive(false);
    }
}
