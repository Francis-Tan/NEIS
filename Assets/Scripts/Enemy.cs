using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected bool not_hit = true;
    public GameObject player;
    protected Rigidbody2D rb; //rigidbody movement better for collision
    protected BoxCollider2D bc; //to detect whether it will collide with player - might replace with raycast box
    public float moveSpeed;
    protected Vector3 directionToPlayer;
    protected Vector2 deltapos; //the change in position
    public float TimeBtwAttacks = 0.2f;
    protected float attackCooldown = 0.2f;
    protected Animator animator;
    private string currentState;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    protected void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    protected abstract void attack();
    public virtual void takeDamage() {   
        if (not_hit) {  
            not_hit = false;
            //instantiate visual effect
        } else Die();
    }
    public abstract int Die();
}
