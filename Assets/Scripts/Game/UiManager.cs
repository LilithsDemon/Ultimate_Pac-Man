using System;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public Text roundText;
    public Text scoreText;
    public Text timeText;
    public Text playerValueText;

    private void Update()
    {
        scoreText.text = "Score: " + FindObjectOfType<GameManager>().score;
        timeText.text = string.Format("Time: {0:0}:{1:00}", FindObjectOfType<GameManager>().currentMins, FindObjectOfType<GameManager>().currentSecs);
        playerValueText.text = ("Player Value: " + Convert.ToString(FindObjectOfType<PlayerManager>().charVal));
    }

    private void Start()
    {
        playerValueText.text = ("Player Value: " + Convert.ToString(FindObjectOfType<PlayerManager>().charVal));
    }

}
