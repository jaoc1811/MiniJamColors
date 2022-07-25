using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] string menuScene;
    [SerializeField] GameObject timeline;

    private void OnPause() {
        if (GameIsPaused) {
            Resume();
        } else {
            Pause();
        }
    }

    public void Resume() {
        StartCoroutine(ResumeCoroutine());
    }

    IEnumerator ResumeCoroutine() {
        timeline.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        timeline.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    public void LoadMenu() {
        StartCoroutine(LoadMenuCoroutine(menuScene));
    }

    IEnumerator LoadMenuCoroutine(string sceneName) {
        timeline.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(sceneName);
    }
}
