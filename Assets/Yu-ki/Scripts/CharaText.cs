using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class CharaText : MonoBehaviour {

    //キャラ名
    public string m_CharaName;

    //名前のテキスト
    public Text m_NameText;

    //会話のテキスト
    public Text m_CoversationText;

    //何秒表示するか
    private float m_DispTime;

    //何秒表示しているか
    private float m_CurrentDispTime;

	// Use this for initialization
	void Start () {

        m_NameText.text = m_CharaName;

        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
        if(isActiveAndEnabled)
        {
            if((m_CurrentDispTime += Time.deltaTime) > m_DispTime)
            {
                gameObject.SetActive(false);
            }
        }

	}

    public void SetText(string Text, float DispTime)
    {
        m_CoversationText.text = Text;

        m_DispTime = DispTime;
        m_CurrentDispTime = 0;

        gameObject.SetActive(true);
    }
}
