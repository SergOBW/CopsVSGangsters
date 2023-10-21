using System;
using UnityEngine;

namespace DefaultNamespace
{
    public enum GraphicsQuality{
        Low = 0,
        Medium = 1,
        High = 2
    }
    public class GlobalSettings
    {
        public GraphicsQuality graphicsQuality;
        public float soundValue;
        public bool isUsingDebug;

        public static GlobalSettings Instance;

        public event Action OnSettingsChanged;
        
        public void Initialize()
        {
            Instance = this;
            SetupDefault();
        }

        private void SetupDefault()
        {
            graphicsQuality = GraphicsQuality.Low;
            isUsingDebug = false;
            soundValue = 1f;
        }

        public void ChangeSettings()
        {
            Debug.Log(graphicsQuality);
            OnSettingsChanged?.Invoke();
        }
    }
}