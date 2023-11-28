using System;
using Save;
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
        public float sensitivity;

        public event Action OnSettingsChanged;
        
        public void Initialize()
        {
            Instance = this;
            SetupDefault();
            SetupGameSaves(SaveGameMechanic.Instance.GetGameSaves()); 
            SaveGameMechanic.Instance.OnDataRefreshed += SetupGameSaves;
        }

        private void SetupGameSaves(GameSaves obj)
        {
            sensitivity = obj.sensitivity;
            soundValue = obj.sound;
            ChangeSettings();
        }

        private void SetupDefault()
        {
            graphicsQuality = GraphicsQuality.Low;
            isUsingDebug = false;
            soundValue = 0.4f;
            sensitivity = 1f;
        }

        public void ChangeGraphics(GraphicsQuality _graphicsQuality)
        {
            graphicsQuality = _graphicsQuality;
            GameSettings.Instance.ChangeRenderPipeLine(graphicsQuality);
            ChangeSettings();
        }

        public void ChangeSettings()
        {
            OnSettingsChanged?.Invoke();
        }

        public void ChangeSensitivity(float value)
        {
            sensitivity = value;
            SaveGameMechanic.Instance.SaveSensitivity(sensitivity);
            ChangeSettings();
        }

        public void ChangeVolume(float value)
        {
            soundValue = value;
            SaveGameMechanic.Instance.SaveSound(soundValue);
            ChangeSettings();
        }

        public float GetSoundValue()
        {
            return Mathf.Log10(soundValue) * 20;
        }
    }
}