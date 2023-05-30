using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    //Searh and follow
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerLayer, groundLayer;
    public Vector3 specialPoint;
    bool isPointset;
    public float pointRange;
    public float visionRange;
    public bool inVisionRange;

    //Attack
    public float attackTime;
    bool attacked;
    public GameObject bullet;
    public float attackRange;
    public bool inAttackRange;
    public float timer;

    private void Searchpoint()
    {
        float randPointZ = Random.Range(-pointRange, pointRange);
        float randPointx = Random.Range(-pointRange, pointRange);
        agent.speed = 5f;

        specialPoint = new Vector3(transform.position.x + randPointx,transform.position.y,transform.position.z + randPointZ);

        if (Physics.Raycast(specialPoint, -transform.up, 2f, groundLayer))
        {
            isPointset = true;

        }

    }

    private void Patrol()
    {
        if (!isPointset)
        {
            Searchpoint();

        }
        if (isPointset)
        {
            agent.SetDestination(specialPoint);
        }

        Vector3 distance = transform.position - specialPoint;

        if (distance.magnitude < 1f || timer <= 0)
        {
            isPointset= false;
            timer = 5f;
        }else
        {
            timer -= Time.deltaTime;
        }
    }

    

    private void FollowPlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = 11f;
        //Debug.Log(player.position);

    }

    //Attack Functions and States
    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!attacked)
        {
            Rigidbody bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            bulletInstance.AddForce(transform.forward * 10f, ForceMode.Impulse);
            bulletInstance.AddForce(transform.up * 5f, ForceMode.Impulse);
            attacked = true;

            Invoke(nameof(Reset), attackTime);
        }
    }

    private void Reset()
    {
        attacked = false;
    }

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        inVisionRange = Physics.CheckSphere(transform.position, visionRange, playerLayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (!inVisionRange && !inAttackRange)
        {
            Patrol();
        }
        if(inVisionRange && !inAttackRange)
        {
            FollowPlayer();
        }
        if(inVisionRange && inAttackRange)
        {
            Attack();
        }
    }
}