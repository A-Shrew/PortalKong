using UnityEngine;
using TMPro;

public class TitleAnimation : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    private Color color1 = new Color(1f, 0.2f, 0f); // Red
    private Color color2 = new Color(1f, 0.5f, 0f); // Orange
    private float flashSpeed = 0.5f; // Time between color changes
    private float timer = 0f;
    private bool isColor1 = true;

    private float[] randomOffsets;
    private string originalText;

    void Start()
    {
        titleText.fontSize = 36;
        titleText.color = color1;

        originalText = titleText.text; // Store the original text
        InitializeOffsets();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flashSpeed)
        {
            timer = 0f;
            isColor1 = !isColor1;
            titleText.color = isColor1 ? color1 : color2;
        }

        ApplyLetterAnimation();
    }

    void InitializeOffsets()
    {
        randomOffsets = new float[originalText.Length]; // Match original text length
        for (int i = 0; i < originalText.Length; i++)
        {
            randomOffsets[i] = Random.Range(0f, Mathf.PI * 2); // Random start offset
        }
    }

    void ApplyLetterAnimation()
    {
        string newText = "";

        for (int i = 0; i < originalText.Length; i++)
        {
            float yOffset = Mathf.Sin(Time.time * 4f + randomOffsets[i]) * 5f; // Move letters up/down
            newText += $"<voffset={yOffset}px>{originalText[i]}</voffset>"; // Apply vertical offset
        }

        titleText.text = newText;
    }
}
