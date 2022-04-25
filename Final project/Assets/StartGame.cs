using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//By Hannah Cain and Chiamaka Ezuruonye

public class StartGame : MonoBehaviour
{
    //Delay to show credits before returning to home screen 
    public float delay = 20;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScenePost(delay));
    }

    //Handles timing
    IEnumerator LoadScenePost(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainGameScene");
    }
}
