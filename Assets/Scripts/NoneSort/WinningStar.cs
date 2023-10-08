using UnityEngine;
using UnityEngine.UI;

public class WinningStar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite winStarSprite;
    [SerializeField] private Sprite looseStarSprite;

    public void SetupStar(bool isWinningStar)
    {
        if (isWinningStar)
        {
            image.sprite = winStarSprite;
        }
        else
        {
            image.sprite = looseStarSprite;
        }
    }
}
