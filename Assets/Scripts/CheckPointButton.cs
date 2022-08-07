using UnityEngine;
using UnityEngine.UI; //for Button class
using UnityEngine.SceneManagement; //for SceneManager
using TMPro; //for TextMeshProUGUI
using System.Collections;

public class CheckPointButton : MonoBehaviour {
    public int FloorNumber;
    private CheckPointManager.PlayerData playerData;

    private void Awake() {
        TextMeshProUGUI txtMP = GetComponent<TextMeshProUGUI>();
        playerData = CheckPointManager.GetPlayerDataAtFloor(FloorNumber);
        if (playerData == null) {
            GetComponent<Button>().interactable = false;
            txtMP.color = new Color(160, 160, 160);
        }
    }

    public void LoadCheckPointLevel() {
        Player.EnableStunSR(false);
        AudioManager.instance.PlayBGM(Sound.BGM_MainLevels);
        Player.GetInstance().Spawn(SceneMethods.MainLevelPos,
            playerData.hp, playerData.mana, 2);
        StartCoroutine(LoadNextLevel());
        IEnumerator LoadNextLevel() {
            yield return new WaitForSeconds(0f);
            SceneManager.LoadSceneAsync(FloorNumber + 1);
            Player.EnableStunSR(true); //must be under waitforseconds to be hidden
            PlayerInfo.SetVisibleAll(true);
        }
    }
}
