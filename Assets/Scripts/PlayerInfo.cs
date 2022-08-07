using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;
    public HealthBar healthBar;
    public ManaBar manaBar;
    public Skill_Icon gunIcon, stunIcon;
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject); 
            return;
        }
        instance = GetComponent<PlayerInfo>();
        DontDestroyOnLoad(instance);
        gameObject.SetActive(false);
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
