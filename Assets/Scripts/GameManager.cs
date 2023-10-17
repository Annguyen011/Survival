using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("GameControl")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 20f;
    [Header("Player info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 20, 40, 60, 100, 150, 200, 250, 300, 400, 500, 600, 700, 900, 1000, 1200 };
    [Header("Gameobject")]
    public Player player;
    public PoolManager poolManager;
    public LevelUp uiLevelUp;
    public Result uiResuilt;
    public GameObject enemyCleaner;

    private void Awake()
    {
        Instance = this;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        if (poolManager == null)
        {
            poolManager = GameObject.FindObjectOfType<PoolManager>();
        }
        
    }


    private void Start()
    {
        uiLevelUp.Hide();

        Stop();
    }

    private void Update()
    {
        if (!isLive) return;

        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;

            GameVictory();
        }

    }
    public void GameStart(int playerId)
    {
        this.playerId = playerId;

        maxHealth = 100;
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.Selected(playerId % 2);

        isLive = true;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        Resume();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());

        uiResuilt.gameObject.SetActive(true);
    }

    private IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(.5f);

        uiResuilt.Lose();
        Stop();

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);

    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());

        uiResuilt.gameObject.SetActive(true);
    }

    private IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(.5f);
        uiResuilt.Win();

        Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);

    }

    public void GetExp()
    {
        if (!isLive) return;

        exp++;
        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();


        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
