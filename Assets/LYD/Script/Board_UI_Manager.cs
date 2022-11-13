using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Board_UI_Manager : MonoBehaviour
{
    //tagImage 넣는 오브젝트 창 
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

    //cardPlay안에 있는 Tramsform content를 가져와야함. 
    public Transform cardPlayTagContent;

    //보드프리팹을 넣는다.
    public GameObject cardObject;

    //여기에 우선 descDisplay 넣어보기
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


    //삭제 버튼 누르고 카드 버튼을 누르면 그 카드가 삭제 되고 값도 삭제되야함.
    //그러면 카드가 눌린지 안눌린지 알아야한다. -> 레이로? 
    //1. content의 위치를 찾아야함
    //2. content의 자식에서 idx순서를 찾아주기 
    //3. 버튼을 누를때는 Interactable을 꺼줘야함.
    //4. destroy cardObject 해주기
    //5. 데이터 삭제 <workJsonTest 접근해서 삭제해주기>
    // Update is called once per frame
    //6. 그 후, false로 바꿔준다. 
    void Update()
    {
        //삭제하는거 만듦! 쓰지는 않지만 아까우니까 살려놓자.
        //불값넣어서 삭제버튼을 누를때 이벤스시스템 발동 
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
                         //1. content의 위치를 찾아야함
                         //2. content의 자식에서 idx순서를 찾아주기 
                         //리스트 삭제!!!!!!!!!!!
                         print(hit.transform.name);

                         //hit쏜 거에서 list를 찾아줘야함 
                         if (tf.gameObject == idx[i])
                         {
                             idx2 = i;
                             //tf.parent = cardContent;
                         }


                     }
                     idx.RemoveAt(idx2);
                     //remove는 내용을 지워 removeat 위치를 지워 
                     //오브젝트의 내용은 알지만 위치를 모를수 있기때문에 위치를 찾아주는 것이 더 편리
                     jsonTestClass.dataList2.RemoveAt(idx2);
                     Destroy(tf.gameObject);
                     cardRemove = false;

                     // destroy
                     //3. 버튼을 누를때는 Interactable을 꺼줘야함.
                     //4. destroy cardObject 해주기
                     //5. 데이터 삭제 <workJsonTest 접근해서 삭제해주기>



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
                //2. 불값(canTag)이 true가 되면서 레이를 쏜다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    //3. 마우스 우클릭으로 레이를 쐈을 때 태그가 눌리면 Tag창이 꺼진다.  
                    if(hit.transform.tag.Contains("Doing"))
                    {
                        Tags.SetActive(false);
                        //4. 하나의 창(임의의 빈 오브젝트(descPlay위에)에서 생성된다.
                        taggo = Instantiate(doing, TagContent);
                        //5. 그 클릭한 값을 jsonTescClass.type 에 넣어준다. 
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

                //4. 하나의 창(임의의 빈 오브젝트(descPlay위에)에서 생성된다.
                //5. 그 클릭한 값을 jsonTescClass.type 에 넣어준다. 
            }
        }
    }


    //descPlay x 버튼 누르면 같이 버튼이 실행되는 함수

    public Text cardTexts;
    public void PostCardData()
    {

        //지금 누르면 3번째거가 나올 때 컨텐츠의 자식오브젝트 갯수를 찾아 (내가 생기기 직전에것을 찾아)
        GameObject go = Instantiate(cardObject, cardContent); //부모를 꼭 잡으시오
        //이름바꾸기 
        cardTexts = go.GetComponentInChildren<Text>();
     
        //다시 생성시키기 위해서는 
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
            //enumTagType 만들어서 데이터리스트에 넣어보기!!!
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

    //to do list 창 뜨게 하기 위해 동그란 버튼 누르면 저장되어있는 값이 카드로 생성되는 함수
    public void StartPostCardData()
    {
       //여기서 저장된 값이 카드로 생성이 됨. 
        for(int i = 0; i < jsonTestClass.dataList2.Count; i++)
        {
            
            GameObject go = Instantiate(cardObject, cardContent); //부모를 꼭 잡으시오
            //생성한 카드오브젝트를 하나에 게임오브젝트 리스트로 넣어주기 <아얘 안변하는 전역변수 / 리스트> 
            
            
            cardTexts = go.GetComponentInChildren<Text>();
            cardTexts.text = jsonTestClass.dataList2[i].cardName;
            memoUI = go.GetComponent<MemoUI>();
            memoUI.cardNameText = cardDisplay.transform.GetChild(0).GetComponent<InputField>();
            memoUI.memoText = cardDisplay.transform.GetChild(1).GetComponent<InputField>();
            memoUI.Set1(cardDisplay);
             memoUI.memoText.text = jsonTestClass.dataList2[i].desc;
            memoUI.Set(cardTexts.text, memoUI.memoText.text);
            idx.Add(go);
            //삭제버튼 누를 때 제이슨에 담았던 데이터들도 접근해서 삭제해주기!
            //내가 해주고 싶은 것: 카드 프리팹에 숫자를 넣어주는 것! 
            //idx 스크립트는 카드스크립트마다 생성이 되는 거니까 


        }

    }

    public void OffCardDisplay()
    {
        cardDisplay.SetActive(false);

    }

    /*//카드 버튼을 누르면 desc화면이 켜진다. 그거에 맞는 타이틀과 내용이 나온다. 
    public void OnOpenDisplay()
    {
        descDisplay.SetActive(true);
       //CardNameText = cardTexts[0].text;
    }*/

    //삭제하는 버튼 함수 *삭제하지 말것!
    #region cardRemoveBtn
    /*//삭제버튼에서 하나의 불값을 설정하여 true/false로 꺼주기
    public void OnRemoveCard()
    {
        cardRemove = true;
    }*/
    #endregion

    //1. descPlay에 있는 태그추가버튼 누르면 ImageTagBackground가 켜짐
    //2. 불값(canTag)이 true가 되면서 레이를 쏜다.
    //3. 마우스 우클릭으로 레이를 쐈을 때 태그가 눌리면 Tag창이 꺼진다.
    //4. 하나의 창(임의의 빈 오브젝트(descPlay위에)에서 생성된다.
    //5. 그 클릭한 값을 jsonTescClass.type 에 넣어준다. 
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
