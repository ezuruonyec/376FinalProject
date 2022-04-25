using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//By Hannah Cain and Chiamaka Ezuruonye
public class FriendController : MonoBehaviour
{
    //Character setup
    private GameObject player;
    private PlayerController playerScript;
    private Animator anim;

    //Start is called once per frame 
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    //When the player wins and collides, trigger win 
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Collider>().name == "Player"){
            if(playerScript.win){
                anim.SetTrigger("Revive");
                Invoke(nameof(endGame), 8);
            }
        }
    }
    //When win, show credits 
    public void endGame()
    {
        SceneManager.LoadScene("Credits");
    }
}
