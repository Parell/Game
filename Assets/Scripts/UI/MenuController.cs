using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [SerializeField] private int menuScene = 1;
    [SerializeField] private int sceneToStart = 2;
    bool isPaused;
    [Space]
    [SerializeField] private GameObject tint;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SceneLoader.Instance.LoadScene(menuScene, LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isPaused && SceneLoader.Instance.currentScene != 1)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && isPaused && SceneLoader.Instance.currentScene != 1)
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
        StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(SceneLoader.FadeDirection.In, sceneToStart, LoadSceneMode.Single));
        yield return new WaitForSeconds(1f);
        HideMainMenu();
        UnPause();
    }

    public void StopGame()
    {
        StartCoroutine(OnStopGame());
    }

    private IEnumerator OnStopGame()
    {
        yield return StartCoroutine(SceneLoader.Instance.FadeAndLoadScene(SceneLoader.FadeDirection.In, menuScene, LoadSceneMode.Single));
        ShowMainMenu();
        UnPause();
    }

    public void Quit()
    {
        StartCoroutine(OnQuit());
    }

    private IEnumerator OnQuit()
    {
        yield return SceneLoader.Instance.Fade(SceneLoader.FadeDirection.In);

#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
