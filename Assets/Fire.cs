using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FireBullet;
    public Transform tf;
    bool running = false;
    public void start()
    {
        if (!running)
        {
            StartCoroutine("f");
            running = true;
        }
    }
    public void stop()
    {
        StopCoroutine("f");
        running = false;
    }
    IEnumerator f()
    {
        for (float i = 0; i < 5; i += 1f)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject x = Instantiate(FireBullet, tf);
            x.transform.parent = null;
        }
        running = false;
    }
}
