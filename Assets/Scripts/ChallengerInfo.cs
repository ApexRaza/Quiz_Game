using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengerInfo : MonoBehaviour
{

    public string userName,id;
    public TextMeshProUGUI userNameTxt;
    public Button tick, cross;
    ChallengeFrnd ChallengeFrnd;


    
    public void Initialize(string username,ChallengeFrnd cf, string id)
    {
        userNameTxt.text = username;
        this.id = id;
        ChallengeFrnd = cf;
        tick.onClick.AddListener(Accept);
        cross.onClick.AddListener(Reject);
        StartCoroutine(AutoReject());
    }


    IEnumerator AutoReject()
    {
        yield return new WaitForSeconds(13f);
        Reject();
        this.gameObject.SetActive(false);
    }


    public void Accept()
    {
        StopCoroutine(AutoReject());
        ChallengeFrnd.AcceptChallenge(id);
    }

    public void Reject()
    {
        ChallengeFrnd.RejectChallenge(id);
    }





}
