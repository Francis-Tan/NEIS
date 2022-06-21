using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVisual : MonoBehaviour
{
    private Animator animator;
    private string currentState;
    const string
        gun_idle = "gun_idle",
        gun_shoot = "gun_shoot";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    public void PlayShootAnimation()
    {
        ChangeAnimationState(gun_shoot);
    }
}
