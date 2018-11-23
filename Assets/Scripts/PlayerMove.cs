using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator anim;

    private SpriteRenderer spriteR;
    public Sprite spriteHurt;
    public Sprite spriteDie;

    private GameObject[] blocks;

    public float jumpPower = 5f;
    public float smooth = 2.0f;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!GameManager.isGameOver)
        {
            if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 버튼을 누르면 y축 방향으로 올라감
            {
                GetComponent<Rigidbody2D>().velocity = new Vector3(0, jumpPower, 0);
                transform.rotation = Quaternion.Euler(0, 0, 60f);
            }

            Quaternion target = Quaternion.Euler(0, 0, -90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth); //Slerp: 구면 선형 보간법(spherical linear interpolation)의 약어
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //게임오버
        GameManager.isGameOver = true;
        Destroy(anim);

        if (collision.gameObject.CompareTag("Block"))
        {
            //위 아래 블럭의 강체 제거
            DestroyRigidBody2DForBlock();

            //'깜짝 놀라는' 스프라이트로 변경
            spriteR.sprite = spriteHurt;

            //먼가 살짝 위로 올라가게 하고 싶었음
            transform.position = new Vector3(
                transform.position.x + 0.1f,
                Mathf.Lerp(transform.position.y, transform.position.y + 8f, Time.deltaTime * smooth),
                0);

            //일정시간 뒤 '깨꼬닥' 스프라이트
            StartCoroutine(ChangeDieSprite());
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());

            //'깨꼬닥' 스프라이트
            spriteR.sprite = spriteDie;
            transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
    }

    void DriveIntoGround()
    {
        //-90도 회전 후 바닥에 쳐박는다
        transform.rotation = Quaternion.Euler(0, 0, -90f);
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, -2f, Time.deltaTime * smooth),
            0);
    }

    void DestroyRigidBody2DForBlock()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blocks.Length; ++i)
        {
            Destroy(blocks[i].GetComponent<BoxCollider2D>());
        }
    }

    IEnumerator ChangeDieSprite()
    {
        yield return new WaitForSeconds(1f);

        //'깨꼬닥' 스프라이트
        spriteR.sprite = spriteDie;
        DriveIntoGround();
    }
}
