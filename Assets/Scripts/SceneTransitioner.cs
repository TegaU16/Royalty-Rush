using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Make sure the game starts fully visible.
        fadeCanvas.alpha = 0f;
        fadeCanvas.blocksRaycasts = false;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return Fade(1f);

        yield return SceneManager.LoadSceneAsync(sceneName);

        yield return Fade(0f);
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        yield return Fade(1f);

        yield return SceneManager.LoadSceneAsync(sceneIndex);

        yield return Fade(0f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvas.alpha;
        float elapsed = 0f;

        fadeCanvas.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null;
        }

        fadeCanvas.alpha = targetAlpha;

        if (targetAlpha == 0f)
            fadeCanvas.blocksRaycasts = false;
    }
}
