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

    public GameObject hurtEffect;
    private bool isUsedHurtEffect = false;

    public GameObject flashPanel; //번쩍하는 패널
    public GameObject endPanel;   //게임 종료시 출력되는 패널
    private bool isShowEndPanel = false;

    //듀토리얼 이후, idle 자세 관련
    public Sprite spriteIdle;
    public RuntimeAnimatorController animController;

    public GameObject gameManager;

    private bool isClickMouseLeft = false;

    private int flag = 1; //위, 아래 flag
    private float speed = 0.4f; //왔다갔다 속도


    private void Start()
    {
        transform.position = new Vector3(-0.46f, 1.8f, 0);
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isClickMouseLeft)
        {
            if (!DataManager.Instance.isGameOver)
            {
                if (Input.GetMouseButton(0))
                {
                    isClickMouseLeft = true;

                    //애니메이터 및 컨트롤러 추가
                    gameObject.AddComponent<Animator>();
                    anim = gameObject.GetComponent<Animator>();
                    anim.runtimeAnimatorController = animController;

                    //중력 크기 지정
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

                    //블럭 생성 시작하기
                    gameManager.GetComponent<GameManager>().StartGame();
                }
            }

            //애니메이션 없음, idle 스프라이트, 중력없이 위아래로 두둥실
            if (transform.position.y > 2f)
                flag = -1;
            else if (transform.position.y <= 1.7f)
                flag = 1;

            transform.Translate(speed * flag * Vector3.up * Time.deltaTime);
        }
        else
        {
            if (!DataManager.Instance.isGameOver)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //번쩍이는 패널은 딱 한 번만 보여줌
        if (!isShowEndPanel)
        {
            isShowEndPanel = true;
            flashPanel.SetActive(true); //flashPanel출력
            StartCoroutine(HideFlashPanel());
        }

        //게임오버
        DataManager.Instance.isGameOver = true;

        //애니메이션 없애기
        Destroy(anim);

        if (collision.gameObject.CompareTag("Block"))
        {
            //부딪혔을 때 '꽝'스프라이트 출력하기
            if (!isUsedHurtEffect)
            {
                isUsedHurtEffect = true;

                hurtEffect.transform.position = collision.contacts[0].point;
                hurtEffect.SetActive(true);
                StartCoroutine(HideHurtEffect());
            }

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

            StartCoroutine(ShowEndPanel());
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

        StartCoroutine(ShowEndPanel());
    }

    void DestroyRigidBody2DForBlock()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blocks.Length; ++i)
        {
            Destroy(blocks[i].GetComponent<BoxCollider2D>());
        }
    }

    //코루틴 ---------------------------------------------------
    IEnumerator ChangeDieSprite()
    {
        yield return new WaitForSeconds(1f);

        //'깨꼬닥' 스프라이트
        spriteR.sprite = spriteDie;
        DriveIntoGround();
    }

    IEnumerator HideHurtEffect()
    {
        yield return new WaitForSeconds(0.2f);

        hurtEffect.SetActive(false);
    }

    IEnumerator HideFlashPanel()
    {
        yield return new WaitForSeconds(0.1f);

        flashPanel.SetActive(false);
    }

    IEnumerator ShowEndPanel()
    {
        yield return new WaitForSeconds(0.25f);

        endPanel.SetActive(true);
    }
}
