using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerScript;
    private Animator anim;

    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Collider>().name == "Player"){
            if(playerScript.win){
                anim.SetTrigger("Revive");
                Invoke(nameof(endGame), 10);
            }
        }
    }

    public void endGame()
    {
        SceneManager.LoadScene("Credits");
    }
}
