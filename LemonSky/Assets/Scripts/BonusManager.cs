using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance {get; private set;}
    int collectedCoins = 0;
    void Start(){
        Instance = this;
    }

    public int GetCollectedCoins(){
        return collectedCoins;
    }


}
