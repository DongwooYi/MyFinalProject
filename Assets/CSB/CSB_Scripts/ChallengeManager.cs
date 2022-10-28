using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    // ��ũ�Ѻ��� content
    public Transform content;

    // ç���� ���� ����
    public GameObject challengeFactory;

    // ���� ç���� ����
    public InputField inputTitleName;
    // ���� Button
    public Button btnConnect;

    void Start()
    {
        // inputTitleName ���� ���� ������ ȣ��Ǵ� �Լ� ���, �ν����� â �� ����ϴ� ���� �ڵ�� ����
        inputTitleName.onValueChanged.AddListener(OnValueChanged);

/*        // inputTitleName ���� Enter Ű ������ ȣ��Ǵ� �Լ� ���
        inputTitleName.onSubmit.AddListener(OnSubmit);*/

        // inputTitleName ���� Focusing �� ��������� �� ȣ��Ǵ� �Լ� ���
        inputTitleName.onEndEdit.AddListener(OnEndEdit);

        //periodToggleGroup = GetComponent<ToggleGroup>();
    }

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

    // "����" ��ư ������ ����
    public void OnClickCreateChallenge()
    {
        // ���ο� ç������ ����
        // NewChallengeList �� Btn_Challenge ������ ����
        GameObject go = Instantiate(challengeFactory, content);    // content �ڽ����� ç������ ����

        // �ֱ� text �� periodInfo �־���
        Challenge challenge = go.GetComponent<Challenge>();
        challenge.SetPeriod(periodInfo);
        challenge.SetParticipants(participantInfo);
    }


    /* �ֱ� ��� ���� */
    bool isSelected;
    public ToggleGroup periodToggleGroup;
    string periodInfo;

    public void PeriodToggle()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<Toggle> toggles = periodToggleGroup.ActiveToggles();
        foreach(Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            periodInfo = toggle.name;
        }
    }

    /* �ο� ��� ���� */

    public ToggleGroup participantToggleGroup;
    string participantInfo;
    public void ParticipantToggle()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<Toggle> toggles = participantToggleGroup.ActiveToggles();
        foreach (Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            participantInfo = toggle.name;
        }
    }


    void Update()
    {
        
    }
}
