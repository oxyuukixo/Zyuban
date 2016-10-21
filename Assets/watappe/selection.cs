using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class selection : MonoBehaviour {

    float move = -40;

    float flag = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow) && flag < 2)
        {
            transform.position = new Vector3(transform.position.x,
                                         transform.position.y + move, transform.position.z);

            flag += 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && flag > 0)
        {
            transform.position = new Vector3(transform.position.x,
                                         transform.position.y - move, transform.position.z);

            flag -= 1;
        }

        if(Input.GetKeyDown(KeyCode.A))
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


    }
    
}
