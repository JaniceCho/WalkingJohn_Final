﻿using UnityEngine;

public class ManagerRespawn : MonoBehaviour
{
    public enum State
    {
        level_1, level_2, level_3
    }

    public float[,] tile_index =
        new float[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 1, 0, 0 }, { 0, -1, 0 },
            { -1, 0, 0 }, { 0, 2, 0 }, { 1, 1, 0 }, { 2, 0, 0 },
            { 1, -1, 0 }, { 0, -2, 0 }, { -1, -1, 0 }, { -2, 0, 0 },
            { -1, 1, 0 }, { 0, 3, 0 }, { 1, 2, 0 }, { 2, 1, 0 },
            { 3, 0, 0 }, { 2, -1, 0 }, { 1, -2, 0 }, { 0, -3, 0 },
            { -1, -2, 0 }, { -2, -1, 0 }, { -3, 0, 0 }, { -2, 1, 0 }, { -1, 2, 0 } };

    public float movePixel = 32.0f;

    public int balence_level_1_border = 10;
    public int balence_level_2_border = 20;

    public int balence_level_1_zombieResNum = 1;
    public int balence_level_1_appleResNum = 2;

    public int balence_level_2_zombieResNum = 2;
    public int balence_level_2_appleResNum = 1;

    public int balence_level_3_zombieResNum = 3;
    public int balence_level_3_appleResNum = 1;

    //public float moveSpeed = 1.0f;
    public State state = State.level_1;

    public GameObject johnGhost;
    public GameObject map_zombie;
    public GameObject map_apple;
    public GameObject map_bomb;

    [HideInInspector]
    public new Transform transform;
    private CharacterController _cc;
    private Animator _animator;

    private int _layermask;

    private GameObject _character_zombie;
    private GameObject _item_apple;

    public GameObject touchPad;

    public Vector3 resPos;

    void Awake()
    {
        transform = GetComponent<Transform>();
        //_cc = GetComponent<CharacterController>();
        //_animator = GetComponent<Animator>();
        _layermask = LayerMask.GetMask("TouchPad");
        movePixel /= 100;
    }

    void Update()
    {

        //if(ManagerGame.moveOn == true)
        //Invoke(state.ToString(), 0.0f);

        if (ManagerGame.gamePhase == 2)
        {
            //touchPad.SetActive(true);
            ProcessInput();
            
            ManagerGame.gamePhase = 3;
        }
    }

    public bool tileCheck(float x, float y)
    {
        
        foreach (Transform child in GameObject.Find("Map").transform)
        {
            //child is your child transform
            //Debug.Log(child.transform.position.x + ", " + child.transform.position.y);
            if (child.transform.position.x == x && child.transform.position.y == y)
                return false;
        }
        
        return true;
    }

    public void ProcessInput()
    {
        if (0 <= ManagerGame.john_day && ManagerGame.john_day <= balence_level_1_border)
            state = State.level_1;
        else if (balence_level_1_border < ManagerGame.john_day && ManagerGame.john_day <= balence_level_2_border)
            state = State.level_2;
        else if (balence_level_2_border < ManagerGame.john_day)
            state = State.level_3;

        int selectedTile = 0;
        int bombChance = 0;

        
        bombChance = Random.Range(1, 10);
        selectedTile = Random.Range(1, 24);
        Random.seed = (int)System.DateTime.Now.Ticks * 10;

        if (bombChance >= 6)
        {
            resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

            if (tileCheck(resPos.x, resPos.y))
            {
                map_bomb = Instantiate(Resources.Load("Prefabs/Item_Mine"), resPos, Quaternion.identity) as GameObject;

                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_bomb.transform.parent = GameObject.Find("Map").transform;
            }

            
        }
        


        if (state == State.level_1)
        {
            for (int i = 0; i < balence_level_1_zombieResNum; i++)
            {
                Random.seed = (int)System.DateTime.Now.Ticks;
                selectedTile = Random.Range(13, 24);

                resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

                if (!tileCheck(resPos.x, resPos.y))
                    break;
               
                map_zombie = Instantiate(Resources.Load("Prefabs/Character_Zombie"), resPos, Quaternion.identity) as GameObject;
                
                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_zombie.transform.parent = GameObject.Find("Map").transform;
                // 리스폰매니저가 가지고 있는 존을 좀비에게 붙임
                map_zombie.gameObject.GetComponent<CharacterZombie>().johnGhost = johnGhost;
                ManagerGame.zombieList.Add(map_zombie);

            }

            for (int i = 0; i < balence_level_1_appleResNum; i++)
            {
                Random.seed = (int)System.DateTime.Now.Ticks*2;
                selectedTile = Random.Range(13, 24);

                resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

                if (!tileCheck(resPos.x, resPos.y))
                    break;

                map_apple = Instantiate(Resources.Load("Prefabs/Item_apple"), resPos, Quaternion.identity) as GameObject;

                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_apple.transform.parent = GameObject.Find("Map").transform;
            }
        }
        else if (state == State.level_2)
        {
            for (int i = 0; i < balence_level_2_zombieResNum; i++)
            {
                Random.seed = (int)System.DateTime.Now.Ticks*3;
                selectedTile = Random.Range(5, 24);

                resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

                if (!tileCheck(resPos.x, resPos.y))
                    break;

                map_zombie = Instantiate(Resources.Load("Prefabs/Character_Zombie"), resPos, Quaternion.identity) as GameObject;

                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_zombie.transform.parent = GameObject.Find("Map").transform;
                // 리스폰매니저가 가지고 있는 존을 좀비에게 붙임
                map_zombie.gameObject.GetComponent<CharacterZombie>().johnGhost = johnGhost;
                ManagerGame.zombieList.Add(map_zombie);
            }

            for (int i = 0; i < balence_level_2_appleResNum; i++)
            {
                Random.seed = (int)System.DateTime.Now.Ticks*4;
                selectedTile = Random.Range(5, 24);

                resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

                if (!tileCheck(resPos.x, resPos.y))
                    break;

                map_apple = Instantiate(Resources.Load("Prefabs/Item_apple"), resPos, Quaternion.identity) as GameObject;

                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_apple.transform.parent = GameObject.Find("Map").transform;
            }
        }
        else if (state == State.level_3)
        {
            for (int i = 0; i < balence_level_3_zombieResNum; i++)
            {
                Random.seed = (int)System.DateTime.Now.Ticks*5;
                selectedTile = Random.Range(5, 24);

                resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

                if (!tileCheck(resPos.x, resPos.y))
                    break;

                map_zombie = Instantiate(Resources.Load("Prefabs/Character_Zombie"), resPos, Quaternion.identity) as GameObject;

                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_zombie.transform.parent = GameObject.Find("Map").transform;
                // 리스폰매니저가 가지고 있는 존을 좀비에게 붙임
                map_zombie.gameObject.GetComponent<CharacterZombie>().johnGhost = johnGhost;
                ManagerGame.zombieList.Add(map_zombie);
            }

            for (int i = 0; i < balence_level_3_appleResNum; i++)
            {
                Random.seed = (int)System.DateTime.Now.Ticks*6;
                selectedTile = Random.Range(5, 24);

                resPos = johnGhost.transform.position +
                new Vector3(tile_index[selectedTile, 0], tile_index[selectedTile, 1], tile_index[selectedTile, 2]);

                if (!tileCheck(resPos.x, resPos.y))
                    break;

                map_apple = Instantiate(Resources.Load("Prefabs/Item_apple"), resPos, Quaternion.identity) as GameObject;

                // rock tile 생성한 것을 Map오브젝트의 Child로 넣자
                map_apple.transform.parent = GameObject.Find("Map").transform;
            }
        }
    }


    //Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //Ray2D ray = new Ray2D(wp, Vector2.zero);
    //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

    /*
    if (hit.collider != null)
    {
        int layer = hit.transform.gameObject.layer;

        if (layer == LayerMask.NameToLayer("TouchPad"))
        {
            string tag = hit.transform.gameObject.tag;

            Vector2 johnsNextPosition = transform.position;

            if (tag == "Right")
            {
                Debug.Log("Right");
                johnsNextPosition.x += movePixel;
                johnGhost.transform.position = johnsNextPosition;
                ManagerGame.ghostMoved = true;
                state = State.Right;
            }
            else if (tag == "Left")
            {
                Debug.Log("Left");
                johnsNextPosition.x -= movePixel;
                johnGhost.transform.position = johnsNextPosition;
                ManagerGame.ghostMoved = true;
                state = State.Left;
            }
            else if (tag == "Up")
            {
                Debug.Log("Up");
                johnsNextPosition.y += movePixel;
                johnGhost.transform.position = johnsNextPosition;
                ManagerGame.ghostMoved = true;
                state = State.Up;
            }
            else if (tag == "Down")
            {
                Debug.Log("Down");
                johnsNextPosition.y -= movePixel;
                johnGhost.transform.position = johnsNextPosition;
                ManagerGame.ghostMoved = true;
                state = State.Down;
            }
            else if (tag == "Mid")
            {
                Debug.Log("Mid");
                johnGhost.transform.position = johnsNextPosition;
                ManagerGame.ghostMoved = true;
                state = State.Idle;
            }
        }
        */





    /** For Animation State **/
    void Idle()
    {

    }



}
