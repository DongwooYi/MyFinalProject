using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// 버튼(캐릭터)을 선택하면 선택되지 않은 캐릭터들 흐릿해짐
// 확인을 눌러야 마이 캐릭터가 변함
// 취소 버튼이나 X 누르면 저장되지 않고 걍 나감
// Content 의 자식들이 캐릭터 종류에 해당

//========= <<< 취소 버그 >>> 그지 같지 잡았음.. 디벨롭 해 수빈아 ==============
public class MyProfileManager : MonoBehaviour
{
    public GameObject panel;
    
    [SerializeField]
    private Transform content;

    [SerializeField]
    private Button[] characterArr;
    //private Slot[] slots;

    private List<Button> characterList;

    private GameObject player;
    private GameObject myCharacter;

    private string prevMyName;  // 이전 나의 이름
    private string currMyName;  // 선택된 나의 이름
    private int prevMyIndex = 0;    // 이전 나의 인덱스 (일단은 0 번으로)
    private int currMyIndex = 0;    // 선택된 나의 인덱스

    // Run 하지 않아도 에디터 상에서 실행이 된다고 함..
    private void OnValidate()
    {
        characterArr = content.GetComponentsInChildren<Button>();  // 캐릭터 리스트 버튼 저장
        //characterList = characterArr.ToList();
        characterList = new List<Button>(characterArr); // 배열을 리스트로 
    }


    void Start()
    {
        player = GameObject.Find("CharacterBackground");

        for (int i = 0; i < characterList.Count; i++)
        {
            // 다른 애들의 이미지의 알파값 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // 만약 나 자신 이라면
            if (i == currMyIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            //SceneManager.LoadScene("CSB_Player");
        }
    }

    public void GoToTheWorld()
    {
        SceneManager.LoadScene("CSB_YDW_Combine");
    }



    // 각 버튼들 입장에서 "다른 애들 어둡게 만들어야지"
    // 확인(저장) 버튼 누르면, 캐릭터 체인지
    // 현재 나의 캐릭터는 밝게

    public void SelectedCharacter()
    {
        prevMyIndex = currMyIndex;  // 이전 나의 인덱스 저장

        // 나의 이름
        currMyName = EventSystem.current.currentSelectedGameObject.name;

        // 나의 인덱스 받아오기
        currMyIndex = GameObject.Find("Content").transform.FindChild(currMyName).GetSiblingIndex();

        // 다른 버튼들 어둡게 만들기
        for(int i = 0; i < characterList.Count; i++)
        {
            // 다른 애들의 이미지의 알파값 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // 만약 나 자신 이라면
            if (i == currMyIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }
    }

    // <<확인(저장)을 누르면>> CharacterChangerPanel 이 꺼지고
    // 변경 내용이 저장 되어 있음
    public void SaveChanges()
    {
        // 변경 내용 저장
        // 나의 인덱스 에 해당하는 게임 오브젝트(캐릭터) 찾기
        myCharacter = player.transform.GetChild(currMyIndex).gameObject;

        for(int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);
        }
        //myCharacter.transform.position = backGround.transform.position;
        myCharacter.SetActive(true);

        // panel 끄기
        panel.SetActive(false);
    }

    // X or 취소 버튼
    // 확인(저장) 누르지 않으면 이전 상태로 돌아간 후 CharacterChangerPanel 끄기
    public void DontSave()
    {
        currMyIndex = prevMyIndex;

        // 다른 버튼들 어둡게 만들기
        for (int i = 0; i < characterList.Count; i++)
        {
            // 다른 애들의 이미지의 알파값 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // 만약 나 자신 이라면
            if (i == currMyIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }


        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);

            if(i==currMyIndex) player.transform.GetChild(i).gameObject.SetActive(true);
        }
        //myCharacter.transform.position = backGround.transform.position;


        // panel 끄기
        panel.SetActive(false);
    }
}
