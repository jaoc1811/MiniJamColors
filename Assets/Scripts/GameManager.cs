using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Transform player;

    [Header("Bomb Timer")]
    public int timer;
    [SerializeField] TMP_Text timerText;
    [SerializeField] AudioClip timerAudio;

    [Header("UI")]
    [SerializeField] RectTransform harakiriProgress;
    [SerializeField] Image attackUI;
    [SerializeField] Image DodgeUI;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject timeline;
    [SerializeField] GameObject surpriseL;
    [SerializeField] GameObject surpriseR;

    [Header("Scenes")]
    [SerializeField] string EndingSceneName;
    [SerializeField] string GameSceneName;
    [SerializeField] string MenuSceneName;
    

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(GameObject.Find("AudioSource"));
        StartCoroutine(CountdownRoutine());
    }

    public void UpdateUIHarakiri(float enemiesKilled) {
        float maxEnemiesKilled = player.GetComponent<PlayerController>().harakiriMax;
        float scale = 1-enemiesKilled/maxEnemiesKilled;
        harakiriProgress.localScale = new Vector3(harakiriProgress.localScale.x, scale, harakiriProgress.localScale.x);
    }

    public void UpdateUIAttack(bool canAttack) {
        Color tempColor = attackUI.color;
        if (canAttack) {
            tempColor.a = 1f;
            attackUI.color = tempColor;
        } else {
            tempColor.a = 0.4f;
            attackUI.color = tempColor;
        }
    }

    public void UpdateUIDodge(bool canDodge) {
        Color tempColor = DodgeUI.color;
        if (canDodge) {
            tempColor.a = 1f;
            DodgeUI.color = tempColor;
        } else {
            tempColor.a = 0.4f;
            DodgeUI.color = tempColor;
        }
    }

    void UpdateUITime() {
        TimeSpan t = TimeSpan.FromSeconds(timer);
        timerText.text = t.ToString(@"mm\:ss");
    }

    public void LoadEnding() {
        SceneManager.LoadScene(EndingSceneName);
    }

    public void LoadGame() {
        StartCoroutine(LoadSceneCoroutine(GameSceneName));
    }

    public void LoadMenu() {
        StartCoroutine(LoadSceneCoroutine(MenuSceneName));
    }

    IEnumerator LoadSceneCoroutine(string sceneName) {
        timeline.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator CountdownRoutine() {
        while (timer > 0) {
            UpdateUITime();
            yield return new WaitForSeconds(1);
            timer--;
            if(timer==5) {
                surpriseL.SetActive(true);
                AudioSource.PlayClipAtPoint(timerAudio, transform.position);
            }
            if (timer < 5) {
                surpriseL.SetActive(!surpriseL.activeInHierarchy);
                surpriseR.SetActive(!surpriseL.activeInHierarchy);
            }
        }
        surpriseL.SetActive(false);
        surpriseR.SetActive(false);
        UpdateUITime();

        while(player.GetComponent<PlayerController>().harakiri) {
            yield return new WaitForEndOfFrame();
        }

        player.GetComponent<PlayerController>().Die();
    }

    public void GameOverScreen() {
        gameOverScreen.SetActive(true);
    }
}
