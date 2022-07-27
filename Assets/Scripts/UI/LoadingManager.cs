using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    public enum FadeDirection { In, Out }

    public int previousScene;
    public int currentScene;

    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] float fadeTime = 0.5f;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError("More then one instances exist, only one can be used");
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        fadeImage.alpha = 1f;
        StartCoroutine(Fade(FadeDirection.Out));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        previousScene = currentScene;
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;
        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetAlpha(ref alpha, fadeDirection);
                yield return null;
            }
            fadeImage.gameObject.SetActive(false);
        }
        else
        {
            fadeImage.gameObject.SetActive(true);
            while (alpha <= fadeEndValue)
            {
                SetAlpha(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }

    void SetAlpha(ref float alpha, FadeDirection fadeDirection)
    {
        fadeImage.alpha = alpha;
        alpha += Time.deltaTime * (1f / fadeTime) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }

    public void LoadScene(int sceneToLoad, LoadSceneMode loadSceneMode)
    {
        SceneManager.LoadScene(sceneToLoad, loadSceneMode);
    }

    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, int sceneToLoad, LoadSceneMode loadSceneMode)
    {
        yield return Fade(fadeDirection);
        SceneManager.LoadScene(sceneToLoad, loadSceneMode);
        yield return Fade(fadeDirection == FadeDirection.In ? FadeDirection.Out : FadeDirection.In);
    }
}
