using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Login.Instance.auth.CurrentUser.IsAnonymous) 
        {
            gameObject.SetActive(true);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }

    public void TestFun() 
    {
        Login.Instance.AnomLinkFB();
    }
}
