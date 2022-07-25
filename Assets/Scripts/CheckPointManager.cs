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
    public static void UpdateCheckpoint(int sceneindex, int hp, int mana) {
        PlayerData pd = savedData[sceneindex / 2 - 1];
        if (pd == null) {
            savedData[sceneindex / 2 - 1] = new PlayerData(hp, mana);
        } else {
            pd.hp = hp;
            pd.mana = mana;
        }
    }

    public static PlayerData GetPlayerDataAtCheckpoint(int sceneindex) {
        return savedData[sceneindex / 2 - 1];
    }
}
