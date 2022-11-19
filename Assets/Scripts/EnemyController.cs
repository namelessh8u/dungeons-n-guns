using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D theRB;
    public float moveSpeed;


    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    [Header("Run Away")]
    public bool shouldRunAway;
    public float runAwayRange;

    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLenght;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patroling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

   

    [Header("Shooting")]
    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public SpriteRenderer theBody;

    public float shootRange;

    [Header("Variables")]
    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldWander) pauseCounter = Random.Range(pauseLenght * .75f, pauseLenght * 1.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if(theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy && PlayerController.instance.canMove)
        {
            //moving
            moveDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer) moveDirection = PlayerController.instance.transform.position - transform.position;
            else if (shouldWander)
            {
                if (wanderCounter > 0)
                {
                    wanderCounter -= Time.deltaTime;

                    moveDirection = wanderDirection;

                    if (wanderCounter <= 0) pauseCounter = Random.Range(pauseLenght * .75f, pauseLenght * 1.25f);
                }

                if (pauseCounter > 0)
                {
                    pauseCounter -= Time.deltaTime;

                    if (pauseCounter <= 0)
                    {
                        wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                        wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                    }
                }
            }
            else if (shouldPatrol)
            {
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;
                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint >= patrolPoints.Length) currentPatrolPoint = 0;
                }
            }
            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange) moveDirection = transform.position - PlayerController.instance.transform.position;
            

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;

            //shoot
            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                    AudioManager.instance.PlaySFX(13);
                }
            }

        }
        else
        {
            theRB.velocity = Vector3.zero;
        }

        //animation
        if (moveDirection != Vector3.zero) anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving", false);

    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        Instantiate(hitEffect, transform.position, transform.rotation);

        AudioManager.instance.PlaySFX(2);

        if (health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(1);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[Random.Range(0, deathSplatters.Length)], transform.position, Quaternion.Euler(0f, 0f, 90f * rotation));

            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance <= itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
}
