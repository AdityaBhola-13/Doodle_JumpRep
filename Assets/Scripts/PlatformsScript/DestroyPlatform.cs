using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatform : MonoBehaviour
{
    // Distroy This GameObject at the end of Animation event :- Done By Aditya
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
