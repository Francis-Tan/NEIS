using UnityEngine;
using UnityEngine.UI; //for Button class
using TMPro; //for TextMeshProUGUI

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
        SceneMethods.LoadCheckPointLevel(FloorNumber, playerData);
    }
}
