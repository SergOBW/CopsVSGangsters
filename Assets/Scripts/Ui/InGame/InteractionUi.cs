using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUi : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    private PlayerDetector _playerDetector;
    public void Initialize()
    {
        _playerDetector = FindObjectOfType<PlayerDetector>();
        _image.fillAmount = 0;
    }

    private void Update()
    {
        if (_playerDetector == null)
        {
            _image.fillAmount = 0;
            _text.gameObject.SetActive(false);
            return;
        }

        if (_playerDetector.CurrentInteractable != null)
        {
            if (_playerDetector.CurrentInteractable.CanInteract())
            {
                _image.fillAmount = _playerDetector.CurrentInteractable.GetHealthNormalized();
                _text.gameObject.SetActive(!AddManager.Instance.isMobile && _playerDetector.CurrentInteractable.CanInteract());
            }
            else
            {
                _image.fillAmount = 0;
                _text.gameObject.SetActive(false);
            }
        }
        else
        {
            _image.fillAmount = 0;
            _text.gameObject.SetActive(false);
            UiMonoStateMachine.Instance.HideNoBigDrillPopup();
        }

    }
}
