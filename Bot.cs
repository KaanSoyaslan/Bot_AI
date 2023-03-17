using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public GameObject Ball;
    public float MoveSpeed;
    public float JumpForce;
    public Rigidbody2D rb;
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isTouchingGround;
    bool animGOon = false;


    public GameObject Target;
    public float stoppingDistance = 0f;
    public float farDistance = 10f;

    bool Botreflex = false;

    public float LastXpoint = 0f;

    public static bool isBotActive = false;

    public bool GoLeft = false;
    public bool GoRight = false;


    public int[] SuperList;
    public GameObject[] SuperObjects;

    bool WaitASecForSuper = false;

    public float BotReflexTime;

    bool Super0Activate;
    bool Super1Activate;
    bool Super2Activate;

    void Start()
    {
        Super0Activate = false;
        Super1Activate = false;
        Super2Activate = false;

        isBotActive = false;
        LastXpoint = gameObject.transform.position.x;

        for (int i = 0; i < SuperList.Length; i++) //super deðerleri 999 landý
        {
            SuperList[i] = 999; //ilk deðer 999
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBotActive && !Player.isBUMBUM)
        {
            if (!GoLeft && !GoRight) //kenarlardamý ?
            {
                if (GameObject.FindGameObjectWithTag("Super") != null)
                {
                    if (!Botreflex)
                    {
                        StartCoroutine(BotREFLEX());
                    }


                    if (Vector2.Distance(transform.position, Target.transform.position) > 0 && Vector2.Distance(transform.position, Target.transform.position) < farDistance && Target != null) //yürüme
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(Target.transform.position.x, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
                    }
                    if (Target.transform.position.y - transform.position.y > 4f && isTouchingGround && Target != null)
                    {
                        rb.velocity = new Vector2(0, 0);
                        rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                    }

                }
                else if (GameObject.FindGameObjectWithTag("Player") != null) //playera hareket etme
                {
                    Target = GameObject.FindGameObjectWithTag("Player");

                    if (Vector2.Distance(transform.position, Target.transform.position) > stoppingDistance && Vector2.Distance(transform.position, Target.transform.position) < farDistance && Target != null
                        && ((Mathf.Abs(Target.transform.position.x - transform.position.x) > 3 && !(Mathf.Abs(Target.transform.position.y - transform.position.y) > 1))) || (Mathf.Abs(Target.transform.position.x - transform.position.x) > 0 && !(Mathf.Abs(Target.transform.position.y - transform.position.y) > 1))) //yürüme
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(Target.transform.position.x, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
                    }
                    if (Target.transform.position.y - transform.position.y > 4f && isTouchingGround && Target != null)
                    {
                        rb.velocity = new Vector2(0, 0);
                        rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                    }
                }

            }//kararlar
            else if (GoLeft && !GoRight)
            {
                var move1 = new Vector3(1, 0, 0);
                transform.position -= move1 * MoveSpeed * Time.deltaTime;
            }
            else if (!GoLeft && GoRight)
            {
                var move1 = new Vector3(1, 0, 0);
                transform.position += move1 * MoveSpeed * Time.deltaTime;
            }

            isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer); //yer kontrolü

            if (rb.velocity.y < 0.1f && rb.velocity.y > -0.1f)
            {
                //normal
                gameObject.transform.localScale = new Vector2(1f, 1f);

            }
            if (rb.velocity.y > 0.1f)
            {
                //jump
                gameObject.transform.localScale = new Vector2(0.8f, 1f);


            }
            else if (rb.velocity.y < -0.1f)
            {
                //jumpdown
                gameObject.transform.localScale = new Vector2(1f, 0.9f);
            }


            if (LastXpoint > gameObject.transform.position.x)
            {
                Ball.transform.Rotate(0f, 0f, 1.5f);
            }
            else if (LastXpoint < gameObject.transform.position.x)
            {
                Ball.transform.Rotate(0f, 0f, -1.5f);
            }
            LastXpoint = gameObject.transform.position.x;


        }

        if (isBotActive && SuperList[0] != 999 && GameObject.FindGameObjectWithTag("Player") != null && !Super0Activate) //eleman boþ deðilse ve 0. butona basýlýrsa
        {
            Target = GameObject.FindGameObjectWithTag("Player");
            if (SuperList[0] < 2 && Target.transform.position.x - transform.position.x > 0 && (Mathf.Abs(Target.transform.position.y - transform.position.y) < 1)) //sað
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[0]], new Vector2(gameObject.transform.position.x + 2, gameObject.transform.position.y), Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[0] = 999;//reset
            }
            else if (SuperList[0] < 4 && SuperList[0] > 1 && Target.transform.position.x - transform.position.x < 0 && (Mathf.Abs(Target.transform.position.y - transform.position.y) < 1)) //sol
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[0]], new Vector2(gameObject.transform.position.x - 2, gameObject.transform.position.y), Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[0] = 999;//reset
            }
            else if (SuperList[0] < 6 && SuperList[0] > 3 && Vector2.Distance(Target.transform.position, gameObject.transform.position) < 5)//dalga
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[0]], gameObject.transform.position, Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[0] = 999;//reset
            }

        }
        if (isBotActive && SuperList[1] != 999 && GameObject.FindGameObjectWithTag("Player") != null && !Super1Activate) //eleman boþ deðilse ve 1. butona basýlýrsa
        {
            Target = GameObject.FindGameObjectWithTag("Player");
            if (SuperList[1] < 2 && Target.transform.position.x - transform.position.x > 0 && (Mathf.Abs(Target.transform.position.y - transform.position.y) < 1)) //sað
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[1]], new Vector2(gameObject.transform.position.x + 2, gameObject.transform.position.y), Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[1] = 999;//reset
            }
            else if (SuperList[1] < 4 && SuperList[1] > 1 && Target.transform.position.x - transform.position.x < 0 && (Mathf.Abs(Target.transform.position.y - transform.position.y) < 1)) //sol
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[1]], new Vector2(gameObject.transform.position.x - 2, gameObject.transform.position.y), Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[1] = 999;//reset
            }
            else if (SuperList[1] < 6 && SuperList[1] > 3 && Vector2.Distance(Target.transform.position, gameObject.transform.position) < 5)//dalga
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[1]], gameObject.transform.position, Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[1] = 999;//reset
            }

        }
        if (isBotActive && SuperList[2] != 999 && GameObject.FindGameObjectWithTag("Player") != null && !Super2Activate) //eleman boþ deðilse ve 2. butona basýlýrsa
        {
            Target = GameObject.FindGameObjectWithTag("Player");
            if (SuperList[2] < 2 && Target.transform.position.x - transform.position.x > 0 && (Mathf.Abs(Target.transform.position.y - transform.position.y) < 1)) //sað
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[2]], new Vector2(gameObject.transform.position.x + 2, gameObject.transform.position.y), Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[2] = 999; //reset
            }
            else if (SuperList[2] < 4 && SuperList[2] > 1 && Target.transform.position.x - transform.position.x < 0 && (Mathf.Abs(Target.transform.position.y - transform.position.y) < 1)) //sol
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[2]], new Vector2(gameObject.transform.position.x - 2, gameObject.transform.position.y), Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[2] = 999; //reset
            }
            else if (SuperList[2] < 6 && SuperList[2] > 3 && Vector2.Distance(Target.transform.position, gameObject.transform.position) < 4)//dalga
            {
                GameObject Super = Instantiate(SuperObjects[SuperList[2]], gameObject.transform.position, Quaternion.identity);
                Super.GetComponent<SuperN>().Atýcý = 1; SoundManager.PlaySound("Super");
                SuperList[2] = 999;//reset 
            }

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !animGOon)
        {
            //Debug.Log("YereDeðdi");
            StartCoroutine(BallDownAnim());
        }

        if (collision.gameObject.tag == "ArenaDown")
        {
            GameManager.WhoIsWýn = 1;
            gameObject.SetActive(false);
            GameManager.isEndComing = 2;
        }

        if (collision.gameObject.tag == "Super" && !WaitASecForSuper)
        {
            SoundManager.PlaySound("SuperTake");
            StartCoroutine(WaitForSecForSuperWaiter());
            Destroy(collision.gameObject);
            int randomSUPER = Random.Range(0, 6);
            if (SuperList[0] == 999)
            {
                StartCoroutine(WaitForSuperWaiter0());
                SuperList[0] = randomSUPER;  //0. slotn boþ ve bu slota atandý
            }
            else if (SuperList[1] == 999)
            {
                StartCoroutine(WaitForSuperWaiter1());
                SuperList[1] = randomSUPER;   //1. slotn boþ ve bu slota atandý
            }
            else if (SuperList[2] == 999)
            {
                StartCoroutine(WaitForSuperWaiter2());
                SuperList[2] = randomSUPER;   //2. slotn boþ ve bu slota atandý
            }


        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ArenaLeft")
        {
            GoRight = true;
        }

        if (collision.gameObject.tag == "ArenaRight")
        {
            GoLeft = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ArenaLeft")
        {
            GoRight = false;
        }
        if (collision.gameObject.tag == "ArenaRight")
        {
            GoLeft = false;
        }

    }
    IEnumerator BallDownAnim()
    {
        animGOon = true;
        gameObject.transform.localScale = new Vector2(1f, 0.5f);
        yield return new WaitForSeconds(0.25f);
        gameObject.transform.localScale = new Vector2(1f, 1f);
        animGOon = false;
    }

    IEnumerator BotREFLEX()
    {
        Botreflex = true;
        float rnd = Random.Range(1.25f, 1.60f);
        yield return new WaitForSeconds(BotReflexTime);
        Target = GameObject.FindGameObjectWithTag("Super");
        Botreflex = false;

    }

    IEnumerator WaitForSecForSuperWaiter()
    {
        WaitASecForSuper = true;
        yield return new WaitForSeconds(0.2f);
        WaitASecForSuper = false;
    }

    IEnumerator WaitForSuperWaiter0()
    {
        Super0Activate = true;
        yield return new WaitForSeconds(0.5f);
        Super0Activate = false;
    }
    IEnumerator WaitForSuperWaiter1()
    {
        Super1Activate = true;
        yield return new WaitForSeconds(0.5f);
        Super1Activate = false;
    }
    IEnumerator WaitForSuperWaiter2()
    {
        Super2Activate = true;
        yield return new WaitForSeconds(0.5f);
        Super2Activate = false;
    }
}
