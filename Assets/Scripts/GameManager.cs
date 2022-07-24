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

    [Header("Attack")]
    [SerializeField] Image attackUI;

    [Header("Dodge")]
    [SerializeField] Image DodgeUI;
    

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(CountdownRoutine());
    }

    public void UpdateUIHarakiri(float enemiesKilled) {
        float maxEnemiesKilled = player.GetComponent<PlayerController>().harakiriMax;
        float scale = 1-enemiesKilled/maxEnemiesKilled;
        harakiriProgress.localScale = new Vector3(harakiriProgress.localScale.x, scale, harakiriProgress.localScale.x);
        Debug.Log("enemiesKilled: " + enemiesKilled);
        Debug.Log("max: " + maxEnemiesKilled);
        Debug.Log(enemiesKilled/maxEnemiesKilled);
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
