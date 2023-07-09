using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{
    public GameObject coins;
    public GameObject deathFX;
    public int coinCount;
    public LayerMask ground;
    public void Pop()
    {
        StartCoroutine("p");
    }
    IEnumerator p()
    {
        yield return null;

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