using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable : MonoBehaviour
{
    public bool disableOnTime;
    public float disableTime;
    private void OnEnable()
    {
        if (disableOnTime)
            StartCoroutine(Disable(disableTime));
    }

    public IEnumerator Disable(float time) 
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

    public void DisableMe()
    {
       this.gameObject.SetActive(false);
    }


}
