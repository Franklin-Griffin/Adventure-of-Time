using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int value = 1;
    Transform player;
    Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<FPSController>().transform;
    }
    void Update() {
        rb.AddForce(Vector3.Normalize(player.position - transform.position) / 5, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<FPSController>())
        {
            string old = other.gameObject.GetComponent<FPSController>().coins.text;
            string done = (int.Parse(old) + value).ToString();
            other.gameObject.GetComponent<FPSController>().coins.text = done;
            Destroy(gameObject);
        }
    }
}
