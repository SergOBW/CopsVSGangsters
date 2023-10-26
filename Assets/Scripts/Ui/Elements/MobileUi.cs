using UnityEngine;

public class MobileUi : MonoBehaviour
{
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
        gameObject.SetActive(isMobile);
    }
}
