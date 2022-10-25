using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    //SpawnPos 들
    public Vector3[] spawnPos;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        //Rpc 호출 빈도
        PhotonNetwork.SendRate = 60;

        //자리 계산 
        //1. spawnPos의 갯수를 할당
        spawnPos = new Vector3[4];
        
        //2. 각도계산 (360 / maxPlayer)
        float angle = 360.0f / spawnPos.Length;
        //3. GameManager 중심에서 5만큼 떨어진 위치들 계산
        for(int i = 0; i < spawnPos.Length; i++)
        {
            spawnPos[i] = transform.position + transform.forward * 5;
            transform.Rotate(0, angle, 0);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = spawnPos[i];
        }

        //현재 방에 들어와있는 인원수를 이용해서 idx 구하자
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //플레이어를 생성한다.
        //PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
        PhotonNetwork.Instantiate("Plane", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }

    //현재 방에 있는 Player를 담아놓자.
    public List<PhotonView> players = new List<PhotonView>();
    public void AddPlayer(PhotonView pv)
    {
        players.Add(pv);
        //만약에 인원이 다들어왔으면
        if(players.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            //턴 변경
            ChangeTurn();
        }
    }

    //턴 변경시 호출 해주는 함수
    public int turnIdx = -1;
    public void ChangeTurn()
    {
        //방장이 아니라면 함수를 나가라!
        if (PhotonNetwork.IsMasterClient == false) return;

        //이전 차례였던 애를 총을 쏘지 못하게!
        if(turnIdx > -1)
        {
            players[turnIdx].RPC("SetMyTurn", RpcTarget.All, false);
        }
        //이번 너의 차례다
        turnIdx++;
        turnIdx %= players.Count;
        players[turnIdx].RPC("SetMyTurn", RpcTarget.All, true);
    }

    //방에 플레이어가 참여 했을 때 호출해주는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        print(newPlayer.NickName + "이 방에 들어왔습니다.");
    }
}
