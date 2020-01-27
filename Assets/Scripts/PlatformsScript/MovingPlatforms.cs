using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    // All Functionality of Platform moving Up To Down or Left To Right :- Done By Me

    float maxXPos;

    //For Left To Right PLatform;
    float nextXPos;
    float xPosA;
    float xPosB;

    //for Up To Down Platform
    float nextYPos;
    float yPosA;
    float yPosB;
    void Start()
    {
        Vector3 camPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane / 2));
        maxXPos = camPosition.x - 0.6f;

        //Initializing Values for Left To Right Platforms
        xPosA = Random.Range(0f, 1f) < 0.5f ? maxXPos : -maxXPos;
        xPosB = -xPosA;
        nextXPos = xPosA;
        

        //Initializing Values for Up To Down Platforms
        yPosA = transform.position.y;
        yPosB = transform.position.y + 2.5f;
        nextYPos = yPosB;
    }

    void Update()
    {
        if(this.gameObject.tag == "LeftToRightPlatform")
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextXPos, transform.position.y), Random.Range(1,2)*Time.deltaTime);
            float dist = Vector3.Distance(new Vector2(transform.position.x, 0), new Vector2(nextXPos, 0));
            if (dist <= 0.1f)
            {
                nextXPos = nextXPos != xPosA ? xPosA : xPosB;
            }
        }
        if(this.gameObject.tag == "UpToDownPlatform")
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, nextYPos), Random.Range(1, 2) * Time.deltaTime);
            float dist = Vector3.Distance(new Vector2(0,transform.position.y), new Vector2(0,nextYPos));
            if (dist <= 0.1f)
            {
                nextYPos = nextYPos != yPosA ? yPosA : yPosB;
            }
        }
    }
}
