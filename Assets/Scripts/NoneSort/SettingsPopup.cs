using UnityEngine;
using UnityEngine.UI;
using Yandex.Plugins.Login;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Button onSoundButton;
    [SerializeField] private Button offSoundButton;

    [SerializeField] private Image soundImage;

    [SerializeField] private Sprite onSound;
    [SerializeField] private Sprite offSound;

    public void Show()
    {
        if (SoundMonoMechanic.Instance.IsSoundOn())
        {
            offSoundButton.gameObject.SetActive(true);
            onSoundButton.gameObject.SetActive(false);
            soundImage.sprite = onSound;
        }
        else
        {
            offSoundButton.gameObject.SetActive(false);
            onSoundButton.gameObject.SetActive(true);
            soundImage.sprite = offSound;
        }
        onSoundButton.onClick.AddListener(OnSound);
        offSoundButton.onClick.AddListener(OffSound);
    }
    
    public void ClosePopup()
    {
        onSoundButton.onClick.RemoveListener(OnSound);
        offSoundButton.onClick.RemoveListener(OffSound);
        gameObject.SetActive(false);
        SaveGameMechanic.Instance.Save();
    }

    private void OnSound()
    {
        SoundMonoMechanic.Instance.OnSound();
        onSoundButton.gameObject.SetActive(false);
        offSoundButton.gameObject.SetActive(true);
        soundImage.sprite = onSound;
    }

    private void OffSound()
    {
        SoundMonoMechanic.Instance.OffSound();
        onSoundButton.gameObject.SetActive(true);
        offSoundButton.gameObject.SetActive(false);
        soundImage.sprite = offSound;
    }
}
