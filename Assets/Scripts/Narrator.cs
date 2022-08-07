using TMPro;
using UnityEngine;
using System.Collections;

public class Narrator : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public TextMeshProUGUI backButton, forwardButton;
    public string[] Dialouge;
    private int maxDialougeReached = 0;
    public Spawner[] spawners;
    private int currSpawner = 0;
    public StatusTile HPRefiller, manaRefiller;
    public Checkpoint checkpoint;
    public Skill_Icon gunIcon, stunIcon;
    private int currDialouge = 0;
    private Player player;

    private void Start() {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = Dialouge[0];
        player = Player.GetInstance();
        ++LoadLevel.instance.enemycount;
        backButton.enabled = false;
        gunIcon = PlayerInfo.instance.gunIcon;
        stunIcon = PlayerInfo.instance.stunIcon;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha3) && currDialouge < Dialouge.Length - 1) {
            tmp.text = Dialouge[++currDialouge];

            if (currDialouge == Dialouge.Length - 1) forwardButton.enabled = false;
            else if (currDialouge == 1) backButton.enabled = true;
            
            if (currDialouge > maxDialougeReached) {
                maxDialougeReached = currDialouge;
                switch (currDialouge) {
                    case 2:
                        //spawn gunner
                        spawnSpawner();
                        break;
                    case 4:
                        HPRefiller.enable();
                        manaRefiller.enable();
                        break;
                    case 5:
                        gunIcon.show();
                        //player.gunIcon = gunIcon;
                        ++player.skillLevel;
                        break;
                    case 6:
                        //spawn 3 gunners
                        spawnSpawner();
                        break;
                    case 7:
                        //spawn 3 assassins
                        spawnSpawner();
                        break;
                    case 8:
                        stunIcon.show();
                        //player.stunIcon = stunIcon;
                        ++player.skillLevel;
                        break;
                    case 10:
                        //spawn gunners and assassins
                        spawnSpawner();
                        break;
                    case 11:
                        //spawn 3 drones
                        spawnSpawner();
                        break;
                    case 12:
                        spawnCheckpoint();
                        break;
                    case 13:
                        Destroy(HPRefiller.gameObject);
                        Destroy(manaRefiller.gameObject);
                        Destroy(checkpoint.gameObject);
                        break;
                    case 15:
                        //allow player to go to floor 1
                        if (--LoadLevel.instance.enemycount == 0) LoadLevel.instance.enable();
                        break;
                };
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && currDialouge > 0) {
            tmp.text = Dialouge[--currDialouge];



            if (currDialouge == Dialouge.Length - 2) forwardButton.enabled = true;
            else if (currDialouge == 0) backButton.enabled = false;
        }
    }

    private void spawnSpawner() {
        Spawner s = spawners[currSpawner++];
        float waitTime = 0;
        if (Physics2D.OverlapBox(s.transform.position, new Vector2(1.25f, 1.25f), 8) != null) {
            player.transform.position = Vector2.zero;
            waitTime = 0.25f;
        }
        StartCoroutine(Enable());
        IEnumerator Enable() {
            yield return new WaitForSeconds(waitTime);
            s.GetComponent<Collider2D>().enabled = true;
            s.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void spawnCheckpoint() {
        float waitTime = 0;
        if (Physics2D.OverlapBox(checkpoint.transform.position, new Vector2(1.25f, 1.25f), 8) != null) {
            player.transform.position = Vector2.zero;
            waitTime = 0.25f;
        }
        StartCoroutine(Enable());
        IEnumerator Enable() {
            yield return new WaitForSeconds(waitTime);
            checkpoint.gameObject.SetActive(true);
        }
    }
}
