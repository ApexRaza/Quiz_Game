using UnityEngine;
using UnityEngine.Events;

public class CustomEventHandler : MonoBehaviour
{
    public UnityEvent claimBtnEvent;
    // Start is called before the first frame update
    public void onClickBtn()
    {
        claimBtnEvent.Invoke();
    }
}
