using UnityEngine;
using System.Collections;

public class Maincamera : MonoBehaviour
{

    public float MouseSpeedX = 10.0f;
    public float MouseSpeedY = 10.0f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Input.GetAxis("Mouse Y") * MouseSpeedY, (Input.GetAxis("Mouse X") * MouseSpeedX), 0);

        Quaternion qua = Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y") * MouseSpeedY, Input.GetAxis("Mouse X") * MouseSpeedX,0));
    }
}
