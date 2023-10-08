using UnityEngine;

public class ScreenShoot : MonoBehaviour
{
    private int screenShotCount;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            screenShotCount++;
            ScreenCapture.CaptureScreenshot("Screenshot" + screenShotCount + ".png");
        }
    }
}
