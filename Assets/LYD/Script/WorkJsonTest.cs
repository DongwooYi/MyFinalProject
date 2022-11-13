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
            Overdue, //�����ʰ�
            Request, //��û
            Hold, //����
            Cancel,
            DI, //�����ӹ�
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
    //������ �ҷ����� ��.
    public void TaskBtn()
    {
        string path = UnityEngine.Application.dataPath + "/DataTodo/dataTodo.txt";
        //�����͸� �ҷ��´�.
        string jsonData2 = File.ReadAllText(path);
        print(jsonData2);

        //jsonData -> info
        ArrayData2 arrayData2 = JsonUtility.FromJson<ArrayData2>(jsonData2);

        //�����ͻ���
        for (int i = 0; i < arrayData2.data2.Count; i++)
        {
            CardLists info = arrayData2.data2[i];
            dataList2.Add(info);
            print(info.cardName);
            print(info.desc);
        }
    }
    
    //to do ���� + ��ư ������ descplayȭ���� �ʱ�ȭ �����ִ°�
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
    
    //1.Ȯ�� ��ư�� ������ ������ �����Ѵ�. 
    //2. ��ҹ�ư�� ������ ������ �ҷ��ͼ� json���� ����� json���� �ҷ����Ŀ�. â�� ������. 
    public void OnOKBtn()
    {
        //��������

        /*taskInfo.boardTitle = "Me";
        taskInfo.APIKey = "2";
        taskInfo.APIToken = "3";*/

        /*//idListinfo.boardName = "To Do";
        cardListinfo.cardName = "ī��Ÿ��Ʋ";
        cardListinfo.desc = "����";
        idListinfo.inlineCards.Add(cardListinfo);*/

        //Debug.Log(cardListinfo.cardName);

        
        //new= ������, ������ ��������. 
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


        //������ �����
        dataList2.Add(info);

        ArrayData2 arrayData2 = new ArrayData2();
        arrayData2.data2 = dataList2;
        string jsonData2 = JsonUtility.ToJson(arrayData2, true);
        print(jsonData2);

        //���� ��� �������� //�̷��� �Ǹ� ��ġ�°žƴѰ�?
        string path = UnityEngine.Application.dataPath + "/DataTodo";
        {
            Directory.CreateDirectory(path);
        }
        //Text ���Ϸ� ����
        File.WriteAllText(path + "/dataTodo.txt", jsonData2);
    }

    //2. ��ҹ�ư�� ������ ������ �ҷ��ͼ� json���� ����� json���� �ҷ����Ŀ�. â�� ������. 
    public void OnDescCancelBtn()
    {
        

        //���ϰ��
        string path = UnityEngine.Application.dataPath + "/DataTodo/dataTodo.txt";
        //�����͸� �ҷ��´�.
        string jsonData2 = File.ReadAllText(path);
        print(jsonData2);

        //jsonData -> info
        ArrayData2 arrayData2 = JsonUtility.FromJson<ArrayData2>(jsonData2);

        //�����ͻ���
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
