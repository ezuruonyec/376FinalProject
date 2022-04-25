using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//By Hannah Cain and Chiamaka Ezuruonye

public class MenuController : MonoBehaviour
{
    //When user hits play, start game
    public void Play(){
        SceneManager.LoadScene("IntroScene");
    }

    public void QuitGame(){
        
        Application.Quit();
    }
}
