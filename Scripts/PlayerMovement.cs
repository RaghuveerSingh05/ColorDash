using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardspeed=10f;
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward*forwardspeed*Time.deltaTime);
    }
}
