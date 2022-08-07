using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public Skill_Icon gunIcon, stunIcon;
    private CanvasGroup canvasGroup;
    private int num = 0;
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject); 
            return;
        }
        instance = GetComponent<PlayerInfo>();
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(instance);
        gameObject.SetActive(false);
        //this is on UI layer which is set to only detect default, enemy and player colliders
        if (GetComponent<BoxCollider2D>().IsTouchingLayers()) {
            ++num;
            FadeOut();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.GetComponent<Enemy>() != null || collider.GetComponent<Player>() != null) {
            if (num++ == 0) FadeOut();
        }
    }

    private void FadeOut() {
        canvasGroup.alpha = 0.3f;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null || collision.GetComponent<Player>() != null) {
            if (--num == 0) FadeIn();
        }
    }

    private void FadeIn() {
        canvasGroup.alpha = 1;
    }

    public static void SetVisibleAll(bool visible) {
        instance.gameObject.SetActive(visible);
    }

    public static void ShowBarsOnly() {
        SetVisibleAll(true);
        instance.gunIcon.hide();
        instance.stunIcon.hide();
    }
}
