using Abstract.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text descriptionText;
    public TMP_Text titleText;

    [SerializeField]private Button _closeButton;
    
    public void Show(InventoryItem inventoryItem)
    {
        gameObject.SetActive(true);
        itemIcon.sprite = inventoryItem.itemIcon;
        switch (LanguageManager.Instance.GetLanguage())
        {
            case Language.en:
                descriptionText.text = inventoryItem.descriptionEn;
                break;
            case Language.ru:
                descriptionText.text = inventoryItem.descriptionRu;
                break;
            case Language.tr:
                descriptionText.text = inventoryItem.descriptionTr;
                break;
            default:
                descriptionText.text = inventoryItem.descriptionEn;
                break;
        }
        titleText.text = inventoryItem.name;
        
        _closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        _closeButton.onClick.RemoveListener(Close);
        gameObject.SetActive(false);
    }
}
