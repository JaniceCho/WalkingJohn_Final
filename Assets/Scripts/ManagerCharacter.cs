﻿using UnityEngine;
using System.Collections;

public class ManagerCharacter : MonoBehaviour
{
    public enum John_Movement
    {
        john_right_idle = 1, john_left_idle, john_back_idle, john_forward_idle, john_right = 11, john_left = 22, john_back = 33, john_forward = 44, john_dead
    }

    public float movePixel = 1f;
    public float moveSpeed = 3f;
    public John_Movement john_movement = John_Movement.john_forward_idle;

    public GameObject johnGhost;
    public GameObject touchPad;

    [HideInInspector]
    public new Transform transform;
    private Animator _animator;

    private int _layermask;
    private Vector2 prevPosition;
    private Vector2 johnsNextPosition;



    void Awake()
    {
        transform = GetComponent<Transform>();
        prevPosition = transform.position;
        _animator = GetComponent<Animator>();
        _layermask = LayerMask.GetMask("TouchPad");
        johnGhost.transform.position = transform.position;
        touchPad.transform.position = transform.position;
    }

    void Update()
    {
        if (ManagerGame.ghostMoved == true)
        {
            Invoke(john_movement.ToString(), 0.0f);
        }

        if (john_movement == John_Movement.john_dead)
        {
            return;
        }
        
        if (ManagerGame.gamePhase == 3 && ManagerGame.ghostMoved != true && ManagerGame.zombieMoved != true)
        {
            ProcessInput();
        }

        if (ManagerGame.gamePhase == 5)
        {
            //Day Text - 상단, 하단로그 + 스코어 업데이트
            UI_DayCount.UpdateDayCount();
            UI_Score.UpdateScore();
            ShowMessage.ShowMessage_Day();
            Mid();
            ManagerGame.john_day++;
            ManagerGame.gamePhase = 2;
        }


    }

    void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ManagerGame.ghostMoved = false;

            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10000f, _layermask);

            if (hit.collider != null)
            {
                int layer = hit.transform.gameObject.layer;

                if (layer == LayerMask.NameToLayer("TouchPad"))
                {
                    string tag = hit.transform.gameObject.tag;

                    johnsNextPosition = transform.position;
                    prevPosition = transform.position;
                    if (tag == "Right")
                    {
                        //Debug.Log("Right");
                        johnsNextPosition.x += movePixel;
                        johnGhost.transform.position = johnsNextPosition;
                        ManagerGame.ghostMoved = true;
                        john_movement = John_Movement.john_right;
                        _animator.SetInteger("john_movement", (int)john_movement);
                        touchPad.transform.position = johnGhost.transform.position;
                        touchPad.SetActive(false);
                    }
                    else if (tag == "Left")
                    {
                        // Debug.Log("Left");
                        johnsNextPosition.x -= movePixel;
                        johnGhost.transform.position = johnsNextPosition;
                        ManagerGame.ghostMoved = true;
                        john_movement = John_Movement.john_left;
                        _animator.SetInteger("john_movement", (int)john_movement);
                        touchPad.transform.position = johnGhost.transform.position;
                        touchPad.SetActive(false);
                    }
                    else if (tag == "Up")
                    {
                        //Debug.Log("Up");
                        johnsNextPosition.y += movePixel;
                        johnGhost.transform.position = johnsNextPosition;
                        ManagerGame.ghostMoved = true;
                        john_movement = John_Movement.john_back;
                        _animator.SetInteger("john_movement", (int)john_movement);
                        touchPad.transform.position = johnGhost.transform.position;
                        touchPad.SetActive(false);
                    }
                    else if (tag == "Down")
                    {
                        //Debug.Log("Down");
                        johnsNextPosition.y -= movePixel;
                        johnGhost.transform.position = johnsNextPosition;
                        ManagerGame.ghostMoved = true;
                        john_movement = John_Movement.john_forward;
                        _animator.SetInteger("john_movement", (int)john_movement);
                        touchPad.transform.position = johnGhost.transform.position;
                        touchPad.SetActive(false);
                    }
                    else if (tag == "Mid")
                    {
                        //Debug.Log("Mid");
                        //johnGhost.transform.position = johnsNextPosition;
                        //ManagerGame.ghostMoved = true;
                        //state = State.Idle;
                        //touchPad.transform.position = johnGhost.transform.position;
                        //touchPad.SetActive(false);
                        Mid();
                    }
                    ManagerGame.gamePhase = 4;
                }
            }
        }
    }

    public void Mid()
    {
        johnsNextPosition = transform.position;
        prevPosition = transform.position;
        johnGhost.transform.position = johnsNextPosition;
        ManagerGame.ghostMoved = true;
        john_movement = John_Movement.john_forward_idle;
        _animator.SetInteger("john_movement", (int)john_movement);
        touchPad.transform.position = johnGhost.transform.position;
        touchPad.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Tile_Rock" || coll.gameObject.tag == "Tile_Sea")
        {
            johnGhost.transform.position = prevPosition;
            transform.position = prevPosition;
            touchPad.transform.position = prevPosition;
        }
        else if (coll.gameObject.tag == "Item_Apple")
        {
            //Score + 300
            ManagerGame.john_score += 300;
            Debug.Log("john_score = " + ManagerGame.john_score);

            //게이지(HP) +20
            if (ManagerGame.john_hp + ManagerGame.appleValue >= 100)
            {

                ShowMessage2.ShowMessage_Apple();
                UI_HPGauge.HPPlus(ManagerGame.appleValue); // ManagerGame.john_hp = 100 
                                                           // & HP게이지 사이즈 1로 조정

                //메세지Test
                Debug.Log("Apple먹음 - Full HP // ManagerGame.john_hp : " + ManagerGame.john_hp);

                //부딪힌 오브젝트 끔
                coll.gameObject.SetActive(false);
            }
            else // 더해도 Full HP 아닐 때
            {
                ShowMessage2.ShowMessage_Apple();				
                UI_HPGauge.HPPlus(ManagerGame.appleValue); // ManagerGame.john_hp + 20 
                                                           // & HP게이지 사이즈 조정
                //메세지Test
                Debug.Log("Apple먹음 // ManagerGame.john_hp : " + ManagerGame.john_hp);
                coll.gameObject.SetActive(false);
            }
        }

    }


    /** For Animation State **/
    void john_right_idle()
    {

        ManagerGame.ghostMoved = false;

        //ManagerGame.moveOn = false;
        touchPad.SetActive(true);
    }

    void john_left_idle()
    {

        ManagerGame.ghostMoved = false;

        //ManagerGame.moveOn = false;
        touchPad.SetActive(true);
    }

    void john_forward_idle()
    {

        ManagerGame.ghostMoved = false;

        //ManagerGame.moveOn = false;
        touchPad.SetActive(true);
    }

    void john_back_idle()
    {

        ManagerGame.ghostMoved = false;

        //ManagerGame.moveOn = false;
        touchPad.SetActive(true);
    }

    void john_right()
    {
        if (MoveUtil.MoveByFrame(transform, johnGhost.transform.position, moveSpeed) == 0.0f)
        {
            transform.position = johnGhost.transform.position;
            john_movement = John_Movement.john_right_idle;
            _animator.SetInteger("john_movement", (int)john_movement);
            //touchPad.SetActive(true);
            return;
        }
    }

    void john_left()
    {
        if (MoveUtil.MoveByFrame(transform, johnGhost.transform.position, moveSpeed) == 0.0f)
        {
            transform.position = johnGhost.transform.position;
            john_movement = John_Movement.john_left_idle;
            _animator.SetInteger("john_movement", (int)john_movement);
            //touchPad.SetActive(true);
            return;
        }
    }

    void john_forward()
    {
        if (MoveUtil.MoveByFrame(transform, johnGhost.transform.position, moveSpeed) == 0.0f)
        {
            transform.position = johnGhost.transform.position;
            john_movement = John_Movement.john_forward_idle;
            _animator.SetInteger("john_movement", (int)john_movement);
            //touchPad.SetActive(true);
            return;
        }
    }

    void john_back()
    {
        if (MoveUtil.MoveByFrame(transform, johnGhost.transform.position, moveSpeed) == 0.0f)
        {
            transform.position = johnGhost.transform.position;
            john_movement = John_Movement.john_back_idle;
            _animator.SetInteger("john_movement", (int)john_movement);
            //touchPad.SetActive(true);
            return;
        }
    }

}
