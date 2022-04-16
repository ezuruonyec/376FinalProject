using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 rotation;
    public float groundDistance = 0.0f;
    public float JumpForce;
    public LayerMask isGround;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        JumpForce = Mathf.Sqrt(2 * -2 * Physics.gravity.y);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        //Froward motion 
        anim.SetFloat("Speed", ver);
        //TODO: Move back
        //Turn left-right
        this.rotation = new Vector3(0, hor * 180 * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);
        //Jump
        if(Input.GetButtonDown("Jump")){
            rigid.AddForce(new Vector3(rigid.velocity.x, JumpForce, rigid.velocity.z), ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
        //Land from jump
        if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, isGround)){
            anim.SetBool("Grounded", true);
            anim.applyRootMotion = true;
        } else {
            anim.SetBool("Grounded", false);
        }
        
    }
    void LateUpdate() {
        //Attack
        if(Input.GetMouseButtonDown(0)){
            anim.SetTrigger("Attack");
        }
        //TODO: Take damage
        //TODO: Duck
        //TODO: Pick up
        //TODO: Die
        //TODO: Get up
    }
}
