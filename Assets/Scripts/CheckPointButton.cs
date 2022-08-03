using UnityEngine;
using UnityEngine.UI; //for Button class
using UnityEngine.SceneManagement; //for SceneManager
using TMPro; //for TextMeshProUGUI
using UnityEngine.EventSystems;

public class CheckPointButton : MonoBehaviour, IPointerDownHandler {
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

    public void OnPointerDown(PointerEventData eventData) {
        if (playerData != null) {
            AudioManager.instance.PlayBGM(Sound.BGM_MainLevels);
            SceneManager.LoadScene(FloorNumber + 1);
            Player.GetInstance().GetComponent<Player>()
                .Spawn(new Vector2(-10.65f, 6.73f), playerData.hp, playerData.mana, 2);
            PlayerInfo.SetVisibleAll(true);
        }
    }
}
