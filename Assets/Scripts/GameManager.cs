using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DataBase.Coins);

        DataBase.Coins = 10;


        Debug.Log(DataBase.Coins);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
