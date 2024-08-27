using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuizGame/Collection", fileName = "Collection")]
public class CollectionsSO : ScriptableObject
{

    public Collection collection;

    public CollectionData[] collectionData;




}

[Serializable]
public class CollectionData
{

    public string Name;
    public int ID;
    public CollectionItems[] item;
}


[Serializable]
public class CollectionItems
{
    public string Name;
    public int ID;
    public int collected;
    public int total;
    
    public Sprite Icon;
}

public enum Collection
{
    Ruby,
    Sapphire,
    Emerald
}

