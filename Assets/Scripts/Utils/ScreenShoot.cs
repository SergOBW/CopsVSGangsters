using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenShoot : MonoBehaviour
{
    private int screenShotCount;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            screenShotCount++;
            ScreenCapture.CaptureScreenshot($"{SceneManager.GetActiveScene().name}_{screenShotCount}.png");
        }
    }
}
