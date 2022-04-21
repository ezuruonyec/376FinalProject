using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Attack
    public int health = 100;
    public float attackRange = 0.5f;
    public int attackDamage = 10;

    public Transform attackPoint;

    //Movement
    public float groundDistance = 0.1f;
    float JumpForce;
    
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 rotation;
    public LayerMask enemyLayer;
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
        if(ver < 0)
            this.transform.Translate(Vector3.forward * ver/10);
        
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

        //Attack
        if(Input.GetMouseButtonDown(0))
            Attack();
        
        //Block
        if(Input.GetKeyDown(KeyCode.LeftShift))
            anim.SetBool("Blocking", true);
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
            anim.SetBool("Blocking", false);
        
        //Pick up
        if(Input.GetMouseButtonDown(1))
            anim.SetTrigger("Gather");
    }

    void Attack(){
        //Do animation
        anim.SetTrigger("Attack");

        //Collision
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        //Do damage
        foreach(Collider enemy in hits){
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage); 
        }
    }

    public void TakeDamage(int damage){
        if(!anim.GetBool("Blocking")){
        health -= damage;
        anim.SetTrigger("Hit");

        //if health below 0 die
        if(health <= 0){
            anim.SetBool("IsAlive", false);
            GetComponent<Collider>().enabled = false;
            this.enabled = false; 
        }
        else{
            anim.SetBool("IsAlive", true);
        }
        }
        else{
            anim.SetTrigger("Blockhit");
        }
    }

void OnDrawGizmosSelected(){
    if(attackPoint == null)
        return;

    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
}
}
