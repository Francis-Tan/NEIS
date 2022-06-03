using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected bool not_hit = true;
    protected GameObject player;
    protected Rigidbody2D rb; //rigidbody movement better for collision
    protected BoxCollider2D bc; //for detecting collisions with player - could replace with raycast box
    public float moveSpeed;
    protected Vector3 directionToPlayer;
    protected Vector2 deltapos; //change in positon
    public float TimeBtwAttacks = 0.2f;
    protected float attackCooldown = 0.2f;

    protected Animator animator;
    private string currentState;

    protected void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    protected abstract void attack();
    public virtual void takeDamage() 
    {
        if (not_hit)
        {
            playhitanimation();
            not_hit = false;
        }
        else { Die(); }
    }

    protected abstract void playhitanimation();
    public abstract int Die();
}
