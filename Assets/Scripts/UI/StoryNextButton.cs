using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class StoryNextButton : MonoBehaviour
{
    [SerializeField] GameObject[] storySequence;
    [SerializeField] GameObject[] tutorialControls;
    [SerializeField] GameObject[] bombWarnings;
    [SerializeField] GameObject BlackScreen;
    [SerializeField] GameObject ThankYouScreen;
    [SerializeField] string nextScene;

    int currentScene;

    private void Start() {
        currentScene = 1;
    }

    public void ChangeScene(){
        if (currentScene == 1){ // Starting, deactivate scenes 0 and 1
            storySequence[currentScene-1].SetActive(false);
        }

        // If last scene, dont deactivate current story scene, just fade to black
        if (currentScene + 1 >= storySequence.Length){
            StartCoroutine(loadScene(nextScene));
        } else { // Other scene
            storySequence[currentScene].SetActive(false);
            currentScene++;
            storySequence[currentScene].SetActive(true);
            Debug.Log(currentScene);
            if (currentScene + 1 == storySequence.Length && ThankYouScreen){ // Last scene
                ThankYouScreen.SetActive(true);
            }
        }
    }

    public void ChangeSceneTutorial(){
        if (bombWarnings[0].activeInHierarchy){
            DirectLoadScene();
            return;
        }
        foreach(GameObject go in tutorialControls){
            go.SetActive(false);
        }
        foreach(GameObject go in bombWarnings){
            go.SetActive(true);
        }
    }

    void disableButtons(){
        foreach(GameObject button in GameObject.FindGameObjectsWithTag("Button")){
            button.GetComponent<Image>().enabled = false; 
            button.GetComponent<Button>().enabled = false;

            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            if (text) text.enabled = false;
        }
    }

    public void SkipStory(){
        StartCoroutine(loadScene(nextScene));
    }

    public void DirectLoadScene(){
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator loadScene(string sceneName){
        if (ThankYouScreen){
            ThankYouScreen.SetActive(false);
        }
        BlackScreen.SetActive(true);
        // Disable buttons
        disableButtons();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
