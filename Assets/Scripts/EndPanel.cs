using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    public Text scoreText;
    public Text bestScoreText;

    public GameObject newImg;
    public GameObject medal;

    public Sprite goldMedal;
    public Sprite silverMedal;
    public Sprite bronzeMedal;

    public GameObject scorePanel;

    private void OnEnable()
    {
        scorePanel.SetActive(false);

        //베스트 스코어 갱신
        if (DataManager.Instance.score > DataManager.Instance.bestScore)
        {
            DataManager.Instance.bestScore = DataManager.Instance.score;
            newImg.SetActive(true);
        }
        else
        {
            newImg.SetActive(false);
        }

        //스코어 출력
        scoreText.text = DataManager.Instance.score.ToString();
        bestScoreText.text = DataManager.Instance.bestScore.ToString();

        //메달 출력
        if (DataManager.Instance.score >= 10)
        {
            medal.GetComponent<Image>().sprite = goldMedal;
        }
        else if (DataManager.Instance.score >= 5)
        {
            medal.GetComponent<Image>().sprite = silverMedal;
        }
        else
        {
            medal.GetComponent<Image>().sprite = bronzeMedal;
        }
    }

}
