using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 rotation;
    public float groundDistance = 0.0f;
    public float JumpForce = 500;
    public LayerMask isGround;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        anim.SetFloat("Speed", ver);
        this.rotation = new Vector3(0, hor * 180 * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);
        if(Input.GetButtonDown("Jump")){
            rigid.AddForce(Vector3.up * JumpForce);
            anim.SetTrigger("Jump");
        }
        if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, isGround)){
            anim.SetBool("Grounded", true);
            anim.applyRootMotion = true;
        } else {
            anim.SetBool("Grounded", false);
        }
        
    }
}
