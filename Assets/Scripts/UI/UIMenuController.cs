using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
    public static UIMenuController Instance;

    [SerializeField] private int menuScene = 1;
    [SerializeField] private int sceneToStart = 2;
    bool isPaused;
    [Space]
    [SerializeField] private GameObject tint;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject savesMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadingManager.Instance.LoadScene(menuScene, LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isPaused && LoadingManager.Instance.currentScene != 1)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && isPaused && LoadingManager.Instance.currentScene != 1)
        {
            UnPause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        //SystemManager.Instance.isCameraLocked = true;
        //SystemManager.Instance.isMovementLocked = true; 

        ShowPauseMenu();
    }

    public void UnPause()
    {
        isPaused = false;
        //SystemManager.Instance.isCameraLocked = false;
        //SystemManager.Instance.isMovementLocked = false; 

        HidePauseMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
    }

    public void HideMainMenu()
    {
        mainMenu.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        settingsMenu.SetActive(true);
        tint.SetActive(true);
    }

    public void HideSettingsMenu()
    {
        settingsMenu.SetActive(false);
        tint.SetActive(false);
    }

    public void ShowSavesMenu()
    {
        savesMenu.SetActive(true);
        tint.SetActive(true);
    }

    public void HideSavesMenu()
    {
        savesMenu.SetActive(false);
        tint.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        tint.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        tint.SetActive(false);
    }

    public void StartGame()
    {
        StartCoroutine(OnStartGame());
    }

    private IEnumerator OnStartGame()
    {
        StartCoroutine(LoadingManager.Instance.FadeAndLoadScene(LoadingManager.FadeDirection.In, sceneToStart, LoadSceneMode.Single));
        yield return new WaitForSeconds(1f);
        HideMainMenu();
        HideSavesMenu();
        UnPause();
    }

    public void StopGame()
    {
        StartCoroutine(OnStopGame());
    }

    private IEnumerator OnStopGame()
    {
        StartCoroutine(LoadingManager.Instance.FadeAndLoadScene(LoadingManager.FadeDirection.In, menuScene, LoadSceneMode.Single));
        yield return new WaitForSeconds(1f);
        ShowMainMenu();
        UnPause();
    }

    public void Quit()
    {
        StartCoroutine(OnQuit());
    }

    private IEnumerator OnQuit()
    {
        yield return LoadingManager.Instance.Fade(LoadingManager.FadeDirection.In);

#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
