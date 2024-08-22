
using UnityEngine;
using TMPro;
using UnityEditor;
using System.Collections;


public class TextMeshProEffect : MonoBehaviour
{
    private TMP_Text m_TextComponent;

    public bool enableOutline = false;
    public float outlineWidth;
    [ColorUsage(true, true)]
    public Color32 outlineColor;
   
    public bool enableShadow = false;
    public float offsetX, offsetY, delate, softness;
    [ColorUsage(true, true)]
    public Color32 shadowColor;

     
    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();


    }

    private void OnEnable()
    {
        if (enableOutline)
            Outline();

        if (enableShadow)
            Shadow();

        //StartCoroutine(Renabling());
    }


    void Outline()
    {
        m_TextComponent.outlineWidth = outlineWidth;
        m_TextComponent.outlineColor = new Color32(outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a);
    }
    void Shadow()
    {
        Material fontMaterial = m_TextComponent.fontMaterial;

        fontMaterial.EnableKeyword("UNDERLAY_ON");
        fontMaterial.SetColor(ShaderUtilities.ID_UnderlayColor, shadowColor);
        fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, offsetX);
        fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, offsetY);
        fontMaterial.SetFloat(ShaderUtilities.ID_UnderlayDilate, delate);
        fontMaterial.SetFloat(ShaderUtilities.ID_UnderlaySoftness, softness);
       
    }

    //jst to fix a glitch 
    IEnumerator Renabling()
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        this.gameObject.SetActive(true);
    }

}




[CustomEditor(typeof(TextMeshProEffect))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
       
        var myScript = target as TextMeshProEffect;

        myScript.enableOutline = EditorGUILayout.Toggle("EnableOutline", myScript.enableOutline);
        using (new EditorGUI.DisabledScope(!myScript.enableOutline))
        {

            myScript.outlineWidth = EditorGUILayout.Slider(new GUIContent("Outline Width"), myScript.outlineWidth, 0, 1);
            myScript.outlineColor = EditorGUILayout.ColorField(new GUIContent("Outline Color"), myScript.outlineColor, true, true, true);
        }




        myScript.enableShadow = EditorGUILayout.Toggle("EnableShadow", myScript.enableShadow);
        using (new EditorGUI.DisabledScope(!myScript.enableShadow))
        {
          
            myScript.offsetX = EditorGUILayout.Slider(new GUIContent("Shadow OffsetX"), myScript.offsetX, -1, 1);
            myScript.offsetY = EditorGUILayout.Slider(new GUIContent("Shadow OffsetY"), myScript.offsetY, -1, 1);
            myScript.delate = EditorGUILayout.Slider(new GUIContent("Shadow Delate"), myScript.delate, -1, 1);
            myScript.softness = EditorGUILayout.Slider(new GUIContent("Shadow Softness"), myScript.softness, 0, 1);
            myScript.shadowColor = EditorGUILayout.ColorField(new GUIContent("Shadow Color"), myScript.shadowColor, true, true, true);
        }


        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }


    }
}