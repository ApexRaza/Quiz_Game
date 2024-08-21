using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDisable : MonoBehaviour
{
    public float disableTime;
    private void Start()
    {
        StartCoroutine(Disable(disableTime));
    }

    public IEnumerator Disable(float time) 
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
