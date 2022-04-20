using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 rotation;
    public float groundDistance = 0.1f;
    float JumpForce;
    public LayerMask isGround;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        JumpForce = Mathf.Sqrt(-1 * Physics.gravity.y);
        
    }

    void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        //Froward motion 
        anim.SetFloat("Speed", ver);
        //Move back
        if(ver < 0){
            this.transform.Translate(Vector3.forward * ver/10);
        }
        //Turn left-right
        this.rotation = new Vector3(0, hor * 180 * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);
        //Jump
        if(Input.GetButtonDown("Jump")){
            if(rigid.position.y <= 0.5){
                rigid.AddForce(Vector3.up * JumpForce * 0.75f, ForceMode.Impulse);
                anim.SetTrigger("Jump");
            }
        }
        //Land from jump
        if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, isGround)){
            anim.SetBool("Grounded", true);
        } else {
            anim.SetBool("Grounded", false);
            this.transform.Translate(Vector3.forward * ver/10);
        }
        
    }
    void LateUpdate() {
        //Attack
        if(Input.GetMouseButtonDown(0)){
            anim.SetTrigger("Attack");
        }

        //Block
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            anim.SetBool("Blocking", true);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            anim.SetBool("Blocking", false);
        }

        //Take damage
        //if skelly attack collide trigger
        // anim.SetTrigger("Hit");
        // subtract health 

        //Pick up
        if(Input.GetMouseButtonDown(1)){
            anim.SetTrigger("Gather");
        }
        
        //Death and get up
        if(health <= 0){
            anim.SetBool("IsAlive", false);
        }
        else{
            anim.SetBool("IsAlive", true);
        }

    }
}
