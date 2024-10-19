using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(StartEffect());
    }


    IEnumerator StartEffect()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        float a = 0;
        for (int i = 0; i <= 10; i++)
        {
            this.transform.localScale = new Vector3(a, 1, 1);
            yield return new WaitForSeconds(0.01f);
            a += 0.1f;
        }
        this.transform.localScale = new Vector3(1, 1, 1);
    }

}
