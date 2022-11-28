using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimation : MonoBehaviourPun
{
    //플레이어 상태 정의
    public enum State
    {
        IDLE,
        MOVE
    }
    //현재 상태
    public State currState;
    //Animator
    public Animator anim;

    //상태 변경
    public void ChangeState(State s)
    {
        photonView.RPC("RpcChangeState", RpcTarget.All, s);
    }

    [PunRPC]
    public void RpcChangeState(State s)
    {
        //현재 상태가 s와 같다면 함수를 나간다.
        if (currState == s) return;

        //현재 상태를 s로 셋팅
        currState = s;

        //s에 따른 animation 플레이
        switch (s)
        {
            case State.IDLE:
                anim.SetTrigger("Idle");
                break;
            case State.MOVE:
                anim.SetTrigger("Move");
                break;
        }
    }

    [PunRPC]
    public void RpcSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }
}
