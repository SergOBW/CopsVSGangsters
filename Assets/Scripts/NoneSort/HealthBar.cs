using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    private PlayerStatsController _playerStatsController;
    [SerializeField] private Image healthImage;

    private void OnEnable()
    {
        _playerStatsController = FindObjectOfType<PlayerStatsController>();
        if (_playerStatsController != null)
        {
            _playerStatsController.OnHealthChanged += Refresh;
            Refresh(_playerStatsController.currentHealth);
        }
    }

    protected void OnDisable()
    {
        if (_playerStatsController != null)
        {
            _playerStatsController.OnHealthChanged -= Refresh;
        }
    }

    private void Refresh(float obj)
    {
        healthText.text = obj.ToString();
        healthImage.fillAmount = obj / 100;
    }
}
