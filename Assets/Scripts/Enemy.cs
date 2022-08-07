using System;
using UnityEngine;
public abstract class Enemy : MonoBehaviour {
    private int mana = 2;
    protected bool not_hit = true;
    protected bool stunned = false;
    public GameObject hiticon;
    public GameObject stunicon;
    protected Player player;
    protected Rigidbody2D rb; //rigidbody movement better for collision
    protected BoxCollider2D bc; //for detecting collisions with
                                //- could replace with raycast box
    protected SpriteRenderer sr;
    public float moveSpeed;
    protected Vector3 directionToPlayer;
    protected Vector2 deltapos; //change in positon
    public float TimeBtwAttacks = 0.2f;
    protected float attackCooldown = 0.2f;
    protected float stunduration = 2f;
    protected float stunscalemax; //refers to original scale of the stunbar
    public event EventHandler OnEnemyDeath;

    protected Animator animator;
    protected string currentState;

    private void Start() {
        enabled = false;
        gameObject.layer = 9; //can use 7 or 9
        player = Player.GetInstance();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        //you can set the alphas to zero in inspector then get rid of these for later optimisation

        Color c = sr.material.color;
        c.a = 0;
        sr.material.color = c;

        c = hiticon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        hiticon.GetComponent<SpriteRenderer>().material.color = c;

        c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 0;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        stunscalemax = stunicon.GetComponent<Transform>().localScale.x;
    }

    public abstract void Spawn();

    private void FixedUpdate() {
        if (stunned) {
            rb.velocity = Vector2.zero;
            stunned_behaviour();
        }
        else {
            unstunned_behaviour();
        }
    }
    private void stunned_behaviour() {
        Transform stuntransform = stunicon.GetComponent<Transform>();
        if (stunduration > 0) {
            stunduration -= Time.deltaTime;
            var newscale = stuntransform.localScale;
            newscale.x = stunscalemax * stunduration / 1.75f;
            stuntransform.localScale = newscale;
        }
        else {
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
    protected void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    protected abstract void attack();
    public virtual void takeDamage() {
        if (not_hit) {
            not_hit = false;
            Color c = hiticon.GetComponent<SpriteRenderer>().material.color;
            c.a = 1;
            hiticon.GetComponent<SpriteRenderer>().material.color = c;
        } else { Death(); }
    }
    public int getMana() {
        return mana;
    }
    public void getStunned() {
        Color c = stunicon.GetComponent<SpriteRenderer>().material.color;
        c.a = 1;
        stunicon.GetComponent<SpriteRenderer>().material.color = c;
        stunned = true;
    }

    public void Death() {
        bc.enabled = false;
        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        Die();
    }
    public abstract void Die();
}
