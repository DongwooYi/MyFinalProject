using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeriodToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //periodToggleGroup = GetComponent<ToggleGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ToggleGroup periodToggleGroup;
    public void PeriodToggleFunc()
    {
        //Toggle theActiveToggle = periodToggleGroup.ActiveToggles().
        IEnumerable<Toggle> toggles = periodToggleGroup.ActiveToggles();
        foreach (Toggle toggle in toggles)
        {
            Debug.Log(toggle.name); // ���õ� ����� �̸�
            // ���õ� ����� �̸��� challenge prefab �� �Ѱ���

        }
    }
}
