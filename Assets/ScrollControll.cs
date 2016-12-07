using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollControll : MonoBehaviour {

    public float m_ScrollSpeed = 0.01f;

    private ScrollRect m_ScrollRect;

	// Use this for initialization
	void Start () {

        m_ScrollRect = GetComponent<ScrollRect>();

	}
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKey(KeyCode.UpArrow))
        {
            m_ScrollRect.verticalNormalizedPosition += m_ScrollSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_ScrollRect.verticalNormalizedPosition -= m_ScrollSpeed;
        }
    }

    void OnEnable()
    {
        if(m_ScrollRect)
        {
            m_ScrollRect.verticalNormalizedPosition = 0;
        }
    }
}
