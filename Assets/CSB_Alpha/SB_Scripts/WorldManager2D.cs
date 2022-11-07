using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 챌린지 생성 버튼 관리
public class WorldManager2D : MonoBehaviour
{
    public InputField inputBookTitleName;
    public InputField inputMaxPeople;
    public Button btnMakeClub;

    void Start()
    {
        // 책 제목 입력
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);



    }

    void Update()
    {
        
    }

    /* 모임 생성 버튼 관련 */
    public void MakeClub()
    {

    }

    void OnValueChanged(string s)
    {
        btnMakeClub.interactable = s.Length > 0;
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);

    }

}
