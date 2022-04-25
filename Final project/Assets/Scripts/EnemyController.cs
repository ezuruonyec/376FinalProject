using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Inspired by https://www.youtube.com/watch?v=UjkSFoLxesw
//with additional components by Hannah Cain 

public class EnemyController : MonoBehaviour
{
    //Character setup
    public UnityEngine.AI.NavMeshAgent agent;
    //public AudioSource swordNoise;
    public Transform player;
    public LayerMask isGround, isPlayer;
    private Animator anim;

    //Health
    public int maxHealth = 30;
    int curHealth; 

    //Watching
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float betweenAttacks;
    bool didAttack;
    public Transform attackPoint;

    //States
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttack;

    private void Awake(){
        player = GameObject.Find("Player").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start(){
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    // Fixed update is called a set number of times per second
    private void FixedUpdate(){
        //Is player in sight and/or attack range
        playerInSight = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if(!playerInSight && !playerInAttack) Patrol();
        if(playerInSight && !playerInAttack) Chase();
        if(playerInSight && playerInAttack && curHealth != 0) Attack();
    }

    //Control patrol state
    private void Patrol(){
        anim.SetBool("Running", false);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distToWalkPoint = transform.position - walkPoint;

        //Walk point reached
        if(distToWalkPoint.magnitude < 1f){
            anim.SetBool("Running", false);
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint(){
        //Random point in range
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround)) 
            walkPointSet = true;
    }

    //If player is spotted, follow
    private void Chase(){
        agent.SetDestination(player.position);
        anim.SetBool("Running", true);
    }

    //If player is in range attack them
    private void Attack(){
        //Freeze movement 
        anim.SetBool("Running", false);
        agent.SetDestination(transform.position);

        //Make sure looking at player 
        transform.LookAt(player);

        if (!didAttack){

            //Do animation
            anim.SetTrigger("Attack");
            //swordNoise.Play();

            //Collision
            Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, isPlayer);

            //Do damage
            foreach (Collider enemy in hits)
            {
                enemy.GetComponent<PlayerController>().TakeDamage(5);
            }

            //Timer between attacks
            didAttack = true;
            Invoke(nameof(ResetAttack), betweenAttacks);
        }
    }

    //If hit by player, impact health 
    public void TakeDamage(int damage){
        //Stop movement 
        anim.SetBool("Running", false);
        didAttack = true;

        //Take damage 
        curHealth -= damage;
        anim.SetTrigger("Damaged");

        //if health below 0 die
        if(curHealth <= 0){
            anim.SetBool("Running", false);
            didAttack = true;
            anim.SetBool("IsAlive", false);
            Destroy(gameObject, 5);
        }
    }

    //Resets attack timer 
    private void ResetAttack(){
        anim.SetBool("Running", false);
        didAttack = false;
    }

    //Visible sword collider sphere 
    void OnDrawGizmosSelected(){
    if(attackPoint == null)
        return;

    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
}
   
    
}
