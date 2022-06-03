using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 dir;
    public float moveSpeed = 20;

    private Animator animator;
    private string currentState;
    const string
        PB_move = "PB_move",
        PB_explode = "PB_explode";

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dir = moveSpeed * (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void FixedUpdate() 
    {
        rb.MovePosition(rb.position + Time.fixedDeltaTime * dir);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    { 
        GameObject runinto = other.gameObject;
        if (runinto.CompareTag("Enemy")) 
        {
            //ChangeAnimationState(PB_explode);
            runinto.GetComponent<Enemy>().takeDamage();
            Destroy(gameObject); 
        }
        else if (runinto.CompareTag("Blocking")) 
        {
            //ChangeAnimationState(PB_explode);
            Destroy(gameObject); 
        }
    }
}