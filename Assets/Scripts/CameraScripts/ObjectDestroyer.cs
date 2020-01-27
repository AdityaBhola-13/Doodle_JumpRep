using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    //Destroying Every Objects going down the Screen :- Done By Aditya
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Doodle")
        {
            Destroy(collision.gameObject);
        }
    }
}
