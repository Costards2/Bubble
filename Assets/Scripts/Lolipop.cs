using UnityEngine;
using UnityEngine.SceneManagement;

public class Lolipop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Menu");
    }
}
