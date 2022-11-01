using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HighScoreTable : MonoBehaviour
{
  public UnityEngine.UI.Toggle toggle;
    public Transform entryContainer;
    public Transform entryTemplate;
    List<HighScoreEntry> highScoreEntriesList;
    public List<Transform> highScoreEntryTransformList;
    public float templateHeight = 70;
    public GameObject leaderBoard;
    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
        highScoreEntriesList = new List<HighScoreEntry>()
        {
new HighScoreEntry{score = 10, name ="닉네임"},
//new HighScoreEntry{score = 10000, name ="Park"},
//new HighScoreEntry{score = 1000, name ="Kwon"},
//new HighScoreEntry{score = 100, name ="Kim"},


        };

        // sort entryList by Score
        for (int i = 0; i < highScoreEntriesList.Count; i++)
        {
            for (int j = i + 1; j < highScoreEntriesList.Count; j++)
            {
                if (highScoreEntriesList[j].score > highScoreEntriesList[i].score)
                {
                    //Swap
                    HighScoreEntry tmp = highScoreEntriesList[i];
                    highScoreEntriesList[i] = highScoreEntriesList[j];
                    highScoreEntriesList[j] = tmp;
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScoreEntriesList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }
    }
    private void Start()
    {
      
        toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
        leaderBoard.SetActive(false);
    }
    void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformsList)
    {

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryrectTransform = entryTransform.GetComponent<RectTransform>();
        entryrectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformsList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = transformsList.Count + 1;
        string ranking;
        switch (rank)
        {
            default: ranking = rank + "등"; break;
            case 1: ranking = "1등"; break;
            case 2: ranking = "2등"; break;
            case 3: ranking = "3등"; break;
        }
        entryTransform.Find("PostText").GetComponent<Text>().text = ranking;
        int score = highScoreEntry.score;
        entryTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString() + " ㎡";
        string name = highScoreEntry.name;
        entryTransform.Find("NameText").GetComponent<Text>().text = name;
        if (rank == 1)
        {
            entryTransform.Find("PostText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("ScoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("NameText").GetComponent<Text>().color = Color.green;

        }

        transformsList.Add(entryrectTransform);
    }
    public void ToggleValueChanged(UnityEngine.UI.Toggle change)
    {
        if(change.isOn)
        {

        leaderBoard.SetActive(true);
        }
        else
        {
            leaderBoard.SetActive(false);

        }
    }
}


/// <summary>
/// 
/// </summary>
[System.Serializable]
class HighScoreEntry
{
    public int score;
    public string name;
}