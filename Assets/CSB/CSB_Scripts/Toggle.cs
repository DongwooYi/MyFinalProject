using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public ToggleGroup participantToggleGroup;
     public string participantInfo;
    public void ParticipantToggleFunc()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<UnityEngine.UI.Toggle> toggles = participantToggleGroup.ActiveToggles();
        foreach (UnityEngine.UI.Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            participantInfo = toggle.name;
        }
    }

}
