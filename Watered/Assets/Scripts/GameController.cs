using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    // The players Script
    public PlayerController player;
    Transform playerTransform;

    public Image lifeImage;
    public GameObject gameOverPanel;

    public AudioClip gameOverMusic, coinSoundEffect, gameOverClip;
    public AudioSource musicSource;
    public AudioSource audioFX;


    // Score parameters
    public float timer;
    public GameObject[] secrets, waterBanks, leaks;
    public int coinsCollected, waterBanksTouched, goldTime, silverTime, bronzeTime, score;


    // Check of the state of the game
    public bool isGameStarted;
    public Text timerText, finalTimeText, coinsCollectedText, waterbanksPickedText, finalScoreText;

    // Contains the secret walls
    public GameObject superForeground;

    //Camera controller
    public CameraFollow cameraController;

    // To handle the zoom size from unity
    public float zoomInSize;
    public float zoomOutSize;
    float zoomSize;
    public bool isZooming;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        playerTransform = player.transform;
        timer = 0;
        score = 0;
        cameraController.Setup(() => playerTransform.position);
        isGameStarted = true;
        secrets = GameObject.FindGameObjectsWithTag("Coin");
        waterBanks = GameObject.FindGameObjectsWithTag("Waterbank");
        leaks = GameObject.FindGameObjectsWithTag("LeakSpawners");
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStarted)
        {
            UpdateChrono();

            if (player.health <= 0)
            {
                audioFX.clip = player.playerSounds[1];
                audioFX.Play();
                MakeGameOver();
            }
        }

        if (isZooming)
        {
            cameraController.HandleZoom(zoomSize);
        }
        lifeImage.fillAmount = player.health / 10;

    }

    // Updates the Text on the screen showing the current level time
    private void UpdateChrono()
    {
        timer += Time.deltaTime;

        //Transforms the timer into a formalized view form 00:00
        FormalizeTime();
    }

    private void FormalizeTime()
    {
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        timerText.text = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
    }

    void MakeGameOver()
    {
        isGameStarted = false;
        CalculateScore();
        for (int i = 0; i <  leaks.Length; i++)
        {
            Destroy(leaks[i]);
        }
        musicSource.Stop();
        StartCoroutine(ShowResults());
    }

    IEnumerator ShowResults()
    {
        musicSource.clip = gameOverMusic;
        musicSource.Play();
        audioFX.clip = gameOverClip;
        gameOverPanel.SetActive(true);
        for (int i = 0; i <  gameOverPanel.transform.childCount; i++)
        {
            // If you keep mouse1 down you skip the step by step
            if (Input.GetMouseButton(0) == true)
            {
                for (int j = 0; j < gameOverPanel.transform.childCount; j++)
                {
                    gameOverPanel.transform.GetChild(j).localScale = new Vector3(1, 1, 1);
                }
                    break;
            }
            gameOverPanel.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
            audioFX.Play();
            if (i + 3 >= gameOverPanel.transform.childCount)
            {
                yield return new WaitForSeconds(2f);
            } else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void CalculateScore()
    {
        if (Math.Round(timer) <= goldTime)
        {
            score += 100;
        }

        if (Math.Round(timer) > goldTime && Math.Round(timer) <= silverTime)
        {
            score += 50;
        }

        if (Math.Round(timer) > silverTime && Math.Round(timer) <= bronzeTime)
        {
            score += 25;
        }

        score += coinsCollected * 100;
        score -= waterBanksTouched * 100;

        switch(score)
        {

            case 400:
                finalScoreText.text = "S+";
                finalScoreText.color = Color.yellow;
                break;
            case 300:
                finalScoreText.text = "S";
                finalScoreText.color = Color.cyan;
                break;
            case 200:
                finalScoreText.text = "A";
                finalScoreText.color = Color.green;
                break;
            case 100:
                finalScoreText.text = "B";
                finalScoreText.color = Color.white;
                break;
            case 50:
                finalScoreText.text = "C";
                finalScoreText.color = Color.red;
                break;
            default:
                finalScoreText.text = "D";
                finalScoreText.color = Color.grey;
                break;
        }
    }

    public void ActivateZoom(bool isZoomingIn)
    {
        if (isZoomingIn)
        {
            zoomSize = zoomInSize;
        }
        else
        {
            zoomSize = zoomOutSize;
        }
        isZooming = true;
    }

    public void CollectCoin(GameObject coin)
    {
        coinsCollected += 1;
        audioFX.Stop();
        audioFX.clip = coinSoundEffect;
        audioFX.Play();
        Destroy(coin);
    }
}
