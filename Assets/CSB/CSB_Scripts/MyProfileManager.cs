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

    GameObject backGround;

    // Run ���� �ʾƵ� ������ �󿡼� ������ �ȴٰ� ��..
    private void OnValidate()
    {
        characterArr = content.GetComponentsInChildren<Button>();  // ĳ���� ����Ʈ ��ư ����
        //characterList = characterArr.ToList();
        characterList = new List<Button>(characterArr); // �迭�� ����Ʈ�� 
    }


    void Start()
    {
        backGround = GameObject.Find("CharacterBackground");
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            //SceneManager.LoadScene("CSB_Player");
        }
    }

    // �� ��ư�� ���忡�� "�ٸ� �ֵ� ��Ӱ� ��������"
    // Ȯ��(����) ��ư ������, ĳ���� ü����
    // ���� ���� ĳ���ʹ� ���

    string myName;  // ���� �̸�
    int myIndex;    // ���� �ε���

    public void SelectedCharacter()
    {
        // ���� �̸�
        myName = EventSystem.current.currentSelectedGameObject.name;

        // ���� �ε��� �޾ƿ���
        myIndex = GameObject.Find("Content").transform.FindChild(myName).GetSiblingIndex();

        // �ٸ� ��ư�� ��Ӱ� �����
        for(int i = 0; i < characterList.Count; i++)
        {
            // �ٸ� �ֵ��� �̹����� ���İ� 90
            characterList[i].image.color = new Color(1, 1, 1, 0.5f);
            //Color color = new Color(1, 1, 1, 0.5f);
            // ���� �� �ڽ� �̶��
            if (i == myIndex) characterList[i].image.color = new Color(1, 1, 1, 1);
        }


    }

    // <<Ȯ��(����)�� ������>> CharacterChangerPanel �� ������
    // ���� ������ ���� �Ǿ� ����
    public GameObject panel;
    
    GameObject myCharacter;

    public void SaveChanges()
    {
        // ���� ���� ����
        // ���� �ε��� �� �ش��ϴ� ���� ������Ʈ(ĳ����) ã��
        myCharacter = backGround.transform.GetChild(myIndex).gameObject;

        for(int i = 0; i < backGround.transform.childCount; i++)
        {
            backGround.transform.GetChild(i).gameObject.SetActive(false);
        }
        //myCharacter.transform.position = backGround.transform.position;
        myCharacter.SetActive(true);

        // panel ����
        panel.SetActive(false);
    }
}
