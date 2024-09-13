using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string PlayerNickName = "JackNickelsen";
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }
}
