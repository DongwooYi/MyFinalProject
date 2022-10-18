using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // ID InputField
    public InputField inputID;

    // Enter Button
    public Button enterButton;

    void Start()
    {
        inputID.onValueChanged.AddListener(OnValueChanged);

        // inpunNickName ���� Enter Ű ������ ȣ��Ǵ� �Լ� ���
        inputID.onSubmit.AddListener(OnSubmit);

        // inputNickName ���� Focusing �� ��������� �� ȣ��Ǵ� �Լ� ���
        inputID.onEndEdit.AddListener(OnEndEdit);

    }

    void Update()
    {
/*        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("CSB_Player");
        }*/
    }

    void OnValueChanged(string str)
    {
        enterButton.interactable = str.Length > 0;
    }

    void OnSubmit(string str)
    {
        if (str.Length > 0)
        {
            OnClickConnect();
        }
    }

    void OnEndEdit(string s)
    {
        print("OnEndEdit : " + s);

    }

    public void OnClickConnect()
    {
        SceneManager.LoadScene("CSB_MyProfile");

    }

}
