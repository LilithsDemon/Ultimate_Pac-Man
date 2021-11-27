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
        scoreText.text = "score: " + FindObjectOfType<GameManager>().score;
        timeText.text = string.Format("time: {0:0}:{1:00}", FindObjectOfType<GameManager>().currentMins, FindObjectOfType<GameManager>().currentSecs);
        playerValueText.text = ("player value: " + Convert.ToString(FindObjectOfType<PlayerManager>().charVal));
    }

    private void Start()
    {
        playerValueText.text = ("player value: " + Convert.ToString(FindObjectOfType<PlayerManager>().charVal));
    }

}
