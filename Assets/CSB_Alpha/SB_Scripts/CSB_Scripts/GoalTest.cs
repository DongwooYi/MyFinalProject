using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTest : MonoBehaviour
{
    bool isOpen;
    public GameObject goalPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ��ǥ ����Ʈ �����ٴݾҴ� ����
    public void GoalList()
    {
        if (isOpen)
        {
            goalPanel.SetActive(false);
            isOpen = false;
        }
        else
        {
            goalPanel.SetActive(true);
            isOpen = true;
        }
    }
}
