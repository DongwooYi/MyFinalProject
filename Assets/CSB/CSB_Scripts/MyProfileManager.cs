using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// ��ư(ĳ����)�� �����ϸ� ���õ��� ���� ĳ���͵� �帴����
// Ȯ���� ������ ���� ĳ���Ͱ� ����
// ��� ��ư�̳� X ������ ������� �ʰ� �� ����
// Content �� �ڽĵ��� ĳ���� ������ �ش�

//========= <<< ��� ���� >>> ���� ���� �����.. �𺧷� �� ����� ==============
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

    private string prevMyName;  // ���� ���� �̸�
    private string currMyName;  // ���õ� ���� �̸�
    private int prevMyIndex = 0;    // ���� ���� �ε��� (�ϴ��� 0 ������)
    private int currMyIndex = 0;    // ���õ� ���� �ε���

    // Run ���� �ʾƵ� ������ �󿡼� ������ �ȴٰ� ��..
    private void OnValidate()
    {
        characterArr = content.GetComponentsInChildren<Button>();  // ĳ���� ����Ʈ ��ư ����
        //characterList = characterArr.ToList();
        characterList = new List<Button>(characterArr); // �迭�� ����Ʈ�� 
    }


    void Start()
    {
        player = GameObject.Find("Player");

        for (int i = 0; i < characterList.Count; i++)
        {
            // �ٸ� �ֵ��� �̹����� ���İ� 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // ���� �� �ڽ� �̶��
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
        SceneManager.LoadScene("PlaygroundDemo");
    }



    // �� ��ư�� ���忡�� "�ٸ� �ֵ� ��Ӱ� ��������"
    // Ȯ��(����) ��ư ������, ĳ���� ü����
    // ���� ���� ĳ���ʹ� ���

    public void SelectedCharacter()
    {
        prevMyIndex = currMyIndex;  // ���� ���� �ε��� ����

        // ���� �̸�
        currMyName = EventSystem.current.currentSelectedGameObject.name;

        // ���� �ε��� �޾ƿ���
        currMyIndex = GameObject.Find("Content").transform.Find(currMyName).GetSiblingIndex();

        // �ٸ� ��ư�� ��Ӱ� �����
        for(int i = 0; i < characterList.Count; i++)
        {
            // �ٸ� �ֵ��� �̹����� ���İ� 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // ���� �� �ڽ� �̶��
            if (i == currMyIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }
    }

    // <<Ȯ��(����)�� ������>> CharacterChangerPanel �� ������
    // ���� ������ ���� �Ǿ� ����
    public void SaveChanges()
    {
        // ���� ���� ����
        // ���� �ε��� �� �ش��ϴ� ���� ������Ʈ(ĳ����) ã��
        myCharacter = player.transform.GetChild(currMyIndex).gameObject;

        for(int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);
        }
        //myCharacter.transform.position = backGround.transform.position;
        myCharacter.SetActive(true);

        // panel ����
        panel.SetActive(false);
    }

    // X or ��� ��ư
    // Ȯ��(����) ������ ������ ���� ���·� ���ư� �� CharacterChangerPanel ����
    public void DontSave()
    {
        currMyIndex = prevMyIndex;

        // �ٸ� ��ư�� ��Ӱ� �����
        for (int i = 0; i < characterList.Count; i++)
        {
            // �ٸ� �ֵ��� �̹����� ���İ� 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // ���� �� �ڽ� �̶��
            if (i == currMyIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }


        for (int i = 0; i < player.transform.childCount; i++)
        {
            player.transform.GetChild(i).gameObject.SetActive(false);

            if(i==currMyIndex) player.transform.GetChild(i).gameObject.SetActive(true);
        }
        //myCharacter.transform.position = backGround.transform.position;


        // panel ����
        panel.SetActive(false);
    }
}
