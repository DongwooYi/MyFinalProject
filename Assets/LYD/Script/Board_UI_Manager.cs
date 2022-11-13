using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Board_UI_Manager : MonoBehaviour
{
    //tagImage �ִ� ������Ʈ â 
    public GameObject Tags;
    public GameObject doing;
    public GameObject complete;
  //  public GameObject overdue;
  //  public GameObject request;
    public GameObject hold;
    public GameObject cancel;
    public GameObject di;
   // public GameObject feedback;
    public Transform TagContent;
    GameObject taggo;
    
    public Transform cardContent;
    public Transform meTagContent;

    //cardPlay�ȿ� �ִ� Tramsform content�� �����;���. 
    public Transform cardPlayTagContent;

    //������������ �ִ´�.
    public GameObject cardObject;

    //���⿡ �켱 descDisplay �־��
   public GameObject cardDisplay;

    public GameObject CardNameText;
    public GameObject DescText;
    
    WorkJsonTest jsonTestClass;
    
    public MemoUI memoUI;

    public List<GameObject> idx;

    //public bool cardRemove = true;

    public bool canTag = false;

    // Start is called before the first frame update
    void Start()
    {
        jsonTestClass = GetComponent<WorkJsonTest>();
        //cardRemove = false;
    }


    //���� ��ư ������ ī�� ��ư�� ������ �� ī�尡 ���� �ǰ� ���� �����Ǿ���.
    //�׷��� ī�尡 ������ �ȴ����� �˾ƾ��Ѵ�. -> ���̷�? 
    //1. content�� ��ġ�� ã�ƾ���
    //2. content�� �ڽĿ��� idx������ ã���ֱ� 
    //3. ��ư�� �������� Interactable�� �������.
    //4. destroy cardObject ���ֱ�
    //5. ������ ���� <workJsonTest �����ؼ� �������ֱ�>
    // Update is called once per frame
    //6. �� ��, false�� �ٲ��ش�. 
    void Update()
    {
        //�����ϴ°� ����! ������ ������ �Ʊ��ϱ� �������.
        //�Ұ��־ ������ư�� ������ �̺����ý��� �ߵ� 
        #region cardRemove
        /* if (cardRemove)
         {
             if (Input.GetMouseButtonDown(0))
             {


                 //GameObject go1 = EventSystem.current.currentSelectedGameObject;
                 //
                 Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                 RaycastHit hit;
                 if (Physics.Raycast(ray, out hit))
                 {

                      Transform tf = hit.transform;
                     int idx2 = 0;
                     tf.GetComponent<Button>().interactable = false;
                     for (int i = 0; i < idx.Count; i++)
                     {
                         //1. content�� ��ġ�� ã�ƾ���
                         //2. content�� �ڽĿ��� idx������ ã���ֱ� 
                         //����Ʈ ����!!!!!!!!!!!
                         print(hit.transform.name);

                         //hit�� �ſ��� list�� ã������� 
                         if (tf.gameObject == idx[i])
                         {
                             idx2 = i;
                             //tf.parent = cardContent;
                         }


                     }
                     idx.RemoveAt(idx2);
                     //remove�� ������ ���� removeat ��ġ�� ���� 
                     //������Ʈ�� ������ ������ ��ġ�� �𸦼� �ֱ⶧���� ��ġ�� ã���ִ� ���� �� ��
                     jsonTestClass.dataList2.RemoveAt(idx2);
                     Destroy(tf.gameObject);
                     cardRemove = false;

                     // destroy
                     //3. ��ư�� �������� Interactable�� �������.
                     //4. destroy cardObject ���ֱ�
                     //5. ������ ���� <workJsonTest �����ؼ� �������ֱ�>



                 }
                 //Destroy(go1);
                 //cardRemove = false;
             }
         }*/
        #endregion

        if(canTag)
        {
            if(Input.GetMouseButton(0))
            {
                //2. �Ұ�(canTag)�� true�� �Ǹ鼭 ���̸� ���.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    //3. ���콺 ��Ŭ������ ���̸� ���� �� �±װ� ������ Tagâ�� ������.  
                    if(hit.transform.tag.Contains("Doing"))
                    {
                        Tags.SetActive(false);
                        //4. �ϳ��� â(������ �� ������Ʈ(descPlay����)���� �����ȴ�.
                        taggo = Instantiate(doing, TagContent);
                        //5. �� Ŭ���� ���� jsonTescClass.type �� �־��ش�. 
                        jsonTestClass.type = taggo;
                        canTag = false;

                    }
                    else if(hit.transform.tag.Contains("Complete"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(complete, TagContent);
                        jsonTestClass.type = taggo;

                        canTag = false;


                    }
                    /*else if (hit.transform.tag.Contains("Overdue"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(overdue, TagContent);
                        jsonTestClass.type = taggo;


                        canTag = false;


                    }
                    else if (hit.transform.tag.Contains("Request"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(request, TagContent);
                        jsonTestClass.type = taggo;


                        canTag = false;


                    }*/
                    else if (hit.transform.tag.Contains("Hold"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(hold, TagContent);
                        jsonTestClass.type = taggo;


                        canTag = false;

                    }
                    else if (hit.transform.tag.Contains("Cancel"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(cancel, TagContent);
                        jsonTestClass.type = taggo;

                        canTag = false;


                    }
                    else if (hit.transform.tag.Contains("DI"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(di, TagContent);
                        jsonTestClass.type = taggo;

                        canTag = false;


                    }
                   /* else if (hit.transform.tag.Contains("Feedback"))
                    {
                        Tags.SetActive(false);
                        taggo = Instantiate(feedback, TagContent);
                        jsonTestClass.type = taggo;

                        canTag = false;


                    }*/

                }

                //4. �ϳ��� â(������ �� ������Ʈ(descPlay����)���� �����ȴ�.
                //5. �� Ŭ���� ���� jsonTescClass.type �� �־��ش�. 
            }
        }
    }


    //descPlay x ��ư ������ ���� ��ư�� ����Ǵ� �Լ�

    public Text cardTexts;
    public void PostCardData()
    {

        //���� ������ 3��°�Ű� ���� �� �������� �ڽĿ�����Ʈ ������ ã�� (���� ����� ���������� ã��)
        GameObject go = Instantiate(cardObject, cardContent); //�θ� �� �����ÿ�
        //�̸��ٲٱ� 
        cardTexts = go.GetComponentInChildren<Text>();
     
        //�ٽ� ������Ű�� ���ؼ��� 
        cardTexts.text = jsonTestClass.dataList2[jsonTestClass.dataList2.Count - 1].cardName;

        memoUI = go.GetComponent<MemoUI>();
        memoUI.cardNameText = cardDisplay.transform.GetChild(0).GetComponent<InputField>();
        memoUI.memoText = cardDisplay.transform.GetChild(1).GetComponent<InputField>();
        memoUI.Set1(cardDisplay);
        memoUI.memoText.text = jsonTestClass.dataList2[jsonTestClass.dataList2.Count - 1].desc;
        memoUI.Set(cardTexts.text, memoUI.memoText.text);

        if (jsonTestClass.input_Title.text == "" && jsonTestClass.input_memo.text == "")
        {
            Destroy(go.gameObject);
        }
            //memoUI.memoText.text = jsonTestClass.dataList2[jsonTestClass.dataList2.Count - 1].desc;

       


        //jsonTestClass.type = gameObject;
        if (jsonTestClass.type.name.Contains("Doing"))
        {
            
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");
            GameObject meTag = Instantiate(doing, parentTag);
            //enumTagType ���� �����͸���Ʈ�� �־��!!!
            //memoUI.type. = jsonTestClass.dataList2[jsonTestClass.dataList2.Count - 1].tagType;
            memoUI.cardTag = Instantiate(doing, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }
        if (jsonTestClass.type.name.Contains("Complete"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(complete, parentTag);
            memoUI.cardTag = Instantiate(complete, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }
       /* if (jsonTestClass.type.name.Contains("Overdue"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(overdue, parentTag);
            memoUI.cardTag = Instantiate(overdue, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }
        if (jsonTestClass.type.name.Contains("Request"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(request, parentTag);
            memoUI.cardTag = Instantiate(request, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }*/
        if (jsonTestClass.type.name.Contains("Hold"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(hold, parentTag);
            memoUI.cardTag = Instantiate(hold, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }
        if (jsonTestClass.type.name.Contains("Cancel"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(cancel, parentTag);
            memoUI.cardTag = Instantiate(cancel, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }
        if (jsonTestClass.type.name.Contains("DI"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(di, parentTag);
            memoUI.cardTag = Instantiate(di, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }
      /*  if (jsonTestClass.type.name.Contains("Feedback"))
        {
            Transform parentTag = go.transform.Find("Scroll View/Viewport/Content");

            GameObject meTag = Instantiate(feedback, parentTag);
            memoUI.cardTag = Instantiate(feedback, cardPlayTagContent);
            memoUI.Set2(memoUI.cardTag);

        }*/




        //wprint($"cardText : {cardTexts[0].text}");

        // desc1 = go.GetComponent<Desc1>();

        //cardTexts[1].text = jsonTestClass.dataList1[to_do_idx].inlineCards[0].desc;

    }

    //to do list â �߰� �ϱ� ���� ���׶� ��ư ������ ����Ǿ��ִ� ���� ī��� �����Ǵ� �Լ�
    public void StartPostCardData()
    {
       //���⼭ ����� ���� ī��� ������ ��. 
        for(int i = 0; i < jsonTestClass.dataList2.Count; i++)
        {
            
            GameObject go = Instantiate(cardObject, cardContent); //�θ� �� �����ÿ�
            //������ ī�������Ʈ�� �ϳ��� ���ӿ�����Ʈ ����Ʈ�� �־��ֱ� <�ƾ� �Ⱥ��ϴ� �������� / ����Ʈ> 
            
            
            cardTexts = go.GetComponentInChildren<Text>();
            cardTexts.text = jsonTestClass.dataList2[i].cardName;
            memoUI = go.GetComponent<MemoUI>();
            memoUI.cardNameText = cardDisplay.transform.GetChild(0).GetComponent<InputField>();
            memoUI.memoText = cardDisplay.transform.GetChild(1).GetComponent<InputField>();
            memoUI.Set1(cardDisplay);
             memoUI.memoText.text = jsonTestClass.dataList2[i].desc;
            memoUI.Set(cardTexts.text, memoUI.memoText.text);
            idx.Add(go);
            //������ư ���� �� ���̽��� ��Ҵ� �����͵鵵 �����ؼ� �������ֱ�!
            //���� ���ְ� ���� ��: ī�� �����տ� ���ڸ� �־��ִ� ��! 
            //idx ��ũ��Ʈ�� ī�彺ũ��Ʈ���� ������ �Ǵ� �Ŵϱ� 


        }

    }

    public void OffCardDisplay()
    {
        cardDisplay.SetActive(false);

    }

    /*//ī�� ��ư�� ������ descȭ���� ������. �װſ� �´� Ÿ��Ʋ�� ������ ���´�. 
    public void OnOpenDisplay()
    {
        descDisplay.SetActive(true);
       //CardNameText = cardTexts[0].text;
    }*/

    //�����ϴ� ��ư �Լ� *�������� ����!
    #region cardRemoveBtn
    /*//������ư���� �ϳ��� �Ұ��� �����Ͽ� true/false�� ���ֱ�
    public void OnRemoveCard()
    {
        cardRemove = true;
    }*/
    #endregion

    //1. descPlay�� �ִ� �±��߰���ư ������ ImageTagBackground�� ����
    //2. �Ұ�(canTag)�� true�� �Ǹ鼭 ���̸� ���.
    //3. ���콺 ��Ŭ������ ���̸� ���� �� �±װ� ������ Tagâ�� ������.
    //4. �ϳ��� â(������ �� ������Ʈ(descPlay����)���� �����ȴ�.
    //5. �� Ŭ���� ���� jsonTescClass.type �� �־��ش�. 
    public void OnTag()
    {
        Tags.SetActive(true);
        canTag = true;

    }

    public void TagXBtn()
    {
        Tags.SetActive(false);
    }

    
}
