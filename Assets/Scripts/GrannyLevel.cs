using UnityEngine;
using UnityEngine.SceneManagement;

public class GrannyLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().name == "LevelOneScene")
        {
            SceneManager.LoadScene("LevelThreeScene");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SceneManager.GetActiveScene().name == "LevelThreeScene")
        {
            SceneManager.LoadScene("VictoryScreen");
        }
        else if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            SceneManager.LoadScene("LevelOneScene");
        }
    }
}
