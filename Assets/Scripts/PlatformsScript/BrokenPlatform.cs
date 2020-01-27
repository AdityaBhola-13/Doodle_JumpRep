using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    // Broken Platform Mechanics :- Done By Aditya
    public bool isbroken;
    private void Update()
    {
        if(isbroken == true)
        {
            transform.Translate(Vector2.down * 3f * Time.deltaTime);
        }
    }
}
