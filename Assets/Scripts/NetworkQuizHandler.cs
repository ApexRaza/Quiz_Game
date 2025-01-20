using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Photon.Pun;
using Photon.Realtime;




[System.Serializable]
public class NetworkData
{
    public string question;
    public Image image;
    public string correctAns;
}
public class NetworkQuizHandler : MonoBehaviourPunCallbacks, IPunObservable
{
    CollectionsSO collectionSO;
    QuizManager nQuizManager;
    public QuizManager shareManager;



    [Space(2)]
    public GameObject QuestionPanel, correctAns, incorrectAns, ansObjects, loadingQ, outOflivePanel;
    public TextMeshProUGUI questionTxt, correctTxt, p1test, p2test, overText, questionCount;
    public Image questionImage;
    public GameObject[] p1Q = new GameObject[3], p2Q = new GameObject[3];

    public int p1, p2;

    public bool[] p1State = new bool[3], p2State = new bool[3];



    public List<NetworkData> networkData;// = new List<NetworkData>();

    public ProgressState progressState;
    public TImer timer;
    string[] game_ticket_id;
    int quizCount = 0;
    int rightAnswerCount = 0;
    int wrongAnswerCount = 0;
    int typeCat = 0;

    public AudioSource swipeAudioSource;



    public int bettingValue;
    public Animator blackshine;
    // Start is called before the first frame update
    void Start()
    {
        collectionSO = Resources.Load<CollectionsSO>("Scriptables/Collection");
        nQuizManager = Resources.Load<QuizManager>("Scriptables/QuizManager");
 
        nQuizManager.QuizTypesInit();

    }
    private void OnEnable()
    {
        resetStates();
    }
    void resetStates()
    {
        p1 = 0;
        p2 = 0;
        quizCount = 0;
        //  p1State
        for (int i = 0; i < p1Q.Length; i++)
        {
            p1Q[i].transform.GetChild(0).gameObject.SetActive(false);
            p1Q[i].transform.GetChild(1).gameObject.SetActive(false);
            p2Q[i].transform.GetChild(0).gameObject.SetActive(false);
            p2Q[i].transform.GetChild(1).gameObject.SetActive(false);
            networkData[i].image.gameObject.SetActive(false);
        }

        QuestionCount();
    }
    public void QuizNo()
    {
        quizCount = 0;
    }
    public void setQuizData()
    {
        // if (PhotonNetwork.LocalPlayer.ActorNumber == 1)

        // ExitGames.Client.Photon.Hashtable type = new ExitGames.Client.Photon.Hashtable();
        typeCat = Random.Range(0, 16);
        // type["Type"] = typeCat;
        Debug.LogError("i am called " + typeCat);

        // PhotonNetwork.CurrentRoom.SetCustomProperties(type);
        p1test.text = p1.ToString();
        p2test.text = p2.ToString();
        GetComponent<PhotonView>().RPC("setQuizTypeRPC", RpcTarget.All, typeCat);
        // setQuizTypeRPC();
    }
    [PunRPC]
    public void setQuizTypeRPC(int num)
    {
        //ExitGames.Client.Photon.Hashtable type = new ExitGames.Client.Photon.Hashtable();
        //typeCat = Random.Range(0, 16);
        //type["Type"] = typeCat;
        //Debug.LogError("i am called " + typeCat);
        //SetQuestionCategory(typeCat);
        //PhotonNetwork.CurrentRoom.SetCustomProperties(type);
        //p1test.text = p1.ToString();
        //p2test.text = p2.ToString();

        SetQuestionCategory(num);
        StartCoroutine(LoadingQuestionData());

    }


    IEnumerator LoadingQuestionData()
    {

        // SetQuestionCategory(PhotonNetwork.CurrentRoom.CustomProperties.GetValueOrDefault());
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            LoadQuestionData();
        else
        {
            
        }

        
        yield return new WaitForSeconds(0.3f);

        DisplayQuestion();
    }


    // setting the two different categories selected by the user
    public void SetQuestionCategory(int category)
    {
        nQuizManager.type = category;
    }

    public void CheckAns(string ans)
    {
        if (ans == nQuizManager.quizType[nQuizManager.type].quizData[num].rightAnswer)
        {

            RightAns();

            GetComponent<PhotonView>().RPC("PlaySound", RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PhotonView>().RPC(nameof(PlayerStateChange), RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PhotonView>().RPC("NextQuestion", RpcTarget.All);

            //StartCoroutine(nameof(Next));
        }
        else
        {
            WrongAnss();
            GetComponent<PhotonView>().RPC("PlaySound", RpcTarget.All, false, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PhotonView>().RPC(nameof(PlayerStateChange), RpcTarget.All, false, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PhotonView>().RPC("NextQuestion", RpcTarget.All);
        }

        GetComponent<PhotonView>().RPC(nameof(ShineEffect), RpcTarget.All);
    }

    [PunRPC]
    void ShineEffect()
    {
        blackshine.SetTrigger("Go");
    }

    [PunRPC]
    public void PlaySound(bool b, int playernb)
    {
        swipeAudioSource.Play();
        if (b)
        {
            if (playernb == 1)
            {
                p1++;
                p1test.text = p1.ToString();
            }
            if(playernb==2)
            {
                p2++;
                p2test.text = p2.ToString();
            }
        }
        else
        {
            if (playernb == 1)
            {
                p2++;
                p1test.text = p1.ToString();
            }
            if (playernb == 2)
            {
                p1++;
                p2test.text = p2.ToString();
            }
        }
        p1test.text = p1.ToString();
        p2test.text = p2.ToString();
    }

    [PunRPC]
    void PlayerStateChange(bool b,int a)
    {
        if (a== 1)
        {
            p1State[quizCount] = b;
            p2State[quizCount] = !b;
            if (b)
            {
                p1Q[quizCount].transform.GetChild(0).gameObject.SetActive(true);
                p2Q[quizCount].transform.GetChild(1).gameObject.SetActive(true);
            }
            

        }
        else
        {
            p1State[quizCount] = b;
            p2State[quizCount] = !b;
            if (b)
            {
                p1Q[quizCount].transform.GetChild(1).gameObject.SetActive(true);
                p2Q[quizCount].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

       



    }



    void DetermineWinner()
    {
        //int p1 = Convert.ToInt32(p1test.ToString());
        //int p2 = Convert.ToInt32(p2test.ToString());

        // Compare scores to find the winner
        int maxScore = Mathf.Max(p1, p2);

        if (maxScore == p1)
        {
            Debug.Log("Player 1 is the winner! " + maxScore);
            GetComponent<PhotonView>().RPC("ActivateWinnerAndLosers", RpcTarget.All, 1);


        }
        else if (maxScore == p2)
        {
            Debug.Log("Player 2 is the winner! " + maxScore);
            GetComponent<PhotonView>().RPC("ActivateWinnerAndLosers", RpcTarget.All, 2);

        }

    }
    [PunRPC]
    public void ActivateWinnerAndLosers(int num)
    {
        outOflivePanel.SetActive(true);
        if ( PhotonNetwork.LocalPlayer.ActorNumber == num)
        {
            overText.text = "YOU WIN!";
            DataBase.Dollars += bettingValue;
        }
        else
        {
            overText.text = "YOU LOSE!";
            DataBase.Dollars -= bettingValue;

        }
        resetStates();
    }





    void RightAns()
    {

        swipeAudioSource.Play();
    }

    void WrongAnss()
    {
        swipeAudioSource.Play();
    }


    void QuestionCount()
    {
        questionCount.text = (quizCount +1).ToString() + "/3";
    }


    [PunRPC]
    public void NextQuestion()
    {
        quizCount++;
        if (quizCount < 3)
        {
            QuestionCount();
            networkData[quizCount].image.gameObject.SetActive(true);
            networkData[quizCount - 1].image.gameObject.SetActive(false);
            questionTxt.text = networkData[quizCount].question.ToString();
            questionTxt.gameObject.SetActive(true);
            ansObjects.SetActive(true);
            timer.ResetTimer();
        }

        if(quizCount ==3)
        {
           // GetComponent<PhotonView>().RPC("DetermineWinner", RpcTarget.All);
            DetermineWinner();
        }
    }


    public IEnumerator Next()
    {
        //Debug.Log("run");
        rightAnswerCount++;
        correctTxt.gameObject.SetActive(true);
        correctAns.SetActive(true);
        ansObjects.SetActive(false);
        yield return new WaitForSeconds(2);
        correctTxt.gameObject.SetActive(false);
        correctAns.SetActive(false);
        quizCount++;
        if(quizCount < 3) 
        {
            networkData[quizCount].image.gameObject.SetActive(true);
            networkData[quizCount - 1].image.gameObject.SetActive(false);
            questionTxt.text = networkData[quizCount].question.ToString();
            questionTxt.gameObject.SetActive(true);
            ansObjects.SetActive(true);
            timer.ResetTimer();
        }
        else 
        {
            timer.ResetTimer();
            quizCount = 0;
            ResetState();
            QuestionPanel.SetActive(false);
        }
    }
    public IEnumerator WrongAns()
    {
        wrongAnswerCount++;
        correctTxt.gameObject.SetActive(true);
        incorrectAns.SetActive(true);
        ansObjects.SetActive(false);

        yield return new WaitForSeconds(2);
        correctTxt.gameObject.SetActive(false);
        incorrectAns.SetActive(false);
        quizCount++;
        if (quizCount < 3)
        {
            networkData[quizCount].image.gameObject.SetActive(true);
            networkData[quizCount - 1].image.gameObject.SetActive(false);
            questionTxt.text = networkData[quizCount].question.ToString();
            questionTxt.gameObject.SetActive(true);
            ansObjects.SetActive(true);
            timer.ResetTimer();
        }
        else
        {
            timer.ResetTimer();
            quizCount = 0;
            ResetState();
            QuestionPanel.SetActive(false);
        }
    }
    IEnumerator StartQuiz()
    {
        questionTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        ansObjects.SetActive(true);
        timer.ResetTimer();
    }

    void ResetState()
    {
        correctTxt.gameObject.SetActive(false);
        incorrectAns.gameObject.SetActive(false);
        questionTxt.gameObject.SetActive(false);
        ansObjects.SetActive(false);
        correctAns.SetActive(false);
    }

   
    IEnumerator LoadImage1(string imageUrl, Image img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);


            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

            // StartCoroutine(StartQuiz());
        }
    }
    IEnumerator LoadImage(string imageUrl, Image img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

          
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
          
           // StartCoroutine(StartQuiz());
        }
    }

    IEnumerator LoadingWait()
    {
        loadingQ.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        progressState.gameObject.SetActive(false);
        QuestionPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        loadingQ.SetActive(false);
    }

    int num = 0;
    public void LoadQuestionData()
    {
       // ResetState();
       ResetQ_Data();
        for (int i = 0; i < 3; i++) 
        {
            networkData[i].question = nQuizManager.quizType[nQuizManager.type].quizData[i].question.ToString();
            networkData[i].correctAns = nQuizManager.quizType[nQuizManager.type].quizData[i].correctAns.ToString();
            StartCoroutine(LoadImage(nQuizManager.quizType[nQuizManager.type].quizData[i].imageLink, networkData[i].image));
        }
      //  DisplayQuestion();
    }


    public void ResetQ_Data()
    {
        for (int i = 0; i < 3; i++)
        {
            networkData[i].question = null;
            networkData[i].correctAns = null;
            networkData[i].image.sprite = null;
           
        }
    }

    public void DisplayQuestion() 
    {
        networkData[0].image.gameObject.SetActive(true);
        questionTxt.text = networkData[0].question.ToString();
        questionTxt.gameObject.SetActive(true);
        ansObjects.SetActive(true);
       // timer.ResetTimer();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(typeCat);
        }
        else
        {
            typeCat = (int)stream.ReceiveNext();
        }
    }
}
