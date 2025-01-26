using UnityEngine;

public class AMLoader : MonoBehaviour
{
    public AudioManager theAm;

    void Awake()
    {
        if (FindObjectOfType<AudioManager>() == null)
        {
            AudioManager.instance = Instantiate(theAm);
            DontDestroyOnLoad(AudioManager.instance.gameObject);
        }
    }
}
