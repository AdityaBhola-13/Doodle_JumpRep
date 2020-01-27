using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    GameManager gManager;
    // Bullet Movement and Destroy :- Done By Aditya
    void Update()
    {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.Translate(Vector2.up * 10 * Time.deltaTime);
        Destroy(this.gameObject, 1.5f);
    }

    // For Distroying Monster By Trigger with Bullet :- Done by Aditya
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Monsters")
        {
            gManager.score = gManager.score + 50;
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
