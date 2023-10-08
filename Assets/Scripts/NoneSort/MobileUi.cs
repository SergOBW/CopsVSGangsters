using UnityEngine;

public class MobileUi : MonoBehaviour
{
    [SerializeField] private GameObject[] mobileInputGo;
    private bool isMobile;

    private void Awake()
    {
#if PLATFORM_ANDROID
        isMobile = true;
#endif
        if (AddManager.Instance != null)
        {
            isMobile = AddManager.Instance.isMobile;
        }
        Setup(isMobile);
    }

    public void Setup(bool isMobile)
    {
        if (isMobile)
        {
            foreach (var gameObject in mobileInputGo)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var gameObject in mobileInputGo)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
