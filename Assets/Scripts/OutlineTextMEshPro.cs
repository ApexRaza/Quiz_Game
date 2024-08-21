using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutlineTextMEshPro : MonoBehaviour
{
    private TMP_Text m_TextComponent;
    

    public float width;
    [Space(10)]

    [ColorUsage(true, true)]
    public Color32 color;

    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        
        m_TextComponent.outlineWidth = width;
        m_TextComponent.outlineColor =   new Color32 (color.r,color.g,color.b,color.a);
    }
}
