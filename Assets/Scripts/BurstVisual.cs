using UnityEngine;

public class BurstVisual : MonoBehaviour {
    private Animator animator;
    private string currentState;
    const string
        stun_notenoughmana = "stun_notenoughmana",
        stun_enoughmana = "stun_enoughmana",
        stun_attack = "stun_attack";
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void ChangeAnimationState(string newState) {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
    public void updateSprite(int mana) {
        ChangeAnimationState(mana < 5 ? stun_notenoughmana : stun_enoughmana);
    }

    public void PlayAttackAnimation() {
        ChangeAnimationState(stun_attack);
    }

    public void StopAttackAnimation() {
        updateSprite(Player.GetInstance().GetComponent<Player>().currentmana);
    }
}
