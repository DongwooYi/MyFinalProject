using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// �÷��̾ å�� ������ ���� ���� �а� �ִ� å UI �� ���
public class MyCurrBookPanel : MonoBehaviour
{
    public GameObject player;   // �÷��̾�
    public GameObject myDesk;   // å��
    public GameObject myCurrBookPanel;  // ���� �а� �ִ� å ��� UI
    public GameObject currBookInfoPanel; // ������ å �� ����

    public GameObject currBookInfoPanelFactory;

    public Transform canvas;

    public WorldManager2D worldManager;
    List<_MyBookInfo> myCurrBookList = new List<_MyBookInfo>();

    public float distance = 1f;

    // �ƹ�Ÿ �Ӹ��� ���� ���� å ���� ����

    void Start()
    {
        player = GameObject.FindWithTag("Player");


    }

    void Update()
    {
        // ���� �÷��̾ å�� ������ ����(�Ÿ� 1����)
        if(Vector3.Distance(player.transform.position, myDesk.transform.position) < distance)
        {
            // ���� �а� �ִ� å ����Ʈ �޾ƿ;��� (��� �޾ƿ;���..? / ���� �а� �ִ� å ������..?)
            myCurrBookList = worldManager.myBookList;

            // ��ư�� �� ������ �ѷ���
            // MyCurrBookPanel �� �ڽ��� �ε����� myCurrBookList �� �ε��� ���缭 �־���

            // ���� �а� �ִ� å Panel
            myCurrBookPanel.SetActive(true);
        }


    }

    // ���� �а� �ִ� ���� ���� ���� �󼼺��� �Լ�
    public void OnClickCurrBook()
    {
        // ���� ������(���� ����)
        GameObject me = EventSystem.current.currentSelectedGameObject;

        // ���� �θ�(myCurrBookPanel)�� ���° �ڽ�����
        int idx = me.transform.GetSiblingIndex();
        print("CurrButtonIdx: " + idx);

        // ���� �ε����� �ش�Ǵ� ���� å ����Ʈ�� ���� �Ѹ���
        //currBookInfoPanel.SetActive(true);

        // ����
        GameObject go = Instantiate(currBookInfoPanelFactory, canvas);
    }
}
