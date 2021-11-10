using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;

    public charMovementController pacman;
    public  Vector2 pacmanStartingPos = new Vector2(0.5f, -8.5f);

    public Transform pellets;

    public int ghostMultiplier {get; private set;} = 1;

    public int score {get; private set;}
    public int lives {get; private set;}

    public float totalTime = 120f;
    private float currentTime;
    public int currentMins {get; private set;} = 0;
    public int currentSecs {get; private set;} = 0;

    private int round = 1;

    private void Start()
    {
        NewGame();
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
        FindObjectOfType<UiManager>().RoundText.text = "Round: " + this.round;
    }

    private void NewRound()
    {
        ResetState();
        RefreshPellets();
        this.round = this.round + 1;
        FindObjectOfType<UiManager>().RoundText.text = "Round: " + this.round;
    }

    private void PacmanDeathReset()
    {
        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(true);
        }

        this.pacman.gameObject.transform.position = pacmanStartingPos;
        this.pacman.movement.SetDirection(Vector2.right);

        this.pacman.gameObject.SetActive(true);
        ResetGhostMultiplier();
        currentTime = totalTime;
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
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
        this.pacman.gameObject.SetActive(false);
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
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            Invoke(nameof(RefreshPellets), 1.5f);
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
            if(pellet.gameObject.activeSelf)
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
            pellet.gameObject.SetActive(true);
        }
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
