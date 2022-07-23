using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int time;
    public bool gameOver;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine() {
        while (time > 0) {
            //UIManager.Instance.UpdateUITime(time);
            yield return new WaitForSeconds(1);
            time--;
        }

        gameOver = true;
        //UIManager.Instance.showGameOverScreen();
    }
}
