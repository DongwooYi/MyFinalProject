using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimation : MonoBehaviourPun
{
    //�÷��̾� ���� ����
    public enum State
    {
        IDLE,
        MOVE
    }
    //���� ����
    public State currState;
    //Animator
    public Animator anim;

    //���� ����
    public void ChangeState(State s)
    {
        photonView.RPC("RpcChangeState", RpcTarget.All, s);
    }

    [PunRPC]
    public void RpcChangeState(State s)
    {
        //���� ���°� s�� ���ٸ� �Լ��� ������.
        if (currState == s) return;

        //���� ���¸� s�� ����
        currState = s;

        //s�� ���� animation �÷���
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
