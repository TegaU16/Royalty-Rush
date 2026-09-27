using System.Collections;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    public static GameEndUI Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private GameObject statsUI;

    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackScreenDelay = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        fadeGroup.alpha = 0f;
        statsUI.SetActive(false);
    }

    public void EndGame()
    {
        StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine()
    {
        yield return FadeToBlack();

        yield return new WaitForSeconds(blackScreenDelay);

        statsUI.SetActive(true);
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);

            yield return null;
        }

        fadeGroup.alpha = 1f;
    }
}
