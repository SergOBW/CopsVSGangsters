using TMPro;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    private PlayerCharacter _inventory;

    private void OnEnable()
    {
        _inventory = FindObjectOfType<PlayerCharacter>();
    }
    
    private void OnDisable()
    {
        _inventory = null;
    }

    private void Update()
    {
        if (_inventory != null)
        {
            Refresh(_inventory.GetAmmunitionCurrent());
        }
    }

    private void Refresh(int obj)
    {
        ammoText.text = obj.ToString();
    }
    
}
