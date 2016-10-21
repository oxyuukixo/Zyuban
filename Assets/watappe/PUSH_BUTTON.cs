using UnityEngine;
using System.Collections;

public class PUSH_BUTTON : MonoBehaviour {

    enum TITELMENBER
    {
        title,
        menu,

    }

    public GameObject[] m_tileMenber;

    private TITELMENBER m_state;

	// Use this for initialization
	void Start () {
        m_state = TITELMENBER.title;
        m_tileMenber[(int)TITELMENBER.menu].SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        switch(m_state)
        {
            case TITELMENBER.title:
                if (!Input.GetMouseButtonDown(0)) break; ;
                m_tileMenber[(int)m_state].SetActive(false);
                m_tileMenber[(int)TITELMENBER.menu].SetActive(true);
                m_state = TITELMENBER.menu;
                break;
            case TITELMENBER.menu:
                break;
        }
	}
}
