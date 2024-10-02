using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUIElements : MonoBehaviour
{
    public TextMeshProUGUI[] gemsTxt, keysTxt, dollarsTxt, livesTxt;
    public TextMeshProUGUI usernameTxt;
    public AudioSource[] allSounds, allMusics;

    public GameObject bottomBar, middlePanel;
    public Slider soundSlider, musicSlider;
    private void Start()
    {
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0.7f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1f);
        Invoke(nameof(UpdateUI), 1);
    }

    private void Update()
    {
        UpdateUI();
    }

    public void SetMusic()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
        foreach (AudioSource audio in allMusics)
        {
            audio.volume = PlayerPrefs.GetFloat("Music");
        }
    }
    public void SetSound()
    {
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
        foreach (AudioSource audio in allSounds)
        {
            audio.volume = PlayerPrefs.GetFloat("Sound");
        }
    }
  

    public void UpdateUI()
    {

        foreach (TextMeshProUGUI t in gemsTxt)
        {
            t.text = DataBase.Gems.ToString();
        }

        foreach (TextMeshProUGUI t in keysTxt)
        {
            t.text = DataBase.Keys.ToString();
        }

        foreach (TextMeshProUGUI t in dollarsTxt)
        {
            t.text = DataBase.Dollars.ToString();
        }

        foreach (TextMeshProUGUI t in livesTxt)
        {
            t.text = DataBase.Lives.ToString();
        }

        usernameTxt.text = DataBase.UserName;


    }

   
  



}
