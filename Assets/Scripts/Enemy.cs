using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected int health = 2;
    public GameObject player;
    protected Rigidbody2D rb; //rigidbody movement better for collision
    protected BoxCollider2D bc; //to detect whether it will collide with player - might replace with raycast box
    protected Vector3 directionToPlayer;
    protected Vector2 deltapos; //the change in position
    public float TimeBtwAttacks = 0.2f;
    protected float attackCooldown = 0.2f;
    protected abstract void attack();
    public abstract int Die();
}
