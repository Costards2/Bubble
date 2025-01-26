using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            door.GetComponent<Animator>().enabled = true;
            gameObject.SetActive(false);
        }
    }

    void TurnOff()
    {
        door.GetComponent<Collider2D>().enabled = false;
    }
}
