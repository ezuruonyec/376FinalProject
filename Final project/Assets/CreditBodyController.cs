using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//By Hannah Cain and Chiamaka Ezuruonye

public class CreditBodyController : MonoBehaviour
{
    //Delay to show credits before returning to home screen 
    public float delay = 30;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScenePost(delay));
    }

    //Handles timing
    IEnumerator LoadScenePost(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("WelcomeScene");
    }
}
