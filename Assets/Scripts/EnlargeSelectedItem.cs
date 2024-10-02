using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnlargeSelectedItemWithLayout : MonoBehaviour
{
    public ScrollRect scrollRect;      // Reference to the ScrollRect component
    public RectTransform content;      // The content holder of the ScrollView
    public float maxScale = 1f;        // Maximum scale for the selected item
    public float minScale = 0.8f;      // Minimum scale for the non-selected items
    public float scaleFactor = 3f;     // Factor to control how fast the scale changes
    public VerticalLayoutGroup layoutGroup;  // Reference to the Vertical Layout Group

    private List<RectTransform> items = new List<RectTransform>();

    void Start()
    {
        // Get all child items of the content (assuming each item is a direct child of the content)
        foreach (Transform item in content)
        {
            items.Add(item.GetComponent<RectTransform>());
        }
    }

    void Update()
    {
        // Call this function every frame to adjust the sizes based on scrolling
        UpdateItemScales();
        // Force the layout group to update its layout after changes
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    void UpdateItemScales()
    {
        // Get the center of the viewport (normalized)
        float viewportHeight = scrollRect.viewport.rect.height;
        float centerY = scrollRect.verticalNormalizedPosition * (content.rect.height - viewportHeight);

        foreach (RectTransform item in items)
        {
            // Get the distance of the item from the center of the viewport
            float itemCenterY = Mathf.Abs(item.anchoredPosition.y - centerY);

            // Normalize the distance so that items far from the center get scaled down
            float distance = itemCenterY / viewportHeight;

            // Calculate the scale based on the distance to the center
            float scale = Mathf.Lerp(maxScale, minScale, Mathf.Clamp01(distance * scaleFactor));

            // Apply the scale to the item
            item.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
