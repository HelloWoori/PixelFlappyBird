using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //블럭 관련
    public float blockSpeed = 3f;
    public float blockMakeTime = 2f;
    public float blockMin = -1f;
    public float blockMax = 1f;

    public GameObject blockPrefab;

    //점수 관련
    public GameObject scorePanel;
    public GameObject[] numberImg;
    public Sprite[] number;

    //게임 시작 관련
    public GameObject startPanel;
    public GameObject player;

    private void Start()
    {
        Time.timeScale = 0; //시간이 흐르지 않는다 (1이면 정상속도, 2이면 모든 움직임이 2배속)
        DataManager.Instance.isGameOver = true; //true로 해서 블럭, 바닥 등 움직이지 않도록
        scorePanel.SetActive(false);
    }

    private void Update()
    {
        //점수 띄우기
        int decimalSpace = DataManager.Instance.score % 100;
        decimalSpace = decimalSpace / 10;
        numberImg[0].GetComponent<Image>().sprite = number[decimalSpace];

        int digitSpace = DataManager.Instance.score % 10;
        numberImg[1].GetComponent<Image>().sprite = number[digitSpace];
    }

    public void StartBtn()
    {
        Time.timeScale = 1;
        player.SetActive(true);
        DataManager.Instance.isGameOver = false;

        startPanel.SetActive(false);
    }

    public void StartGame()
    {
        scorePanel.SetActive(true);
        StartCoroutine(MakeBlock());
    }


    IEnumerator MakeBlock()
    {
        do
        {
            Instantiate
                (
                blockPrefab,
                new Vector3(5, Random.Range(blockMin, blockMax), 0),
                Quaternion.Euler(new Vector3(0, 0, 0))
                );
            yield return new WaitForSeconds(blockMakeTime);
            
        } while (!DataManager.Instance.isGameOver);
    }

}
