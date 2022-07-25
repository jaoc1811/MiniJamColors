using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] string gameSceneName;
    [SerializeField] string menuSceneName;

    [Header("Fade")]
    [SerializeField] Image fade;
    [SerializeField] float fadeDuration = 1f;
    public void LoadGame() {
        StartCoroutine(LoadGameCoroutine());
    }

    public void LoadMenu() {
        SceneManager.LoadScene(menuSceneName);
    }

    public void ExitGame() {
        Application.Quit();
    }

    IEnumerator LoadGameCoroutine() {
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(gameSceneName);
    }

    public IEnumerator FadeIn() {
        float counter = 0f;
        while (counter < fadeDuration) {
            counter += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, counter / fadeDuration);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, newAlpha);
            yield return null;
        }

    }
}
