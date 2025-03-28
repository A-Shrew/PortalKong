using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BananaBounce : MonoBehaviour
{
    public RectTransform[] bananaImages; 
    public float bounceHeight = 20f; 
    public float bounceSpeed = 1f; 

    void Start()
    {
        foreach (RectTransform banana in bananaImages)
        {
            StartCoroutine(BounceBanana(banana, Random.Range(0f, 1f)));
        }
    }

    IEnumerator BounceBanana(RectTransform banana, float delay)
    {
        yield return new WaitForSeconds(delay); 

        Vector3 originalPosition = banana.anchoredPosition;

        while (true)
        {
            float randomSpeed = Random.Range(bounceSpeed * 0.8f, bounceSpeed * 1.2f);
            float randomHeight = Random.Range(bounceHeight * 0.8f, bounceHeight * 1.2f); 

            float elapsedTime = 0;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * randomSpeed;
                float yOffset = Mathf.Sin(elapsedTime * Mathf.PI) * randomHeight;
                banana.anchoredPosition = originalPosition + new Vector3(0, yOffset, 0);
                yield return null;
            }
        }
    }
}
