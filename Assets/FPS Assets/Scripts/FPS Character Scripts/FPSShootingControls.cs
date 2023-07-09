using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FPSShootingControls : MonoBehaviour
{

    public GameObject mainCam;

    private float nextTimeToFire = 0f;

    private GameObject wall_Impact, enemy_Impact;

    private GameObject bullet;

    FPSController controller;

    public LayerMask raycaster;

    void Start()
    {
        controller = gameObject.GetComponent<FPSController>();
        wall_Impact = Resources.Load<GameObject>("concrete_impact");
        enemy_Impact = Resources.Load<GameObject>("wood_impact");
        bullet = Resources.Load<GameObject>("Bullet");
    }

    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if ((Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && controller.quickFire)) && Time.time > nextTimeToFire)
        {
            if (Time.timeScale != 1)
            {
                nextTimeToFire = Time.time + 1f / (controller.fireRate * 20);
            }
            else
            {
                nextTimeToFire = Time.time + 1f / controller.fireRate;
            }

            Fire();
        }
    }

    void Fire()
    {
        if (controller.current_Hands_Weapon.name == "deagle")
        {
            RaycastHit hit;

            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, 1000, raycaster))
            {

                if (hit.transform.tag == "Enemy")
                {
                    if (hit.transform.gameObject.transform.root.gameObject.GetComponent<Enemy>())
                    {
                        hit.transform.gameObject.transform.root.gameObject.GetComponent<Enemy>().Damage();
                        GameObject impact = Instantiate(enemy_Impact, hit.point, Quaternion.LookRotation(hit.normal));
                        impact.transform.SetParent(hit.transform);
                        if (controller.current_Hands_Weapon.name == "m4a1")
                        {
                            hit.transform.gameObject.transform.root.gameObject.GetComponent<Enemy>().Damage();
                            impact.transform.localScale *= 2;
                        }
                    }
                    else
                    {
                        hit.transform.gameObject.transform.root.gameObject.GetComponent<Dragon>().Damage();
                        GameObject impact = Instantiate(enemy_Impact, hit.point, Quaternion.LookRotation(hit.normal));
                        impact.transform.SetParent(hit.transform);
                        if (controller.current_Hands_Weapon.name == "m4a1")
                        {
                            hit.transform.gameObject.transform.root.gameObject.GetComponent<Dragon>().Damage();
                            impact.transform.localScale *= 2;
                        }
                    }
                }
                else if (hit.transform.tag == "Pinata")
                {
                    hit.transform.gameObject.transform.root.gameObject.GetComponent<Pinata>().Pop();
                }
                else
                {
                    GameObject impact = Instantiate(wall_Impact, hit.point, Quaternion.LookRotation(hit.normal));
                    impact.transform.SetParent(hit.transform);
                    if (controller.current_Hands_Weapon.name == "m4a1")
                    {
                        impact.transform.localScale *= 2;
                    }
                }

            }
        }
        else
        {
            var rotation = controller.current_Hands_Weapon.nozzle.transform.rotation;
            Bullet newB = Instantiate(bullet, controller.current_Hands_Weapon.nozzle.position, rotation).GetComponent<Bullet>();
            newB.wall_Impact = wall_Impact;
            newB.enemy_Impact = enemy_Impact;
        }
    }
}