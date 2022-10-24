using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// ��ư(ĳ����)�� �����ϸ� ���õ��� ���� ĳ���͵� �帴����
// Ȯ���� ������ ���� ĳ���Ͱ� ����
// ��� ��ư�̳� X ������ ������� �ʰ� �� ����
<<<<<<< Updated upstream

// Content �� �ڽĵ��� ĳ���� ������ �ش�
=======
// Content �� �ڽĵ��� ĳ���� ������ �ش�

//========= <<< ��� ���� >>> ���� ���� �����.. �𺧷� �� ����� ==============
>>>>>>> Stashed changes
public class MyProfileManager : MonoBehaviour
{
    public enum Character   // ĳ���� ����
    {
        Character_1,
        Character_2,
        Character_3,
        Character_4,
    }

    [SerializeField]
    private Transform content;

    [SerializeField]
    private Button[] characterArr;
    //private Slot[] slots;

    private List<Button> characterList;

<<<<<<< Updated upstream
=======
    private GameObject backGround;
    private GameObject myCharacter;

    private string prevMyName;  // ���� ���� �̸�
    private string currMyName;  // ���õ� ���� �̸�
    private int prevMyIndex = 0;    // ���� ���� �ε��� (�ϴ��� 0 ������)
    private int currMyIndex = 0;    // ���õ� ���� �ε���

>>>>>>> Stashed changes
    // Run ���� �ʾƵ� ������ �󿡼� ������ �ȴٰ� ��..
    private void OnValidate()
    {
        characterArr = content.GetComponentsInChildren<Button>();  // ĳ���� ����Ʈ ��ư ����
        //characterList = characterArr.ToList();
        characterList = new List<Button>(characterArr); // �迭�� ����Ʈ�� 
    }


    void Start()
    {

<<<<<<< Updated upstream
=======
        for (int i = 0; i < characterList.Count; i++)
        {
            // �ٸ� �ֵ��� �̹����� ���İ� 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // ���� �� �ڽ� �̶��
            if (i == currMyIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }
>>>>>>> Stashed changes
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            //SceneManager.LoadScene("CSB_Player");
        }
    }

<<<<<<< Updated upstream
=======


>>>>>>> Stashed changes
    // �� ��ư�� ���忡�� "�ٸ� �ֵ� ��Ӱ� ��������"
    // Ȯ��(����) ��ư ������, ĳ���� ü����
    // ���� ���� ĳ���ʹ� ���

    public void SelectedCharacter()
    {
<<<<<<< Updated upstream
        // ���� �̸�
        string myName = EventSystem.current.currentSelectedGameObject.name;

        // ���� �ε��� �޾ƿ���
        int myIndex = GameObject.Find("Content").transform.Find(myName).GetSiblingIndex();
=======
        prevMyIndex = currMyIndex;  // ���� ���� �ε��� ����

        // ���� �̸�
        currMyName = EventSystem.current.currentSelectedGameObject.name;

        // ���� �ε��� �޾ƿ���
        currMyIndex = GameObject.Find("Content").transform.Find(currMyName).GetSiblingIndex();
>>>>>>> Stashed changes

        // �ٸ� ��ư�� ��Ӱ� �����
        for(int i = 0; i < characterList.Count; i++)
        {
            // �ٸ� �ֵ��� �̹����� ���İ� 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
<<<<<<< Updated upstream
            // ���� �� �ڽ� �̶�� continue
            if (i == myIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }
    }

    // Ȯ��(����)�� ������ CharacterChangerPanel �� ������
    // ���� ������ ���� �Ǿ� ����
    public GameObject panel;
    public void SaveChanges()
    {
        // ���� ���� ����
=======
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
        myCharacter = backGround.transform.GetChild(currMyIndex).gameObject;

        for(int i = 0; i < backGround.transform.childCount; i++)
        {
            backGround.transform.GetChild(i).gameObject.SetActive(false);
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


        for (int i = 0; i < backGround.transform.childCount; i++)
        {
            backGround.transform.GetChild(i).gameObject.SetActive(false);

            if(i==currMyIndex) backGround.transform.GetChild(i).gameObject.SetActive(true);
        }
        //myCharacter.transform.position = backGround.transform.position;

>>>>>>> Stashed changes

        // panel ����
        panel.SetActive(false);
    }
}
