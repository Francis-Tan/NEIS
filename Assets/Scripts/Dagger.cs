using UnityEngine;

public class Dagger : MonoBehaviour {
    private Animator animator;
    private string currentState;
    const string
        Dagger_idle = "Dagger_idle",
        Dagger_hit = "Dagger_hit";

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    public void PlayHitAnimation() {
        ChangeAnimationState(Dagger_hit);
    }
}