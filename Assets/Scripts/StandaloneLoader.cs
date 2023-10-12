using Player;
using UnityEngine;
using Yandex.Plugins.Login;

public class StandaloneLoader : MonoBehaviour
{
    [SerializeField] private UiMonoStateMachine _uiMonoStateMachine;
    [SerializeField] private GlobalMechanicSetup _globalMechanicSetup;

    private void Start()
    {
        //TODO: Need to handle
        SaveGameMechanic saveGameMechanic = new SaveGameMechanic();
        saveGameMechanic.Initialize();
        LoginManager loginManager = new LoginManager();
        loginManager.Initialize();
        LanguageManager languageManager = new LanguageManager();
        languageManager.Initialize();
        
        _globalMechanicSetup.InitializeMechanics();
        _uiMonoStateMachine.InitializeStandalone();
        FindObjectOfType<PlayerStatsController>().Initialize();
        LevelStateMachine.Instance.SetLevelPlayState();
    }
}
