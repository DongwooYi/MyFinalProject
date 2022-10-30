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

   public PeriodToggle periodToggle;

    void Start()
    {
        // inputTitleName ���� ���� ������ ȣ��Ǵ� �Լ� ���, �ν����� â �� ����ϴ� ���� �ڵ�� ����
        inputTitleName.onValueChanged.AddListener(OnValueChanged);

/*        // inputTitleName ���� Enter Ű ������ ȣ��Ǵ� �Լ� ���
        inputTitleName.onSubmit.AddListener(OnSubmit);*/

        // inputTitleName ���� Focusing �� ��������� �� ȣ��Ǵ� �Լ� ���
        inputTitleName.onEndEdit.AddListener(OnEndEdit);

        periodToggle.GetComponent<PeriodToggle>();
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
    public Toggle Toggle;
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
        challenge.SetParticipants("(1/" + Toggle.participantInfo[0] + ")");  // ���� �ο�
        challenge.SetRePeriod(calendarController_1._target.text + "~" + calendarController_2._target2.text);    // ���� �Ⱓ
        challenge.SetChallPeriod(calendarController_3._target3.text + "~" + calendarController_4._target4.text); // ç���� �Ⱓ
    }


    /* �ֱ� ��� ���� */
   /* bool isSelected;
    public ToggleGroup periodToggleGroup;
    string periodInfo;

    public void PeriodToggle()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<UnityEngine.UI.Toggle> toggles = periodToggleGroup.ActiveToggles();
            
        foreach(UnityEngine.UI.Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            periodInfo = toggle.name;
        }
    }

    *//* �ο� ��� ���� *//*

    public ToggleGroup participantToggleGroup;
    string participantInfo;
    public void ParticipantToggle()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<UnityEngine.UI.Toggle> toggles = participantToggleGroup.ActiveToggles();
        foreach (UnityEngine.UI.Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            participantInfo = toggle.name;
        }
    }
*/

    void Update()
    {
        
    }
}
