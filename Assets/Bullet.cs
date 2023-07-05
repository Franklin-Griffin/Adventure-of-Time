using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    FPSController controller;
    public GameObject enemy_Impact, wall_Impact;
    Rigidbody rb;
    public int force = 30;
    void Start()
    {
        controller = FindObjectOfType<FPSController>();
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(new Vector3(0, 30, 0), ForceMode.Impulse);
    }
    void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            transform.rotation *= Quaternion.Euler(90, 0, 0);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.tag == "Enemy")
        {
            if (other.gameObject.transform.root.gameObject.GetComponent<Enemy>())
            {
                other.gameObject.transform.root.gameObject.GetComponent<Enemy>().Damage();
                GameObject impact = Instantiate(enemy_Impact, other.GetContact(0).point, Quaternion.LookRotation(other.GetContact(0).normal));
                impact.transform.SetParent(other.transform);
                if (controller.current_Hands_Weapon.name == "m4a1")
                {
                    impact.transform.localScale *= 2;
                    Collider[] areaDmg = Physics.OverlapSphere(transform.position, 10000);
                    foreach (Collider col in areaDmg) {
                        if (col.tag == "Enemy") {
                            Transform c = col.transform.root;
                            if (c.GetComponent<Enemy>()) 
                            {
                                c.GetComponent<Enemy>().Damage();
                            }
                            else if (c.GetComponent<Dragon>())
                            {
                                c.GetComponent<Dragon>().Damage();
                            }
                        }
                    }
                }
            }
            else
            {
                other.gameObject.transform.root.gameObject.GetComponent<Dragon>().Damage();
                GameObject impact = Instantiate(enemy_Impact, other.GetContact(0).point, Quaternion.LookRotation(other.GetContact(0).normal));
                impact.transform.SetParent(other.transform);
                if (controller.current_Hands_Weapon.name == "m4a1")
                {
                    impact.transform.localScale *= 2;
                    Collider[] areaDmg = Physics.OverlapSphere(transform.position, 10);
                    foreach (Collider col in areaDmg) {
                        if (col.tag == "Enemy") {
                            Transform c = col.transform.root;
                            if (c.GetComponent<Enemy>()) 
                            {
                                c.GetComponent<Enemy>().Damage();
                            }
                            else if (c.GetComponent<Dragon>())
                            {
                                c.GetComponent<Dragon>().Damage();
                            }
                        }
                    }
                }
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.transform.tag == "Shield")
        {
            Destroy(gameObject);
        }
        else
        {
            GameObject impact = Instantiate(wall_Impact, other.GetContact(0).point, Quaternion.LookRotation(other.GetContact(0).normal));
            impact.transform.SetParent(other.transform);
            if (controller.current_Hands_Weapon.name == "m4a1")
            {
                impact.transform.localScale *= 2;
            }
            Destroy(gameObject);
        }
    }
}
