using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText;
    private Color normalColor = Color.white;
    private Color hoverColor = new Color(1f, 0.8f, 0f); // Yellowish glow

    void Start()
    {
        buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor; // Change color on hover
        transform.localScale = Vector3.one * 1.1f; // Slightly increase size
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor; // Revert to original color
        transform.localScale = Vector3.one; // Reset size
    }
}
