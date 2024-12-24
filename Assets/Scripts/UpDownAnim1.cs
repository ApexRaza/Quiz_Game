using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpDownAnim1 : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 originalPos, tempPos;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        tempPos = rectTransform.anchoredPosition;
        originalPos = rectTransform.anchoredPosition;
    }

    void OnEnable()
    {

       // StartCoroutine(Anim());
    }


    IEnumerator Anim()
    {
        for (int i = 0; i < 10; i++)
        {
            tempPos.y += 5f;            
            rectTransform.anchoredPosition = tempPos;
            yield return new WaitForSeconds(0.1f);
            tempPos.y -= 5f;
            rectTransform.anchoredPosition = tempPos;

        }
       // rectTransform.anchoredPosition = originalPos;
        StartCoroutine(Anim());
    }


    private void OnDisable()
    {
        StopCoroutine(Anim());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
