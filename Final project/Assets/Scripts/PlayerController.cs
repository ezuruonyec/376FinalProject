using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//By Hannah Cain and Chiamaka Ezuruonye

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
    
    //Player setup 
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 rotation;
    public LayerMask enemyLayer;
    public LayerMask isGround;
    //public AudioSource swordNoise;
    //public AudioSource deflectNoise;

    //Collecting and Scoring
    int CubeCount = 0;
    int CapsCount = 0;
    int SphCount = 0;
    public bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        JumpForce = Mathf.Sqrt(-1 * Physics.gravity.y);
    }

    //Fixed update is called a set number of times per second
    void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        //Froward motion 
        anim.SetFloat("Speed", ver);

        //Move back
        if (ver < 0)
            this.transform.Translate(Vector3.forward * ver / 10);

        //Turn left-right
        this.rotation = new Vector3(0, hor * 180 * Time.deltaTime, 0);
        this.transform.Rotate(this.rotation);

        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (rigid.position.y <= 0.5)
            {
                rigid.AddForce(Vector3.up * JumpForce * 0.75f, ForceMode.Impulse);
                anim.SetTrigger("Jump");
            }
        }

        //Land from jump
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, isGround))
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
            this.transform.Translate(Vector3.forward * ver / 10);
        }

        //Attack
        if (Input.GetMouseButtonDown(0))
            Attack();

        //Block
        if (Input.GetKeyDown(KeyCode.LeftShift))
            anim.SetBool("Blocking", true);

        if (Input.GetKeyUp(KeyCode.LeftShift))
            anim.SetBool("Blocking", false);

        //Pick up - not needed but still cool
        if (Input.GetMouseButtonDown(1))
            anim.SetTrigger("Gather");

        //Win case   
        win = WinCheck();
    }

    //Attack enemy 
    void Attack(){
        //Do animation
        anim.SetTrigger("Attack");
        //swordNoise.Play();

        //Collision
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        //Do damage
        foreach(Collider enemy in hits){
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage); 
        }
    }

    //If attacked by enemy, be impacted 
    public void TakeDamage(int damage){
        if(!anim.GetBool("Blocking")){
            //Act it 
            anim.SetTrigger("Hit");
            //deflectNoise.Play();

            //take damage 
            health -= damage;

            //if health below 0 die
            if (health <= 0)
            {
                anim.SetBool("IsAlive", false);
                Invoke(nameof(endGame), 10);
            }
            else
            {
                anim.SetBool("IsAlive", true);
            }
        }
        else{
            //If blocking when attacked, no damage 
            anim.SetTrigger("Blockhit");
        }
    }

    //When collecting items 
    void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("Gather");

        if (other.tag == "Cubes")
        {
            CubeCount += 1;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Capsules")
        {
            CapsCount += 1;
            Destroy(other.gameObject);
        }
        else if (other.tag == "Spheres")
        {
            SphCount += 1;
            Destroy(other.gameObject);
        }
    }
    
    //Make sure necessary items are collected 
    bool WinCheck(){
        return (CubeCount >= 5 &&
            CapsCount >= 5 &&
            SphCount >= 5);
    }
    
    //If die return to main menu
    public void endGame()
    {
        SceneManager.LoadScene("WelcomeScene");
    }

    //See sword collider sphere 
    void OnDrawGizmosSelected(){
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
