using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private MaterialPropertyBlock mpb;
    private Renderer renderer;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        renderer = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();

        // Set an initial outline color using Material Property Block
        ChangeOutlineColor(Color.red); // Example: Set outline color to red
    }

    public void ChangeOutlineColor(Color newColor)
    {
        // Apply the outline color using Material Property Block
        renderer.GetPropertyBlock(mpb);
        mpb.SetColor("_OutlineColor", newColor);
        renderer.SetPropertyBlock(mpb);
    }

    public void SetOutlineWidth(float width)
    {
        // Apply the outline width using Material Property Block
        renderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_OutlineWidth", width);
        renderer.SetPropertyBlock(mpb);
    }
}
