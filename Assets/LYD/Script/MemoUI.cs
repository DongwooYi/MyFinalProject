using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//cardui 프리팹에 스크립트를 넣는다.
//버튼을 클릭했을 때 
public class MemoUI : MonoBehaviour
{
    /*public enum CardPlayTayType
    {
        Doing,
        Complete,
        Overdue, //기한초과
        Request, //요청
        Hold, //보류
        Cancel,
        DI, //마감임박
        Feedback
    }

    public CardPlayTayType type;*/

    GameObject descPlay;

   public GameObject cardTag;
    public GameObject cts;

   public InputField cardNameText;
   public InputField memoText;

    //여기 밑에서 생성시켜줘야함. board UI Manager 스크립트에있는 아니였어!!!!!!
    //public Transform content;

    public string cardTitle;
    public string memo;

    public void Set(string s1, string s2)
    {
        cardTitle = s1;
        memo = s2;
    }

    public void Set1(GameObject go)
    {
        descPlay = go;

    }

    
    public void Set2(GameObject go1)
    {
        cts = go1;
    }
    // Start is called before the first frame update
    void Start()
    {
       /* cardNameText = descPlay.transform.GetChild(0).GetComponent<InputField>();
        memoText = descPlay.transform.GetChild(1).GetComponent<InputField>();*/

    }

    public void OnDescDisplay()
    {
        descPlay.SetActive(true);
        //바꾸고 싶은 텍스트를 가져와준다.
        
        cardNameText.text = cardTitle;
        memoText.text = memo;
        if(cts.name.Contains("Doing"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("Complete"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("Overdue"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("Request"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("Hold"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("Cancel"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("DI"))
        {
            Set2(cts);
        }
        if (cts.name.Contains("DI"))
        {
            Set2(cts);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
