using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner_Bullet : MonoBehaviour
{
    private Rigidbody2D rb; //rigidbody movement is to avoid moving in a rotated transform after rotation
    private Vector2 dir;
    public float moveSpeed = 16;
    public int dmg = 1;
    public bool destroy = false;

    private Animator animator;
    private string currentState;
    const string
        EB_move = "EB_move",
        EB_explode = "EB_explode";

    private void Awake() 
    {
        dir = moveSpeed * (Player.GetInstance().transform.position - transform.position).normalized;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void reset(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        dir = moveSpeed * (Player.GetInstance().transform.position - transform.position).normalized;
    }

    private void FixedUpdate() 
    {
        rb.MovePosition(rb.position + dir * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    { 
        GameObject runinto = other.gameObject;
        if (runinto.CompareTag("Player")) 
        {
            //ChangeAnimationState(EB_explode);
            runinto.GetComponent<Player>().takeDamage(dmg);
            if (destroy) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
        else if (runinto.CompareTag("Blocking")) 
        {
            //ChangeAnimationState(EB_explode);
            if (destroy) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
    }
}
