using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public Image content;
    public GameObject touchGuard;
    public GameObject scrollContent;
    public GameObject scrollviewbar;
    Scrollbar sb;
    RectTransform rt;

    public Text counts;

    private void Start()
    {
        rt = scrollContent.GetComponent<RectTransform>();
        touchGuard.SetActive(false);
    }
    private void Update()
    {
        rt.sizeDelta = new Vector2(500, scrollContent.transform.childCount * (100));
        counts.text = scrollContent.transform.childCount.ToString();
        if (scrollviewbar.activeSelf)
        {
            sb = scrollviewbar.GetComponent<Scrollbar>();
            if (sb.value < 0)
            {
                sb.value = 0.1f;
                touchGuard.SetActive(true);
                CreateContent();
            }
        }
    }
    public void CreateContent()
    {
        for (int i = 0; i < 10; i++)
        {
            Image go = Instantiate(content, Vector3.zero, Quaternion.identity);
            go.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            go.transform.parent = scrollContent.transform;
        }
        touchGuard.SetActive(false);
    }

    public void UpperContent()
    {
        touchGuard.SetActive(true);
        StartCoroutine(upperScrol());
    }
    IEnumerator upperScrol()
    {
        while (true)
        {
            yield return null;
            if (sb.value >= 1) break;
            sb.value += 0.1f;
        }
        touchGuard.SetActive(false);
    }
}
