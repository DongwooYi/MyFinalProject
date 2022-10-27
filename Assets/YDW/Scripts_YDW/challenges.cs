using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class challenges : MonoBehaviour
{
    ChallengePanelManager challengePanelManager;
    public List<Text> texts = new List<Text>();
    public List<Image> materials = new List<Image>();
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    void Start()
    {
       foreach( Text text in texts)
       {
            text.text = "1111";
            if(text.text == "1111")
            {
                foreach(Image renderer in materials)
                {
                    renderer.GetComponent<Image>().color = Color.blue;
                }
            }
       }
    }

    
    void Update()
    {
        
    }
}
