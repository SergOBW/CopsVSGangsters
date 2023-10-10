using System;
using System.Collections.Concurrent;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yandex.Plugins.Login;

public class Loader : MonoBehaviour
{
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private GameObject globalMechanicSetupPrefab;

    [SerializeField] private string startedSceneName = "Hub";
    

    private void Start()
    {
        Application.targetFrameRate = 144;
            
        SaveGameMechanic saveGameMechanic = new SaveGameMechanic();
        saveGameMechanic.Initialize();
        LoginManager loginManager = new LoginManager();
        loginManager.Initialize();
        LanguageManager languageManager = new LanguageManager();
        languageManager.Initialize();
        WeaponManagerMechanic weaponManagerMechanic = new WeaponManagerMechanic();
        weaponManagerMechanic.Initialize();
            
        GameObject gameManager = Instantiate(globalMechanicSetupPrefab);
        DontDestroyOnLoad(gameManager);
        gameManager.name = "GameManager";
        gameManager.GetComponent<GlobalMechanicSetup>().InitializeMechanics();
            
        GameObject ui = Instantiate(uiPrefab);
        DontDestroyOnLoad(ui);
        ui.GetComponent<UiMonoStateMachine>().Initialize();
            
        LoadTheScene();
    }
    

    private void LoadTheScene()
    {
        SceneManager.LoadScene(startedSceneName);
    }
    
    public class Dispatcher : AbstractSingleton<Dispatcher>
    {
        private static readonly ConcurrentBag<Action> pending = new ConcurrentBag<Action>();

        public void Invoke(Action fn)
        {
            pending.Add(fn);
        }

        private void Update()
        {
            InvokePending();
        }

        private void InvokePending()
        {
            /*
             * TODO fix :
             *
             * InvalidOperationException: Collection was modified; enumeration operation may not execute.
             * Happens when you call Dispatcher.Invoke inside of another Dispatcher.Invoke
             */

            while (!pending.IsEmpty)
            {
                pending.TryTake(out var action);

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    // If there is no Catch, the pending list never clears.
                    Debug.LogError("Error happened during invoking action with Dispatcher. Error : ");
                    Debug.LogError(e);
                }
            }
        }
    }
}
