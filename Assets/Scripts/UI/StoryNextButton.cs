using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryNextButton : MonoBehaviour
{
    [SerializeField] GameObject[] storySequence;
    [SerializeField] GameObject BlackScreen;

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
            StartCoroutine(loadGame());
        } else { // Other scene
            storySequence[currentScene].SetActive(false);
            currentScene++;
            storySequence[currentScene].SetActive(true);
        }
    }

    void disableButtons(){
        foreach(GameObject button in GameObject.FindGameObjectsWithTag("Button")){
            button.GetComponent<Image>().enabled = false; 
            button.GetComponent<Button>().enabled = false;
        }
    }

    public void SkipStory(){
        StartCoroutine(loadGame());
    }

    IEnumerator loadGame(){
        BlackScreen.SetActive(true);
        // Disable buttons
        disableButtons();
        yield return new WaitForSeconds(2f);
        Debug.Log("Load game scene");
    }
}
