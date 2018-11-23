using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;

    public float blockSpeed = 3f;
    public float blockMakeTime = 2f;
    public float blockMin = -1f;
    public float blockMax = 1f;

    public GameObject blockPrefab;

    private void Start()
    {
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
            
        } while (!GameManager.isGameOver);
    }

}
