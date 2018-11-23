using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    public GameObject backFloor;

    private void Update()
    {
        if (!DataManager.Instance.isGameOver)
        {
            if (transform.position.x <= -15f)
            {
                transform.position = new Vector3(
                backFloor.transform.position.x + 12f,
                gameObject.transform.position.y,
                0
                );
            }

            transform.Translate(-1 * DataManager.Instance.moveSpeed * Time.deltaTime, 0, 0);
        }
    }

}
