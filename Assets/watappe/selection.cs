using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class selection : MonoBehaviour {

    float move = -90;

    float flag = 0;
    float flag1 = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float Y = Input.GetAxis("Vertical");

        if (Y <0 && flag < 3 && flag1 == 0)
        {
            transform.position = new Vector3(transform.position.x,
                                         transform.position.y + move, transform.position.z);

            flag += 1;

            flag1 = 1;
        }

        if (Y > 0 && flag > 0 && flag1 == 0)
        {
            transform.position = new Vector3(transform.position.x,
                                         transform.position.y - move, transform.position.z);

            flag -= 1;

            flag1 = 1;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("start_button"))
        {
            if(flag == 0)
            {
                SceneManager.LoadScene(0);
            }
            if (flag == 1)
            {
                SceneManager.LoadScene(1);
            }
        }
        if(Y == 0)
        {
            flag1 = 0;
        }

    }
    
}
