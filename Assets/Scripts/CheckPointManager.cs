using UnityEngine;
using UnityEngine.SceneManagement; //for SceneManager
public class CheckPointManager : MonoBehaviour {
    public class PlayerData {
        public int hp, mana;

        public PlayerData (int hp, int mana) {
            this.hp = hp;
            this.mana = mana;
        }
    }

    private static PlayerData[] savedData = new PlayerData[(SceneManager.sceneCountInBuildSettings - 2)/2];

    //sceneindex refers to the scene index in build settings
    public static void UpdateCheckpoint(int floornumber, int hp, int mana) {
        PlayerData pd = savedData[(floornumber - 1)/2];
        if (pd == null) {
            savedData[(floornumber - 1) / 2] = new PlayerData(hp, mana);
        } else {
            pd.hp = hp;
            pd.mana = mana;
        }
    }

    public static PlayerData GetPlayerDataAtFloor(int floornumber) {
        return savedData[(floornumber - 1) / 2];
    }
}
