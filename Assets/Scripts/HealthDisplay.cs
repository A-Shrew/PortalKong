using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    private int health = 3; 
    public TMP_Text healthText;
    public Image[] bananaImages; 
    
    void Start()
    {
        UpdateHealthDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (health > 0)
            {
                health--;
                UpdateHealthDisplay();
            }
        }
    }

    void UpdateHealthDisplay()
    {

        for (int i = 0; i < bananaImages.Length; i++)
        {
            bananaImages[i].enabled = (i < health);
        }        
    }
    
}
