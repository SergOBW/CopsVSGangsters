using TMPro;
using UnityEngine;

public class InteractionUi : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private PlayerDetector _playerDetector;
    public void Initialize()
    {
        _playerDetector = FindObjectOfType<PlayerDetector>();
    }

    private void Update()
    {
        if (_playerDetector == null)
        {
            return;
        }

        if (_playerDetector.CanInteraction)
        {
            _text.gameObject.SetActive(true);
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }
}
