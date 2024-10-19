using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour
{


    public GameObject[] target;


    public void EnableTarget()
    {
        foreach (var obj in target)
        {
            obj.SetActive(true);
        }
    }



    private void OnEnable()
    {
        foreach (var obj in target)
        {
            obj.SetActive(false);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
