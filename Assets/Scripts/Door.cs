using UnityEngine;

public class Door : MonoBehaviour
{
    public void TurnOffCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
