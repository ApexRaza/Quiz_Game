using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        DataSaver.Instance.dts.GradeUpgrade = (DataBase.LevelUp - 1) % 7 + 1;
        UpdateGradeDetails();
    }

    public void UpdateGradeDetails() 
    {
        try 
        {
            DataBase.GradeUpgrade = DataSaver.Instance.dts.GradeUpgrade;
            int cycleIndex = (DataSaver.Instance.dts.LevelUp - 1) / 7;
            DataSaver.Instance.dts.GradeName = gradeNames[cycleIndex % gradeNames.Length];
            DataSaver.Instance.dts.GradeColor = gradeColors[DataSaver.Instance.dts.GradeUpgrade - 1];
            DataSaver.Instance.dts.QuestionsToTreasure = questionsToTreasureValues[cycleIndex % questionsToTreasureValues.Length];

            DataBase.GradeName = DataSaver.Instance.dts.GradeName;
            DataBase.GradeColor = DataSaver.Instance.dts.GradeColor;
            DataBase.QuestionsToTreasure = DataSaver.Instance.dts.QuestionsToTreasure;
            DataSaver.Instance.savaData();
            ///....For Test....///
            DisplayGradeDetails();
        }
        catch(IndexOutOfRangeException e) 
        {
            Debug.Log("Index out of range: " + e.Message);

            DataBase.GradeUpgrade = 1;
            DataSaver.Instance.dts.GradeUpgrade = 1;
            int cycleIndex = (DataSaver.Instance.dts.LevelUp - 1) / 7;
            DataSaver.Instance.dts.GradeName = gradeNames[cycleIndex % gradeNames.Length];
            DataSaver.Instance.dts.GradeColor = gradeColors[DataSaver.Instance.dts.GradeUpgrade - 1];
            DataSaver.Instance.dts.QuestionsToTreasure = questionsToTreasureValues[cycleIndex % questionsToTreasureValues.Length];

            DataBase.GradeName = DataSaver.Instance.dts.GradeName;
            DataBase.GradeColor = DataSaver.Instance.dts.GradeColor;
            DataBase.QuestionsToTreasure = DataSaver.Instance.dts.QuestionsToTreasure;
            DataSaver.Instance.savaData();
            ///....For Test....///
            DisplayGradeDetails();
        }
        catch (Exception ex)
        {
            Debug.LogError("An unexpected error occurred: " + ex.Message);
        }
    }

    public void LevelUp() 
    {
        try 
        {
            DataBase.LevelUp++;
            DataBase.GradeUpgrade++;
            DataSaver.Instance.dts.LevelUp = DataBase.LevelUp;

            DataSaver.Instance.dts.GradeUpgrade = (DataSaver.Instance.dts.LevelUp - 1) % 7 + 1;

            int cycleIndex = (DataSaver.Instance.dts.LevelUp - 1) / 7;
            DataSaver.Instance.dts.GradeName = gradeNames[cycleIndex % gradeNames.Length];
            DataSaver.Instance.dts.QuestionsToTreasure = questionsToTreasureValues[cycleIndex % questionsToTreasureValues.Length];
            DataSaver.Instance.dts.GradeColor = gradeColors[DataSaver.Instance.dts.GradeUpgrade - 1];

            

            UpdateGradeDetails();
        }
        catch (IndexOutOfRangeException ex) 
        {
            Debug.Log("Index out of range: " + ex.Message);
            DataBase.LevelUp++;
            DataSaver.Instance.dts.GradeUpgrade = 1;
            int cycleIndex = (DataSaver.Instance.dts.LevelUp - 1) / 7;
            DataSaver.Instance.dts.GradeName = gradeNames[cycleIndex % gradeNames.Length];
            DataSaver.Instance.dts.QuestionsToTreasure = questionsToTreasureValues[cycleIndex % questionsToTreasureValues.Length];
            DataSaver.Instance.dts.GradeColor = gradeColors[0];

            UpdateGradeDetails();
        }
        catch (Exception ex)
        {
            Debug.LogError("An unexpected error occurred: " + ex.Message);
        }

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
