using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    Transform playerRef;
    public int health;
    public GameObject coins;
    public GameObject deathFX;
    NavMeshAgent nav;
    Animator anim;
    public LayerMask ground;
    public float dist = 2;
    public int coinCount = 1;
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    public virtual void Update()
    {
        if (playerRef && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && health > 0
        && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")
        && !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Activate"))
        {
            if (Vector3.Distance(transform.position, playerRef.position) > dist)
            {
                nav.SetDestination(Vector3.MoveTowards(playerRef.position, transform.position, dist));
                anim.ResetTrigger("Attack1");
                anim.ResetTrigger("Attack2");
                anim.ResetTrigger("Attack3");
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
            {
                if (Random.Range(0, 100) > 50)
                {
                    anim.ResetTrigger("Attack1");
                    anim.ResetTrigger("Attack2");
                    anim.ResetTrigger("Attack3");
                    anim.SetTrigger("Attack1");
                }
                else if (Random.Range(0, 100) > 50)
                {
                    anim.ResetTrigger("Attack1");
                    anim.ResetTrigger("Attack2");
                    anim.ResetTrigger("Attack3");
                    anim.SetTrigger("Attack2");
                }
                else
                {
                    anim.ResetTrigger("Attack1");
                    anim.ResetTrigger("Attack2");
                    anim.ResetTrigger("Attack3");
                    anim.SetTrigger("Attack3");
                }
            }
        }
        else if (playerRef)
        {
            nav.SetDestination(transform.position);
        }
        anim.SetFloat("Speed", nav.velocity.magnitude / nav.speed);
    }

    public void PlayerFound(Transform player)
    {
        playerRef = player;
        anim.SetTrigger("Spot");
    }

    public void Damage()
    {
        if (health <= 0)
            return;
        health--;
        if (health <= 0)
        {
            StartCoroutine("Die");
        }
        else
        {
            if (playerRef == null)
            {
                playerRef = GameObject.FindGameObjectWithTag("Player").transform;
                if (GetComponent<FOV>())
                    Destroy(GetComponent<FOV>());
            }
            anim.SetTrigger("Damage");
        }
    }

    IEnumerator Die()
    {
        yield return null;
        anim.SetTrigger("Die");
        anim.ResetTrigger("Damage");
        playerRef = null;
        nav.SetDestination(transform.position);
        yield return new WaitForSeconds(0.5f);

        do
        {
            yield return new WaitForEndOfFrame();
        } while (anim.GetCurrentAnimatorStateInfo(0).IsName("Death"));

        for (int i = 0; i < transform.childCount; i++)
        {
            // Deactivate the child game object
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (deathFX)
        {
            Quaternion rotation = transform.rotation;

            Instantiate(deathFX, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < coinCount; i++)
        {
            RaycastHit hit;
            while (Physics.Raycast(transform.position, -transform.up, out hit, 1000, ground) == false)
            {
                transform.Translate(0, 1, 0);
            }
            Instantiate(coins, hit.point, Quaternion.LookRotation(hit.normal)).transform.Rotate(new Vector3(90, 0, 0));
            yield return new WaitForEndOfFrame();
        }

        DestroyImmediate(gameObject);
    }
}