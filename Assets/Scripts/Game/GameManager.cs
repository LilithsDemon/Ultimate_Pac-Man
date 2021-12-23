using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public List<Vector2> playerStartingPos = new List<Vector2>() {new Vector2(0f, -8.5f), new Vector2(0f, 2.5f), new Vector2(-10f, 13.5f), new Vector2(10f, 13.5f)};

    public GameObject pacmanPrefab;
    public GameObject ghostPrefab;

    public GameObject pelletPrefab;
    public Transform pellets;

    public Animator roundAnimator;
    public GameObject pacmanUI;
    public GameObject ghostUI;

    public GameObject mainUI;
    public GameObject pauseTimeText;
    public Text endRoundScoreText;

    private GameObject player;

    public int ghostMultiplier {get; private set;} = 1;

    public int score {get; private set;}
    public int roundScore {get; private set;}

    public float totalTime = 120f;
    private float currentTime;
    public int currentMins {get; private set;} = 0;
    public int currentSecs {get; private set;} = 0;

    public float totalPauseTime = 14f;
    public float currentPauseTime;

    public bool paused = true;

    public int round {get; private set;} = 1;

    private PhotonView pV;

    private byte[] freeValues = new byte[] {0, 1, 2, 3};
    private Color[] colors = new Color[] {new Color(1f, 1f, 0, 1f),
                                          new Color(0.996649916f, 0f, 0f, 1f),
                                          new Color (0.870588235f, 0.631372549f, 0.521568627f, 1f)};

    private void Awake()
    {
        pV = this.GetComponent<PhotonView>();
        NewGame();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //Runs the selection of player value on all players game
            pV.RPC("GiveValues", RpcTarget.AllBuffered);
        }
        //Sets values for the start of the game
    }
    
    [PunRPC]
    void updateList(byte[] newList)
    {
        freeValues = newList;
    }

    [PunRPC]
    void GiveValues()
    {
        //Choses a random value from the available player values list
        int value = UnityEngine.Random.Range(0, freeValues.Length);
        byte charValue = freeValues[value];
        //Removes that value from players lists
        byte[] tmpArray = new byte[freeValues.Length-1];
        int count = 0;
        for (int i = 0; i < freeValues.Length; i++)
        {
            if (i != value)
            {
                tmpArray[count] = (freeValues[i]);
                count++;
            }
        }

        pV.RPC("updateList", RpcTarget.All, tmpArray);
        //Sets value and creates player characters
        FindObjectOfType<PlayerManager>().SetChar(charValue);
        InstantiateChars();
    }

    [PunRPC]
    void UpdateTime(float newTime, float newPauseTime)
    {
        this.currentTime = newTime;
        if (newPauseTime > 0)
        {
            this.currentPauseTime = newPauseTime;
        }else
        {
            paused = false;
            this.currentPauseTime = totalPauseTime;
        }
        this.currentMins = Mathf.FloorToInt(newTime / 60);
        this.currentSecs = Mathf.FloorToInt(newTime % 60);
    }

    void TakeAwayTime()
    {
        if (this.currentTime > 0 && this.paused == false)
        {
            this.currentTime = this.currentTime - Time.deltaTime;
        }else if (this.paused == true)
        {
            this.currentPauseTime = this.currentPauseTime - Time.deltaTime;
        } 
        else
        {
            this.currentTime = totalTime;
            pV.RPC("NewRound", RpcTarget.All);
        }
        pV.RPC("UpdateTime", RpcTarget.All, this.currentTime, this.currentPauseTime);
        this.currentMins = Mathf.FloorToInt(this.currentTime / 60);
        this.currentSecs = Mathf.FloorToInt(this.currentTime % 60);
    }

    private void ChangeChar()
    {
        PhotonNetwork.Destroy(player);
        InstantiateChars();
    }

    public void InstantiateChars()
    {
        int tmpCharVal = FindObjectOfType<PlayerManager>().charVal;
        if (tmpCharVal == 0)
        {
            player = PhotonNetwork.Instantiate(pacmanPrefab.name, playerStartingPos[tmpCharVal], Quaternion.identity);
        }
        else
        {
            ghostPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().color = colors[tmpCharVal - 1];
            player = PhotonNetwork.Instantiate(ghostPrefab.name, playerStartingPos[tmpCharVal], Quaternion.identity);
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            TakeAwayTime();
        }
        if (paused == true)
        {
            pauseTimeText.SetActive(true);
            pauseTimeText.GetComponent<Text>().text = Math.Floor(this.currentPauseTime).ToString();
        } else
        {
            pauseTimeText.SetActive(false);
        }
    }

    private void NewGame()
    {
        paused = true;
        score = 0;
        roundScore = 0;
        StartPellets();
        ResetGhostMultiplier();
        this.round = 1;
        currentTime = totalTime;
        currentPauseTime = 5f;
    }

    [PunRPC]
    private void NewRound()
    {
        paused = true;
        mainUI.SetActive(false);
        endRoundScoreText.text = "SCORE THIS ROUND: " + this.roundScore.ToString();
        Invoke(nameof(ActivateMainUI), 9f);
        if (this.round == 4)
        {
            Invoke(nameof(LeaveGame), 8f);
        }
        roundAnimator.SetTrigger("EndRound");
        ActivateUIChar();
        Invoke(nameof(ResetState), 3.4f);
        Invoke(nameof(ActivateUIChar), 3.5f);
        FindObjectOfType<PlayerManager>().IncrementChar();
        ChangeChar();
        this.round = this.round + 1;
        this.roundScore = 0;
    }

    public void ActivateMainUI()
    {
        mainUI.SetActive(true);
    }

    public void ActivateUIChar()
    {
        if (FindObjectOfType<PlayerManager>().charVal == 0)
        {
            pacmanUI.SetActive(true);
            ghostUI.SetActive(false);
        }
        else
        {
            pacmanUI.SetActive(false);
            ghostUI.SetActive(true);
        }
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        FindObjectOfType<SaveManager>().AddCoins(Mathf.FloorToInt(this.score/100));
        SceneManager.LoadScene(0);
    }

    public void PacmanDeath()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            pV.RPC("NewRound", RpcTarget.All);
        }
    }

    private void ResetState()
    {
        ChangeChar();
        ResetGhostMultiplier();
        RefreshPellets();
        this.currentTime = this.totalTime;
    }

    private void AddToScore(int points)
    {
        this.score = this.score + points;
        this.roundScore = this.roundScore + points;
    }

    public void GhostEaten(Ghost ghost)
    {
        AddToScore(ghost.points * ghostMultiplier);
        this.ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        if (pellet.gameObject.transform.localScale.x > 0.9f && paused == false)
        {
            pellet.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            if (FindObjectOfType<PlayerManager>().charVal == 0)
            {
                AddToScore(pellet.points);
            }

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
            if(pellet.GetChild(0).gameObject.transform.localScale.x > 0.9f)
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