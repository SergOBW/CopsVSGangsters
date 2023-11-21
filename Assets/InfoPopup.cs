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
                titleText.text = inventoryItem.name;
                break;
            case Language.ru:
                descriptionText.text = inventoryItem.descriptionRu;
                titleText.text = inventoryItem.nameRu;
                break;
            case Language.tr:
                descriptionText.text = inventoryItem.descriptionTr;
                titleText.text = inventoryItem.nameTr;
                break;
            default:
                descriptionText.text = inventoryItem.descriptionEn;
                titleText.text = inventoryItem.name;
                break;
        }
        
        _closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        _closeButton.onClick.RemoveListener(Close);
        gameObject.SetActive(false);
    }
}
