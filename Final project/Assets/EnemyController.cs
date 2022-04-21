using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Inspired by https://www.youtube.com/watch?v=UjkSFoLxesw
//with additional components

public class EnemyController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
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

    void Start(){
        curHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    private void Update(){
        //Is player in sight and/or attack range
        playerInSight = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if(!playerInSight && !playerInAttack) Patrol();
        if(playerInSight && !playerInAttack) Chase();
        if(playerInSight && playerInAttack) Attack();
    }

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

    private void Chase(){
        agent.SetDestination(player.position);
        anim.SetBool("Running", true);
    }

    private void Attack(){
        //Freeze movement 
        anim.SetBool("Running", false);
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!didAttack){

            //Do animation
        anim.SetTrigger("Attack");

        //Collision
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, isPlayer);

        //Do damage
        foreach(Collider enemy in hits){
            enemy.GetComponent<PlayerController>().TakeDamage(10); 
        }

            didAttack = true;
            Invoke(nameof(ResetAttack), betweenAttacks);
        }
    }

    public void TakeDamage(int damage){
        anim.SetBool("Running", false);
        curHealth -= damage;
        anim.SetTrigger("Damaged");

        //if health below 0 die
        if(curHealth <= 0){
            anim.SetBool("IsAlive", false);
            Destroy(gameObject, 5);
        }
        else{
            anim.SetBool("IsAlive", true);
        }
    }

    private void ResetAttack(){
        anim.SetBool("Running", false);
        didAttack = false;
    }

    void OnDrawGizmosSelected(){
    if(attackPoint == null)
        return;

    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
}
   
    
}
