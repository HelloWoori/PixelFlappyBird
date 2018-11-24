using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound("Score");
            DataManager.Instance.score += 1;
        }
    }

}
