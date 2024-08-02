using UnityEngine;
using TMPro;
using System;

public class LevelUpScript : MonoBehaviour
{
    /// <Test>
    public TextMeshProUGUI GradeColorsTxt, GradeNamesTxt, QtTValuesTxt, ProfileLvlTxt, GradeUpgradeTxt;
    /// </Test>
    public string[] gradeColors = { "White", "Yellow", "Orange", "Green", "Blue", "Brown", "Black" };
    public string[] gradeNames = { "Shoshi", "Gaku", "Kodona", "Masuta", "Dhosi", "Meishu" };
    public int[] questionsToTreasureValues = { 3, 4, 5, 6, 7, 8 };
    public int gradeUpgrade;
    public string gradeColor;
    public string gradeName;
    public int questionsToTreasure;
    public int lvlUp;


    private void Start()
    {
        lvlUp = DataBase.LevelUp;
        gradeUpgrade = DataBase.GradeUpgrade;
        gradeName = DataBase.GradeName;
        questionsToTreasure = DataBase.QuestionsToTreasure;
        gradeColor = DataBase.GradeColor;
        UpdateGradeDetails();
    }
    public void LevelUp()
    {
        if(lvlUp < 42) 
        {
            lvlUp++;
            try
            {
                gradeUpgrade++;
                //gradeUpgrade = (lvlUp - 1) % 7 + 1;

                int cycleIndex = (lvlUp - 1) / 7;
                gradeName = gradeNames[cycleIndex % gradeNames.Length];
                questionsToTreasure = questionsToTreasureValues[cycleIndex % questionsToTreasureValues.Length];
                gradeColor = gradeColors[gradeUpgrade - 1];

                UpdateGradeDetails();
            }
            catch (IndexOutOfRangeException ex)
            {
                Debug.Log("Index out of range: " + ex.Message);
                gradeUpgrade = 1;

                int cycleIndex = (lvlUp - 1) / 7;
                gradeName = gradeNames[cycleIndex % gradeNames.Length];
                questionsToTreasure = questionsToTreasureValues[cycleIndex % questionsToTreasureValues.Length];
                gradeColor = gradeColors[gradeUpgrade - 1];

                UpdateGradeDetails();
            }
        }
    }

    public void UpdateGradeDetails() 
    {
        DataBase.LevelUp = lvlUp;
        DataBase.GradeUpgrade = gradeUpgrade;
        DataBase.GradeName = gradeName;
        DataBase.QuestionsToTreasure = questionsToTreasure;
        DataBase.GradeColor = gradeColor;
        DataSaver.Instance.SaveData();
        ///....For Test....///
        DisplayGradeDetails();
    }

    public void DisplayGradeDetails()
    {
        GradeColorsTxt.text = "Grade Color: " + DataSaver.Instance.dts.GradeColor;
        GradeNamesTxt.text = "Grade Name: " + DataSaver.Instance.dts.GradeName;
        QtTValuesTxt.text = "Questions to Treasure: " + DataSaver.Instance.dts.QuestionsToTreasure;
        ProfileLvlTxt.text = "Profile Level: " + DataSaver.Instance.dts.LevelUp;
        GradeUpgradeTxt.text = "Grade Upgrade: " + DataSaver.Instance.dts.GradeUpgrade;
    }
}
