using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameManager gManager;
    Transform target;

    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(gettingPlayer());
    }
    // Camera Follow Doodle :- Done By Sumedh
    void LateUpdate()
    {
        if(gManager.isGameOver != true)
        {
            if(target != null)
            {
                Vector3 newPosition = new Vector3(transform.position.x, target.position.y, transform.position.z);
                if (target.position.y > transform.position.y)
                {
                    transform.position = newPosition;
                }
            }
            
        }
        
    }
    IEnumerator gettingPlayer()
    {
        yield return new WaitForSeconds(0.3f);
        target = GameObject.FindWithTag("Doodle").transform;
        StopCoroutine(gettingPlayer());
    }
}
