using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo instance;
    //shouldn't be needed after carrying
    public static HealthBar healthBar;
    public static ManaBar manaBar;
    public Skill_Icon gunIcon, stunIcon;
    private void Awake() {
        if (instance != null) {
            Destroy(SceneManager.GetActiveScene().buildIndex == 1 ? instance : gameObject); 
            return;
        }
        instance = GetComponent<PlayerInfo>();
        healthBar = instance.GetComponentInChildren<HealthBar>();
        manaBar = instance.GetComponentInChildren<ManaBar>();
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
