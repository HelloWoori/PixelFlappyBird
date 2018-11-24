using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;


    public static DataManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool isGameOver = true;
    public int score = 0;
    public int bestScore = 0;

    //블럭, 바닥 움직임 속도
    public float moveSpeed = 3f;

    //재시작 여부 체크
    public bool isRestart = false;
}
