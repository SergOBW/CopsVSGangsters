using CrazyGames;
using Ui.States;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUi : MonoBehaviour
{
    [SerializeField] private GameObject shootTutorialPc;
    [SerializeField] private GameObject shootTutorialMobile;
    [SerializeField] private GameObject moveTutorialPc;
    [SerializeField] private GameObject moveTutorialMobile;

    [SerializeField] private PlayerInput playerInput;
    
    private bool isClickCompleted;
    private float timer;
    private UiMonoPlayState _uiMonoPlayState;
    
    public void Initialize()
    {
        Debug.Log("Tutorial initialize");
    }

    private void StartTutorial(UiMonoPlayState uiMonoPlayState)
    {
        if (AddManager.Instance.isMobile)
        {
            shootTutorialMobile.SetActive(true);
        }
        else
        {
            shootTutorialPc.SetActive(true);
        }
        LevelStateMachine.Instance.Tutorial();
        isClickCompleted = false;
        playerInput.enabled = true;
        playerInput.ActivateInput();
        timer = 0;
        _uiMonoPlayState = uiMonoPlayState;
        
        Debug.Log("Tutorial started");
    }

    private void Update()
    {
        if (isClickCompleted && timer < 2f)
        {
            timer += Time.deltaTime;
        }
    }

    public void ClickCompleted(InputAction.CallbackContext context)
    {
        if (AddManager.Instance.isMobile)
        {
            shootTutorialMobile.SetActive(false);
            moveTutorialMobile.SetActive(true);
        }
        else
        {
            shootTutorialPc.SetActive(false);
            moveTutorialPc.SetActive(true);
        }
        isClickCompleted = true;
    }
    
    public void MoveCompeted(InputAction.CallbackContext context)
    {
        if (!isClickCompleted || timer <= 1f)
        {
            return;
        }
        if (AddManager.Instance.isMobile)
        {
            shootTutorialMobile.SetActive(false);
            moveTutorialMobile.SetActive(false);
        }
        else
        {
            shootTutorialPc.SetActive(false);
            moveTutorialPc.SetActive(false);
        }
        LevelStateMachine.Instance.SetLevelPlayState();
        if (AddManager.Instance.AddAggregator == AddAggregator.CrazyGames)
        {
            CrazyEvents.Instance.GameplayStart();
        }
        playerInput.enabled = false;
    }
    
}
