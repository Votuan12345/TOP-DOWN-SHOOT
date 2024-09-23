using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int enemyMax;
    private int enemyCount;
    public List<GameObject> enemies = new List<GameObject>();
    public float spawnEnemyTime;
    private int kill; // số lượng quái bị hạ
    private bool isSpawnEnemy;
    public float spawnEnemyPos;
    public int batch; // từng đợt
    public float difficultyTimer;

    public Texture2D crosshairCursor;
    private bool isGameOver;
    private bool isWin;

    public int Kill { 
        get => kill; 
        set
        {
             kill = value;
            GameGUIManager.instance.ShowKillText(kill);
        } 
    }

    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public bool IsWin { get => isWin; set => isWin = value; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeMouse();
        Kill = 0;
        isSpawnEnemy = true;
        enemyCount = 0;
        StartCoroutine(spawnEnemy());
        InvokeRepeating("BoostDifficulty", difficultyTimer, difficultyTimer);
    }

    private void Update()
    {
        if (kill >= enemyMax && !isGameOver && !isWin)
        {
            isWin = true;
            GameGUIManager.instance.ShowWinPanel();
            StopAllCoroutines();
        }

        if (isWin) return;

        if (isGameOver)
        {
            StopAllCoroutines();
            GameGUIManager.instance.ShowGameOverPanel();
        }
    }

    IEnumerator spawnEnemy()
    {
        
        if((enemyCount-kill) >= batch)
        {
            isSpawnEnemy = false;
        }
        else
        {
            isSpawnEnemy = true;
        }
        
        if(enemies != null && enemies.Count > 0 && isSpawnEnemy)
        {
            int index = Random.Range(0, enemies.Count);
            // 55, 54
            int randomPosX = Random.Range(-1, 1);
            int randomPosY = Random.Range(-1, 1);
            if(randomPosX == 0) randomPosX = 1;
            if(randomPosY == 0) randomPosY = 1;

            GameObject enemyGameObject = Instantiate(enemies[index], new Vector3(randomPosX, randomPosY, 0) * spawnEnemyPos, Quaternion.identity);
            enemyGameObject.GetComponent<EnemyAI>().Id = enemyCount;

            enemyCount++;
        }
        yield return new WaitForSeconds(spawnEnemyTime);

        if (enemyCount >= enemyMax)
        {
            isSpawnEnemy = false;
            StopCoroutine(spawnEnemy());
        }
        else
        {
            StartCoroutine(spawnEnemy());
        }


    }

    // Invoke
    private void BoostDifficulty()
    {
        if(spawnEnemyTime > 3)
        {
            spawnEnemyTime -= 0.5f;
            batch += 1;
        }
    }

    public void ChangeMouse()
    {
        Cursor.SetCursor(crosshairCursor, new Vector2(crosshairCursor.width / 2, crosshairCursor.height), CursorMode.Auto);
    }

    public void ResetMouse()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
