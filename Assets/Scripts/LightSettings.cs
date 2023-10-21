using DefaultNamespace;
using UnityEngine;

public class LightSettings : MonoBehaviour
{
    private Light _light;
    private void Start()
    {
        _light = GetComponent<Light>();
        GlobalSettings.Instance.OnSettingsChanged += OnSettingsChanged;
        OnSettingsChanged();
    }

    private void OnSettingsChanged()
    {
        switch (GlobalSettings.Instance.graphicsQuality)
        {
            case GraphicsQuality.High:
                _light.enabled = true;
                break;
            default: _light.enabled = false;
                break;
        }
    }
}
