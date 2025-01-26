using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect; // It's the "velocity" of the parallax
    public bool noUp;
    float startPositionY;

    void Start()
    {
        startpos = transform.position.x;
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        cam = FindObjectOfType<Camera>().gameObject;

       startPositionY = transform.position.y;
    }

    void Update()
    {
        if (noUp)
        {

        }   
        // How far we have moved in world space
        float dist = cam.transform.position.x * parallaxEffect;

        // How far the camera has moved relative to the image (Obs: The value of the "temp" is the same oposite value of the object's position like, 1 and -1  )
        float temp = cam.transform.position.x * (1 - parallaxEffect); // This also returns the value needed for the image to accompany the camare position 

        if (noUp)
        {
            transform.position = new Vector3(startpos + dist, startPositionY, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        }

        // When the value of temp is bigger or smaller than the length the position of the object is changed to the center of the camera 
        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
