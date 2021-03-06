﻿using UnityEngine;
using System.Collections;

public class ManagerGame : MonoBehaviour
{
    public static bool ghostMoved = false;
    public static bool zombieMoved = false;

    public static ArrayList zombieList = new ArrayList();
    public static ArrayList bombList = new ArrayList();

    public static int gamePhase = 0;

    public static int john_hp = 100;
    public static int john_day = 1; //며칠 살아 남았는지. -> UI_DaysCount.cs
    public static int john_zombiekill = 0;
    public static int john_score = 0;

	// Score
    public static int dayScore = 100; // +1day를 더 살면
    public static int zombieScore = 200; //좀비를 죽이면
    //public static int appleScore = 300; //사과를 먹으면

    //Value : 체력 채워지는 값
    public static int appleValue = 20;

    GameObject gameOverUI;
    GameObject gameStartUI;

    public GameObject johnGhost;
    public GameObject touchPad;


    // Use this for initialization
    void Start()
    {
        touchPad.SetActive(false);

        gameStartUI = GameObject.Instantiate(Resources.Load("Images/UI/UI_gamestart"), johnGhost.transform.position, Quaternion.identity) as GameObject;
        gameStartUI.SetActive(true);

        gameOverUI = GameObject.Instantiate(Resources.Load("Images/UI/UI_gameover"), johnGhost.transform.position, Quaternion.identity) as GameObject;
        gameOverUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
  
        zombieMoved = false;
        foreach (GameObject zombie in zombieList)
        {
            if (zombie.GetComponent<CharacterZombie>().isMoving == true)
            {
                zombieMoved = true;
            }
        }

        if (gamePhase == 0 && Input.GetMouseButtonDown(0))
        {
            gameStartUI.SetActive(false);
            touchPad.SetActive(true);
            gamePhase = 1;
        }
        else if (gamePhase != 9 && john_hp <= 0)
        {
            gameOverUI.SetActive(true);
            //john_hp = -100;
            gamePhase = 9;
        }
        else if (gamePhase == 9 && Input.GetMouseButtonDown(0))
        {
            gameOverUI.SetActive(false);
            john_hp = 100;
            DestroyAll();
            ManagerGame.CreateMapObject();
            gameStartUI.SetActive(true);
            UI_HPGauge.HPReset();
            gamePhase = 0;
        }
    }

    // 게임이 끝나면 Rock, John, Item, Zombie를 없앰.
    public static void DestroyAll()
    {
        Destroy(GameObject.Find("Map"));
        zombieList.Clear();
    }

    // Empty Object "Map"만들기
    public static void CreateMapObject()
    {
        GameObject Map = new GameObject("Map");
    }
}
