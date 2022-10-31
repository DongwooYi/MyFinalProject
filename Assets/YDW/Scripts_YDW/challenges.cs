using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class challenges : MonoBehaviour
{
    public GameObject ChallengesList;
    public Sprite sprite;
    public List<Text> texts = new List<Text>();
    public List<Image> images = new List<Image>();
    public Dictionary<string, int> dictionary = new Dictionary<string, int>();

    public PhoneCamera phoneCamera;
    void Start()
    {
        /*foreach( Text text in texts)
        {
             text.text = "1111";
             if(text.text == "111")
             {
                 foreach(Image renderer in materials)
                 {
                     renderer.GetComponent<Image>().color = Color.blue;
                 }
             }
        }*/
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].gameObject.name = i + "";
            texts[i].text = i + 1 + ": 나의 할일";
            
            if (i == 1)
            {
                //images[0].GetComponent<Image>().color = Color.blue;


            }
            if (i == 2)
            {
                //images[1].GetComponent<Image>().color = Color.red;
            }
            if (i == 3)
            {
                //images[2].GetComponent<Image>().color = Color.green;

            }
        }
        dictionary.Add(texts[0].gameObject.name, 100);
        dictionary.Add(texts[1].gameObject.name, 200);
        dictionary.Add(texts[2].gameObject.name, 300);


        foreach (KeyValuePair<string, int> i in dictionary)
        {

            Debug.Log(i.Key + "Key" + i.Value + "Value");
        }
    }


    void Update()
    {
        if (phoneCamera.isConfirm)
        {
            images[0].GetComponentInChildren<Image>().sprite = sprite;
        }
    }
    public void OnClickChallengesList()
    {
        ChallengesList.SetActive(false);
    }
}
