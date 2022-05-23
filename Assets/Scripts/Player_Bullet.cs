using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 dir;
    public float moveSpeed = 20;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) { 
        GameObject runinto = other.gameObject;
        if (runinto.CompareTag("Enemy")) {
            //Instantiate(bulletexplode, transform.position, transform.rotation);
            Destroy(gameObject); 
            runinto.GetComponent<Enemy>().takeDamage();
        }
        if (runinto.CompareTag("Blocking")) { 
            //Instantiate(bulletexplode, transform.position, transform.rotation);
            Destroy(gameObject); 
        }
    }
}
