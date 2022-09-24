using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    bool isplayerAlive = false;

    public bool IsPlayerAlive
    {
        get {   return isplayerAlive;   }
        set {   isplayerAlive = value;  }
    }

    GameManager()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }




}
