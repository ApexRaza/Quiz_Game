using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContenSpacing : MonoBehaviour
{
    public VerticalLayoutGroup VerticalLayoutGroup;
    // Start is called before the first frame update
    void Start()
    {

        VerticalLayoutGroup = GetComponent<VerticalLayoutGroup>();

        if (DataBase.LevelUp == 1)
        {
            VerticalLayoutGroup.spacing = -500f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
