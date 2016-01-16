﻿using UnityEngine;
using System.Collections;

public class ManagerGame : MonoBehaviour
{
    public static int gamePhase = 0;
    public static int gameDays = 0;

    public int john_hp = 100;
    public int john_day = 1;
    public int john_score = 0;

    GameObject gameOverUI;
    GameObject gameStartUI;


    // Use this for initialization
    void Start()
    {
        gameStartUI = GameObject.Instantiate(Resources.Load("Images/UI/UI_gamestart"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        gameStartUI.SetActive(true);

        gameOverUI = GameObject.Instantiate(Resources.Load("Images/UI/UI_gameover"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        gameOverUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (gamePhase == 0 && Input.GetMouseButtonDown(0))
        {
            gameStartUI.SetActive(false);
            gamePhase = 1;
        }

        else if (john_hp == 0)
        {
            gameOverUI.SetActive(true);
            john_hp = -1;
        }
        else if (john_hp == -1 && Input.GetMouseButtonDown(0))
        {
            gameOverUI.SetActive(false);
            john_hp = 100;
            DestroyAll();
            ManagerGame.CreateMapObject();
            gameStartUI.SetActive(true);
            gamePhase = 0;
        }
    }

    // 게임이 끝나면 Rock, John, Item, Zombie를 없앰.
    public static void DestroyAll()
    {
        Destroy(GameObject.Find("Map"));
    }

    // Empty Object "Map"만들기
    public static void CreateMapObject()
    {
        GameObject Map = new GameObject("Map");
    }
}
