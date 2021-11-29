using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;

    public Vector2 pacmanStartingPos = new Vector2(0.5f, -8.5f);

    public GameObject playerPrefab;
    public GameObject ghostPrefab;

    public GameObject pelletPrefab;

    public Transform pellets;

    public int ghostMultiplier {get; private set;} = 1;

    public int score {get; private set;}
    public int lives {get; private set;}

    public float totalTime = 120f;
    private float currentTime;
    public int currentMins {get; private set;} = 0;
    public int currentSecs {get; private set;} = 0;

    private int round = 1;

    private List<byte> freeValues = new List<byte> {0, 1, 2, 3};

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView pV = this.GetComponent<PhotonView>();
            pV.RPC("GiveValues", RpcTarget.AllBuffered);
        }
        NewGame();
    }

    [PunRPC]
    void GiveValues()
    {
        int value = UnityEngine.Random.Range(0, freeValues.Count);
        byte charValue = freeValues[value];
        freeValues.RemoveAt(value);
        FindObjectOfType<PlayerManager>().SetChar(charValue);
        if (FindObjectOfType<PlayerManager>().charVal == 0)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, pacmanStartingPos, Quaternion.identity);
        }else
        {
            GameObject player = PhotonNetwork.Instantiate(ghostPrefab.name, pacmanStartingPos, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }

        if (this.currentTime > 0)
        {
            this.currentTime = this.currentTime - Time.deltaTime;

            this.currentMins = Mathf.FloorToInt(this.currentTime / 60);
            this.currentSecs = Mathf.FloorToInt(this.currentTime % 60);
        } else
        {
            this.currentTime = 0;
            NewRound();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(1);
        ResetState();
        FindObjectOfType<UiManager>().roundText.text = "Round: " + this.round;
    }

    private void NewRound()
    {
        ResetState();
        RefreshPellets();
        this.round = this.round + 1;
        FindObjectOfType<UiManager>().roundText.text = "Round: " + this.round;
    }

    private void PacmanDeathReset()
    {
        ResetState();
    }

    private void ResetState()
    {
        StartPellets();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(true);
        }


        ResetGhostMultiplier();
        currentTime = totalTime;
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        SetScore(this.score + (ghost.points * ghostMultiplier));
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        SetLives(this.lives -1);
        if (this.lives > 0)
        {
            Invoke(nameof(PacmanDeathReset), 3.0f); //Waits 3 seconds to do this function
        }
        else {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        if (pellet.gameObject.transform.localScale.x > 0.02f)
        {
            pellet.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            SetScore(this.score + pellet.points);

            if (!HasRemainingPellets())
            {
                Invoke(nameof(RefreshPellets), 1.5f);
            }
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        PelletEaten(pellet);
        //In case one is already on
        CancelInvoke();
        //Eats a powerPellet
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach(Transform pellet in this.pellets)
        {
            if(pellet.GetChild(0).gameObject.transform.localScale.x > 0.02f)
            {
                //If a single pellet is found
                return true;
            }
        }
        //If no active pellets foudn
        return false;
    }

    private void RefreshPellets()
    {
        foreach(Transform pellet in this.pellets)
        {
            //Sets all pellets to active
            pellet.GetChild(0).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void StartPellets()
    {
        foreach(Transform pellet in this.pellets)
        {
            GameObject actualPellet = PhotonNetwork.Instantiate(pelletPrefab.name, pellet.position, Quaternion.identity, 0);
            actualPellet.transform.parent = pellet;
        }
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}