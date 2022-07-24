using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameOver;
    Transform player;

    [Header("Bomb Timer")]
    public int timer;
    [SerializeField] TMP_Text timerText;

    [Header("Harakiri")]
    [SerializeField] RectTransform harakiriProgress;


    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CountdownRoutine());
    }

    public void UpdateUIHarakiri(int enemiesKilled) {
        int maxEnemiesKilled = player.GetComponent<PlayerController>().harakiriMax;
        float scale = enemiesKilled/maxEnemiesKilled;
        harakiriProgress.localScale = new Vector3(scale, scale, scale);
        Debug.Log(harakiriProgress.localPosition);
    }

    void UpdateUITime() {
        TimeSpan t = TimeSpan.FromSeconds(timer);
        timerText.text = t.ToString(@"mm\:ss");
    }

    IEnumerator CountdownRoutine() {
        while (timer > 0) {
            UpdateUITime();
            yield return new WaitForSeconds(1);
            timer--;
        }

        gameOver = true;
        //UIManager.Instance.showGameOverScreen();
    }
}
