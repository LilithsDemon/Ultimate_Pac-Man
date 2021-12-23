using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{

    public Text loadingText;
    public float animationTime = 0.5f;
    private float currentTiming = 0;
    public int dots = 0;
    public List<string> dotString = new List<string>() {};

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = FindObjectOfType<SaveManager>().Name();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene(2);
    }

    public void Update()
    {
        currentTiming = currentTiming + Time.deltaTime;
        if (currentTiming > animationTime)
        {
            if (this.dots < 3)
            {
                this.dots = this.dots + 1;
            }
            else
            {
                this.dots = 0;
            }
            this.dotString = new List<string>() {};
            for (int i = 0; i < dots; i++)
            {
                this.dotString.Add(".");
            }
            this.loadingText.text = "Loading\n" + string.Join("", dotString);
            currentTiming = 0;
        }
    }
}