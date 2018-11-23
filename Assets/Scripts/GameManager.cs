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
    public GameObject[] numberImg;
    public Sprite[] number;

    private void Start()
    {
        StartCoroutine(MakeBlock());
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
