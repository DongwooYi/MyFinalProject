using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;







[System.Serializable]
public class CardLists
{
    public string cardName;
    public string desc;
        public enum TagType
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

        public TagType tagType;
    
}

[System.Serializable]
public class ArrayData2
{
    public List<CardLists> data2;
}
public class WorkJsonTest : MonoBehaviour
{
    public GameObject type;

    public GameObject btn_add;
    public GameObject descDisplay;
    public GameObject descDisplay_cancelBtn;
    public GameObject descDisplay_OkBtn;
    public InputField input_Title;
    public InputField input_memo;

   // public idLists idListinfo = new idLists();
   // public CardLists cardListinfo;


    public List<CardLists> dataList2 = new List<CardLists>();

    // Start is called before the first frame update
    void Awake()
    {

    }
    //데이터 불러오는 곳.
    public void TaskBtn()
    {
        string path = UnityEngine.Application.dataPath + "/DataTodo/dataTodo.txt";
        //데이터를 불러온다.
        string jsonData2 = File.ReadAllText(path);
        print(jsonData2);

        //jsonData -> info
        ArrayData2 arrayData2 = JsonUtility.FromJson<ArrayData2>(jsonData2);

        //데이터생성
        for (int i = 0; i < arrayData2.data2.Count; i++)
        {
            CardLists info = arrayData2.data2[i];
            dataList2.Add(info);
            print(info.cardName);
            print(info.desc);
        }
    }
    
    //to do 옆에 + 버튼 누르면 descplay화면을 초기화 시켜주는것
    public void OnCardAddBtn()
    {
        descDisplay.SetActive(true);
       
        if(input_Title.text != null && input_memo.text != null)
        {
            input_Title.text = "";
            input_memo.text = "";
        }
        if(type != null)
        {
            Destroy(type);
        }
    }
    
    //1.확인 버튼을 누르면 정보를 저장한다. 
    //2. 취소버튼을 누르면 정보를 불러와서 json으로 만들고 json으로 불러온후에. 창이 꺼진다. 
    public void OnOKBtn()
    {
        //정보저장

        /*taskInfo.boardTitle = "Me";
        taskInfo.APIKey = "2";
        taskInfo.APIToken = "3";*/

        /*//idListinfo.boardName = "To Do";
        cardListinfo.cardName = "카드타이틀";
        cardListinfo.desc = "설명";
        idListinfo.inlineCards.Add(cardListinfo);*/

        //Debug.Log(cardListinfo.cardName);

        
        //new= 생성자, 변수를 생성해줌. 
        CardLists info = new CardLists();
        info.cardName = input_Title.text;
        info.desc = input_memo.text;
        if(type.name.Contains("Doing"))
        {
            info.tagType = CardLists.TagType.Doing;

        }
        if(type.name.Contains("Complete"))
        {
            info.tagType = CardLists.TagType.Complete;
        }
        if(type.name.Contains("Overdue"))
        {
            info.tagType = CardLists.TagType.Overdue;

        }
        if (type.name.Contains("Request"))
        {
            info.tagType = CardLists.TagType.Request;

        }
        if (type.name.Contains("Hold"))
        {
            info.tagType = CardLists.TagType.Hold;

        }
        if (type.name.Contains("Cancel"))
        {
            info.tagType = CardLists.TagType.Cancel;

        }
        if (type.name.Contains("DI"))
        {
            info.tagType = CardLists.TagType.DI;

        }
        if (type.name.Contains("Feedback"))
        {
            info.tagType = CardLists.TagType.Feedback;

        }


        //정보를 담아줌
        dataList2.Add(info);

        ArrayData2 arrayData2 = new ArrayData2();
        arrayData2.data2 = dataList2;
        string jsonData2 = JsonUtility.ToJson(arrayData2, true);
        print(jsonData2);

        //저장 경로 가져오기 //이렇게 되면 겹치는거아닌가?
        string path = UnityEngine.Application.dataPath + "/DataTodo";
        {
            Directory.CreateDirectory(path);
        }
        //Text 파일로 저장
        File.WriteAllText(path + "/dataTodo.txt", jsonData2);
    }

    //2. 취소버튼을 누르면 정보를 불러와서 json으로 만들고 json으로 불러온후에. 창이 꺼진다. 
    public void OnDescCancelBtn()
    {
        

        //파일경로
        string path = UnityEngine.Application.dataPath + "/DataTodo/dataTodo.txt";
        //데이터를 불러온다.
        string jsonData2 = File.ReadAllText(path);
        print(jsonData2);

        //jsonData -> info
        ArrayData2 arrayData2 = JsonUtility.FromJson<ArrayData2>(jsonData2);

        //데이터생성
        for(int i = 0; i < arrayData2.data2.Count; i++)
        {
            CardLists info = arrayData2.data2[i];
            //dataList2.Add(info);
            print(info.cardName);
            print(info.desc);
            //print(arrayData2.data2.Count);
        }

        descDisplay.SetActive(false);
    }

    
    



    // Update is called once per frame
    void Update()
    {
        
    }
}
