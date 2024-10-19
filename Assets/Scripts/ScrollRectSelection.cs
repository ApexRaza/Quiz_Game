
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSelection : MonoBehaviour
{
    public ScrollRect scrollRect;  
  
    public float scrollSpeed = 5f;  
    public int totalItems = 10;     
    public float stopDuration = 2f; 

    private float targetPosition;
    private bool isSpinning = false;
    private float spinTime;

    void Start()
    {
        
      // StartSlotSpin();
    }
    private void OnEnable()
    {
        StartSlotSpin();
    }
    public void StartSlotSpin()
    {
        scrollRect.enabled = true;
        isSpinning = true;
        spinTime = 0f;

        
        int randomIndex = Random.Range(0, totalItems);
        targetPosition = 1f - (randomIndex / (float)(totalItems - 1));

        
        StartCoroutine(SpinSlot());
    }
    
    private IEnumerator SpinSlot()
    {
        while (spinTime < stopDuration)
        {
           
            scrollRect.verticalNormalizedPosition += Time.deltaTime * scrollSpeed;
            
           
            if ( scrollRect.verticalNormalizedPosition >= 1f)
            {
                 scrollRect.verticalNormalizedPosition = 0f;
            }

            spinTime += Time.deltaTime;
            yield return null;
        }

       
        StartCoroutine(SlowDownToStop());
    }

    private IEnumerator SlowDownToStop()
    {
        float deceleration = 1f;  
        while (Mathf.Abs( scrollRect.verticalNormalizedPosition - targetPosition) > 0.01f)
        {
             scrollRect.verticalNormalizedPosition = Mathf.Lerp( scrollRect.verticalNormalizedPosition, targetPosition, Time.deltaTime * deceleration);

            deceleration += Time.deltaTime;  
            yield return null;
        }


         scrollRect.verticalNormalizedPosition = targetPosition;
        isSpinning = false;
        scrollRect.enabled = false;
    }
}
