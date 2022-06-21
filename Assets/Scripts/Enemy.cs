using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Enemy : MonoBehaviour
{
    public int mana;
    protected bool not_hit = true;
    protected bool stunned = false;
    public GameObject hiticon;
    public GameObject stunicon;
    protected GameObject player;
    protected Rigidbody2D rb; //rigidbody movement better for collision
    protected BoxCollider2D bc; //for detecting collisions with player - could replace with raycast box
    public float moveSpeed;
    protected Vector3 directionToPlayer;
    protected Vector2 deltapos; //change in positon
    public float TimeBtwAttacks = 0.2f;
    protected float attackCooldown = 0.2f;
    protected float stunduration = 2f;
    protected float stunscalemax;

    protected Animator animator;
    private string currentState;

    private void Awake()
    {
        //you can set the alphas to zero in inspector then get rid of these for later optimisation
        Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        hiticon.GetComponent<SpriteRenderer>().material.color = c;
        c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        stunscalemax = stunicon.GetComponent<Transform>().localScale.x;
    }

    private void FixedUpdate()
    {
        if (stunned)
        {
            stunned_behaviour();
        }
        else
        {
            unstunned_behaviour();
        }
    }
    private void stunned_behaviour()
    {
        Transform stuntransform = stunicon.GetComponent<Transform>();
        if (stunduration > 0)
        {
            stunduration -= Time.deltaTime;
            var newscale = stuntransform.localScale;
            newscale.x = stunscalemax * stunduration / 1.75f;
            stuntransform.localScale = newscale;
        }
        else
        {
            stunduration = 1.75f;
            Color c = stunicon.GetComponent<SpriteRenderer>().material.color;
            c.a = 0;
            stunicon.GetComponent<SpriteRenderer>().material.color = c;
            var newscale = stuntransform.localScale;
            newscale.x = stunscalemax;
            stuntransform.localScale = newscale;
            stunned = false;
        }
    }
    protected abstract void unstunned_behaviour();
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
    public void becomestunned()
    {
        Color c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 1;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        stunned = true;
    }
    public abstract void Die();
}
