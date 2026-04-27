using Unity.VisualScripting;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0,3,-8);
    public float smoothspeed = 0.125f;



    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)return;

        Vector3 desiredpos= target.position +offset; // desired camera position


        Vector3 smoothedpos= Vector3.Lerp(transform.position,desiredpos,smoothspeed); // smooth movement of the camera 
 
        transform.position=smoothedpos; //apply to camera

        transform.LookAt(target);  //make look camera at player 

    }
}
