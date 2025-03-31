using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image[] bananaImages; 
    
    void Start()
    {
        UpdateHealthDisplay();
    }

    void Update()
    {
        UpdateHealthDisplay();
    }

    void UpdateHealthDisplay()
    {
        for (int i = 0; i < bananaImages.Length; i++)
        {
            bananaImages[i].enabled = (i < player.health);
        }
    }
    
}
