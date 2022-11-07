using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ç���� ���� ��ư ����
public class WorldManager2D : MonoBehaviour
{
    public InputField inputBookTitleName;
    public InputField inputMaxPeople;
    public Button btnMakeClub;

    void Start()
    {
        // å ���� �Է�
        inputBookTitleName.onValueChanged.AddListener(OnValueChanged);
        inputBookTitleName.onEndEdit.AddListener(OnEndEdit);



    }

    void Update()
    {
        
    }

    /* ���� ���� ��ư ���� */
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
