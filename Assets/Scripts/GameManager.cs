﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject tap;

    private void Start()
    {
        if (!DataManager.Instance.isRestart)
        {
            SoundManager.Instance.PlaySound("LongBGM");
            DataManager.Instance.isGameOver = true; //true로 해서 블럭, 바닥 등 움직이지 않도록
            //Time.timeScale = 0; //시간이 흐르지 않는다 (1이면 정상속도, 2이면 모든 움직임이 2배속)

            startPanel.SetActive(true);
            scorePanel.SetActive(false);
      
            tap.SetActive(true);
            StartCoroutine(ShowTab());
        }
        else //재시작인 경우, 튜토리얼 생략 후 바로 게임 시작
        {
            StartBtn();
        }
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

    //게임 시작
    public void StartBtn()
    {
        SoundManager.Instance.StopSound("LongBGM");
        StartCoroutine(PlayBGM());

        Time.timeScale = 1;
        player.SetActive(true);
        DataManager.Instance.isGameOver = false;

        startPanel.SetActive(false);

        StopCoroutine(ShowTab());
        tap.SetActive(false);
    }

    public void StartBtnWithPlayBtnSound()
    {
        SoundManager.Instance.PlaySound("Btn");
        StartBtn();
    }

    //게임 재시작 버튼
    public void ReStartBtn()
    {
        SoundManager.Instance.PlaySound("Btn");

        DataManager.Instance.isGameOver = false;
        DataManager.Instance.score = 0;
        DataManager.Instance.isRestart = true;

        SceneManager.LoadScene("Main");
    }

    //튜토리얼 이후에 점수 계산 및 블럭생성
    public void StartGame()
    {
        scorePanel.SetActive(true);
        StartCoroutine(MakeBlock());
    }

    //코루틴 -----------------------------------------------------------
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

    IEnumerator ShowTab()
    {
        do
        {
            if (tap.activeSelf)
                tap.SetActive(false);
            else
                tap.SetActive(true);

            yield return new WaitForSeconds(0.5f);

        } while (true);
    }

    IEnumerator PlayBGM()
    {
        yield return new WaitForSeconds(1f);

        SoundManager.Instance.PlaySound("ShortBGM");
    }
}
