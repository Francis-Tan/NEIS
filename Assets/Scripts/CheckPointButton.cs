using UnityEngine;
using UnityEngine.UI; //for Button class
using UnityEngine.SceneManagement; //for SceneManager
using TMPro; //for TextMeshProUGUI
using UnityEngine.EventSystems;

public class CheckPointButton : MonoBehaviour, IPointerDownHandler {
    public int FloorNumber;
    private int CheckPointIndex;
    private CheckPointManager.PlayerData playerData;

    private void Awake() {
        TextMeshProUGUI txtMP = GetComponent<TextMeshProUGUI>();
        CheckPointIndex = FloorNumber + 1;
        playerData = CheckPointManager.GetPlayerDataAtCheckpoint(CheckPointIndex);
        if (playerData == null) {
            GetComponent<Button>().interactable = false;
            txtMP.color = new Color(160, 160, 160);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (playerData != null) {
            SceneManager.LoadScene(CheckPointIndex);
            Player.GetInstance().GetComponent<Player>().spawn(new Vector2(-10.65f, 6.73f), 
                    playerData.hp, playerData.mana);
            PlayerInfoCanvas.instance.GetComponent<Canvas>().enabled = true;
        }
    }
}
