using System.Collections;

using UnityEngine;

public class UpDownAnim : MonoBehaviour
{
    Vector2 temp,origin;

   public float timeDelay, speed;
    void OnEnable()
    {
        StartCoroutine(UpDown());
    }
    
    IEnumerator UpDown()
    { 
        temp = transform.localPosition;
        origin = transform.localPosition;


        for (int i = 0; i < 10; i++)
        {
            temp.y += speed;
            transform.localPosition = temp;
            yield return new WaitForSeconds(timeDelay);

        }
        for (int i = 0; i < 10; i++)
        {
            temp.y -= speed;
            transform.localPosition = temp;
            yield return new WaitForSeconds(timeDelay);

        }

        transform.localPosition = origin;


        StartCoroutine(UpDown());
    }
}
