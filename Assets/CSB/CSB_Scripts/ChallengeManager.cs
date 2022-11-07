using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public Transform panelManager;
    public GameObject makeNewChallengePanel;

    GameObject player;  // �÷��̾�
    GameObject ingChallengeObj; // ���� �� Ʈ���� �߻���Ű�� ������Ʈ
    GameObject newChallengeObj; // �� ç���� Ʈ���� �߻���Ű�� ������Ʈ

    public GameObject ingChallengeFactory;  // ���� ���� ç���� ����
    public Transform ingContent;


    // ç���� ��� content
    public Transform content;

    // ç���� ���� ����
    public GameObject challengeFactory;

    // ���� ç���� ����
    public InputField inputTitleName;
    // ���� Button
    public Button btnConnect;

   public PeriodToggle periodToggle;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    // �÷��̾� ã��
        newChallengeObj = GameObject.Find("NewChallenge");  // ��ü ã��
        ingChallengeObj = GameObject.Find("IngChallenge");  // ������ Ʈ���� ��ü ã��

        /* �� ç���� ���� ���� */
        // inputTitleName ���� ���� ������ ȣ��Ǵ� �Լ� ���, �ν����� â �� ����ϴ� ���� �ڵ�� ����
        inputTitleName.onValueChanged.AddListener(OnValueChanged);

/*        // inputTitleName ���� Enter Ű ������ ȣ��Ǵ� �Լ� ���
        inputTitleName.onSubmit.AddListener(OnSubmit);*/

        // inputTitleName ���� Focusing �� ��������� �� ȣ��Ǵ� �Լ� ���
        inputTitleName.onEndEdit.AddListener(OnEndEdit);

        periodToggle.GetComponent<PeriodToggle>();
    }

    void Update()
    {
        NewChallengeList();
        IngChallengeList();
        if(Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene("ChallengeWorld");
        }
    }


    /* �� ç���� ���� ���� */
    // "����" ��ư Ȱ��ȭ
    void OnValueChanged(string s)
    {
        // ���� s�� ���̰� 0���� ũ��
        // ��ư�� �����ϰ� ����
        // �׷��� �ʴٸ�
        // ��ư�� �������� �ʰ� ����
        btnConnect.interactable = s.Length > 0;
    }

    // ��Ŀ���� ������� ��
    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);

    }
    public Toggle_CSB toggle;
    public CalendarController calendarController_1;
    public CalendarController calendarController_2;
    public CalendarController calendarController_3;
    public CalendarController calendarController_4;

    // "����" ��ư ������ ����
    public void OnClickCreateChallenge()
    {
        // ���ο� ç������ ����
        // NewChallengeList �� Btn_Challenge ������ ����
        GameObject go = Instantiate(challengeFactory, content);    // content �ڽ����� ç������ ����

        // �ֱ� text �� periodInfo �־���
        Challenge challenge = go.GetComponent<Challenge>();
        challenge.SetTitle(inputTitleName.text);    // ����
        challenge.SetPeriod(periodToggle.a);    // �ֱ� �� ����
        challenge.SetParticipants("(1/" + toggle.participantInfo[0] + ")");  // ���� �ο�
        challenge.SetRePeriod(calendarController_1._target.text + "~" + calendarController_2._target2.text);    // ���� �Ⱓ
        challenge.SetChallPeriod(calendarController_3._target3.text + "~" + calendarController_4._target4.text); // ç���� �Ⱓ

        makeNewChallengePanel.SetActive(false);

        // ���뵵 ���������
    }


    /* ç���� ���� ���� */

    // ���� ���� ç���� ��� ������
    public void NewChallengeList()
    {
        // ���� ��ü���� �Ÿ��� 1.5���� ������
        if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1.5f)
        {
            // <ç���� ����> ������Ʈ(����) �� ��, setActive
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(true);
            //newChallengeObj.transform.GetChild(0).LookAt(Camera.main.transform);    // ī�޶� ������ ���ϵ���

            // ���� ��ü���� �Ÿ��� 1 ���� ������ 
            if (Vector3.Distance(player.transform.position, newChallengeObj.transform.position) < 1f)
            {
                newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
                // ������ �� �ִ� ç���� ���(UI)�� ��  (���� �ڽĿ�����Ʈ �� �ε��� �� ��)
                panelManager.GetChild(0).gameObject.SetActive(true);
                // �ϳ��� �����ϸ� ç���� ����� ����(?)
                // ç���� ���� ��û
            }
            else
            {
                panelManager.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            newChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // ���� ���� ç������ ���� ���� Ȯ��
    // ç���� ����� ç������ ������ 

    public Transform challengeInfo;
    public void ShowGoalList()
    {
        // ��ư�� �̸�
        string btn = EventSystem.current.currentSelectedGameObject.name;

        // ��ư�� �ε��� ã��
        int idx = content.Find(btn).GetSiblingIndex();

        // �� �ε����� �ش��ϴ� ChallengeInfoManager ���ӿ�����Ʈ�� �ڽ� ���ֱ�
        challengeInfo.GetChild(idx).gameObject.SetActive(true);

    }

    // �� ç���� ����
    public void OpenNewChallenge()
    {
        // �� ç���� ���� UI ����
    }

    // ç���� ���� ��ư�� ������ ç������ <���� ç����>�� ����
    public void JoinNewChallenge()
    {
        GameObject go = Instantiate(ingChallengeFactory, ingContent);    // content �ڽ����� ç������ ����

        IngChallenge ingchallenge = go.GetComponent<IngChallenge>();
        ingchallenge.ShowTitle("새벽 수영 챌린지");    // �ϴ��� ���� ç���� ������ �����صξ��ٰ� ����
    }



    /* ���� ���� ç����(����ç����) ���� */
    // ���� ���� ç���� ��� ������
    public void IngChallengeList()
    {
        // ���� ��ü���� �Ÿ��� 1.5���� ������
        if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1.5f)
        {
            // <�� ç����> ������Ʈ(����) �� ��, setActive
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(true);
            // �ֺ��� ����Ʈ

            // ���� ��ü���� �Ÿ��� 1 ���� ������ 
            if (Vector3.Distance(player.transform.position, ingChallengeObj.transform.position) < 1f)
            {
                ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
                //ingChallengeObj.transform.GetChild(0).LookAt(Camera.main.transform);    // ī�޶� ������ ���ϵ���


                // ���� ���� ç���� ���(UI)�� ��  (���� �ڽĿ�����Ʈ �� �ε��� �� ��)
                panelManager.GetChild(3).gameObject.SetActive(true);
                // �ϳ��� �����ϸ� ç���� ����� ����
                // ����� ��ư
            }
            else
            {
                panelManager.GetChild(3).gameObject.SetActive(false);
            }
        }
        else
        {
            ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void GotoMyChallengeWorld()
    {
        ingChallengeObj.transform.GetChild(0).gameObject.SetActive(false);
        // ç���� ���� ����
        SceneManager.LoadScene("ChallengeWorld");

    }



}
