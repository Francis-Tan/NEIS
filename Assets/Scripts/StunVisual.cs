using UnityEngine;

public class StunVisual : MonoBehaviour {
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
        string newState = mana < 5 ? stun_notenoughmana : stun_enoughmana;
        animator.Play(newState);
        currentState = newState;
        //when a deactivated object activates, its animator will go back to its default state (notenoughmana)
        //for player spawn(), in the case where your saved mana and death mana are >= 5,
        //the state check stops the default state from being corrected
    }

    public void PlayAttackAnimation() {
        ChangeAnimationState(stun_attack);
    }

    public void StopAttackAnimation() {
        updateSprite(Player.GetInstance().GetComponent<Player>().currentmana);
    }
}
