using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int mana;
    protected bool not_hit = true;
    public GameObject hiticon;
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

    private void Awake()
    {
        Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        hiticon.GetComponent<SpriteRenderer>().material.color = c;
    }
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
            Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
            c.a = 1;
            hiticon.GetComponent<SpriteRenderer>().material.color = c;
        }
        else { Die(); }
    }

    protected abstract void playhitanimation();
    public int getmana()
    {
        return mana;
    }
    public abstract void Die();
}
