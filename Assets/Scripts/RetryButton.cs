using UnityEngine;
using UnityEngine.UI; //for Button class

public class RetryButton : MonoBehaviour
{
    private int savedHP, savedMana;
    private Button button;


    /**
    -on checkpoint enter, checkpoint sends info to button
    -button activates
    -allow checkpoint to update info for an already active button

    or

    use a buttonmanager which checkpoints can call to update their respective checkpoint
    */
    public class CheckPointButton {
        public Button button;
        public int savedHP, savedMana;
        public void update() {
        
        }
    }
}
