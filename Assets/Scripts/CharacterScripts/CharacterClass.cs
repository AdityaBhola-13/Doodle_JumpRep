using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterClass : MonoBehaviour
{
    GameManager gManager;
    public UIManager uiManager;
    public GameObject pickUp;
    Animator anim;
    Rigidbody2D rigi;

    public GameObject bulletPrefab;
    public GameObject canonPrefab;

    public float jumpForce = 420f;

    [SerializeField]
    public float tiltSpeed;
    float dirX;

    //[Header("Score Settings")]
    public TextMeshProUGUI score;
    float currentScore;

    bool inPower;
    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        score = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        rigi = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {

        if(gManager.isGameOver == false)
        {
            //Movement with Tilt :- Done By Sumedh
            dirX = Input.acceleration.x * tiltSpeed;

            //Checking Doodle is Going Left or Right :- Done By Aditya
            if (Input.acceleration.x < -0.05)
            {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
            if (Input.acceleration.x > 0.05)
            {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }

            //Shoot on Click or Touch :- Done By Aditya and Sumedh
            if (Input.GetMouseButtonDown(0) && inPower == false)
            {
                Shoot();
            }
            ScoreSystem(); //Calling Function For Scoring System :- Done By Aditya
        }
        

    }
    private void FixedUpdate()
    {
        //Changing Doodle X-Velocity to move Left or Right
        rigi.velocity = new Vector2(dirX, rigi.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collision with Platform , Avoiding Collision From Coming down and Jump on It, Spring Part :- Done By Aditya
        if (gManager.isGameOver == false)
        {
            Vector2 velo = rigi.velocity;
            //Jumping on Plateforms or Colliding With Plateforms
            if (collision.relativeVelocity.y >= 0f)
            {
                if (collision.gameObject.tag.EndsWith("Platform"))
                {
                    if (collision.gameObject.tag == "VisToInvisPlatform")
                    {
                        collision.gameObject.GetComponent<Animator>().Play("VisToInvisAnim");
                    }
                    if (collision.gameObject.tag == "BurstPlatform")
                    {
                        collision.gameObject.GetComponent<Animator>().Play("BurstPlatformAnim");
                    }
                    anim.Play("Doodle_Jump");
                    velo.y = jumpForce * Time.deltaTime;
                    rigi.velocity = velo;
                }
                if (collision.gameObject.tag == "Spring")
                {
                    anim.Play("Doodle_Jump");
                    collision.gameObject.GetComponent<Animator>().Play("SpringAnim");
                    velo.y = jumpForce * 2 * Time.deltaTime;
                    rigi.velocity = velo;
                }

            }
            if (collision.gameObject.tag == "Monsters")
            {
                anim.Play("Doodle_Jump");
                velo.y = jumpForce * Time.deltaTime;
                rigi.velocity = velo;
                gManager.score = gManager.score + 100;
                Destroy(collision.gameObject);
            }
            //Left To Right Spawning Machanics :- Done By Aditya
            float myXpos = transform.position.x;
            if (collision.gameObject.name == "LeftSideSpawner")
            {
                Debug.Log("Exit_Left");
                transform.position = new Vector2(-myXpos - 0.1f, transform.position.y);
            }
            if (collision.gameObject.name == "RightSideSpawner")
            {
                Debug.Log("Exit_Right");
                transform.position = new Vector2(-myXpos + 0.1f, transform.position.y);
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //When Doodle Exit from Plateform
        if (collision.gameObject.tag.EndsWith("Platform"))
        {
            anim.Play("Doodle_Idle");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Broken Platform :- Done By Aditya
        if(gManager.isGameOver == false)
        {
            if (collision.gameObject.tag == "PlatformBroken")
            {
                collision.gameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
                collision.gameObject.GetComponent<Animator>().Play("BrokenPlatformAnim");
                collision.gameObject.GetComponent<BrokenPlatform>().isbroken = true;
            }
            //Copter :- Done By Sumedh
            if (collision.gameObject.tag == "Copter" && inPower == false)
            {
                pickUp = collision.gameObject;
                rigi.gravityScale = 0;
                StartCoroutine(PowerUp());
            }
        }
        // Detecting Moster and Destroying Objects from Below The Screen :- Done By Aditya
        if(collision.gameObject.tag == "Monsters")
        {
            rigi.velocity = Vector3.down * 50 * Time.deltaTime;
            gManager.isGameOver = true;
            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        if (collision.gameObject.name == "Objects_Destroyer")
        {
            Debug.Log("GameOver");
            gManager.isGameOver = true;
            uiManager.OnGameOver();
            Destroy(gameObject);
        }
    }

    //Copter Mecahnics :- Done By Sumedh and improvised by Aditya
    IEnumerator PowerUp()
    {
        inPower = true;
        while (inPower)
        {
            Debug.Log("Loop Started");

            pickUp.GetComponent<Animator>().Play("CopterAnim");
            pickUp.transform.parent = this.transform;
            pickUp.transform.position = new Vector2(transform.position.x, transform.position.y + 0.45f);
            rigi.AddForce(Vector2.up * Time.deltaTime);
            yield return new WaitForSeconds(3f);
            inPower = false;
            rigi.gravityScale = 1;
        }
        pickUp.transform.parent = null;
        Destroy(pickUp.GetComponent<CircleCollider2D>());
        pickUp.AddComponent<Rigidbody2D>().gravityScale = 1.5f;
        yield return new WaitForSeconds(0.2f);
        pickUp.AddComponent<CircleCollider2D>().isTrigger = true;
        Debug.Log(inPower);
        StopCoroutine(PowerUp());
    }

    //Score System :- Done by Aditya
    void ScoreSystem()
    {
        if (rigi.velocity.y > 0 && transform.position.y > currentScore)
        {
            currentScore = transform.position.y;
            gManager.score = Mathf.RoundToInt(currentScore * 20);
        }
        score.text = Mathf.Round(gManager.score).ToString();
        if (gManager.score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", gManager.score);
        }
        if (gManager.score > 5000)
        {
            Debug.Log("Finish");
        }
    }

    // Shooting Mecahnics Done By Aditya and Sumedh
    void Shoot()
    {
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion targetRotation = Quaternion.Euler(0, 0, clickPos.x * -10);
        GameObject canonObject = Instantiate(canonPrefab, new Vector2(transform.position.x,transform.position.y +0.369f) , targetRotation);
        GameObject bulletObject = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y + 0.369f), targetRotation);
        canonObject.transform.parent = this.transform;
        Destroy(canonObject, 0.25f);
        anim.Play("Doodle_Shoot");
    }
}
    