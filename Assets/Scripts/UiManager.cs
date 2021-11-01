using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public Text RoundText;
    public Text ScoreText;
    public Text TimeText;

    private void Update()
    {
        ScoreText.text = "Score: " + FindObjectOfType<GameManager>().score;
        TimeText.text = string.Format("Time: {0:0}:{1:00}", FindObjectOfType<GameManager>().currentMins, FindObjectOfType<GameManager>().currentSecs);
    }

}
