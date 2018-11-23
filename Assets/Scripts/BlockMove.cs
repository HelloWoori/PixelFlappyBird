using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private void Update()
    {
        if (!GameManager.isGameOver)
        {
            transform.Translate(-1 * DataManager.Instance.moveSpeed * Time.deltaTime, 0, 0);

            if (transform.position.x <= -4f)
            {
                Destroy(gameObject);
            }
        }
    }

}
