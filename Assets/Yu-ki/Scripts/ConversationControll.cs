using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class ConversationControll : MonoBehaviour
{
    public GameObject[] m_TextBox;

    //フェードに使用するオブジェクト
    public Image m_FadeObject;

    //ログ表示時の背景
    public GameObject m_Log;

    //ログのテキスト
    public Text m_LogText;

    //読み込むテキストのパス(シーン移動する前に設定)
    public static string m_TextPath = "Test";

    //テキストの状態
    enum ConversationState
    {
        Read,       //テキスト表示
        Wait,       //時間待ち
        Fade,       //暗転中
        Log,        //ログの表示
        Finish      //テキスト読み込み終了    
    }

    //テキストの状態
    private ConversationState m_State;

    //ストリームリーダー
    private StreamReader m_Sr;

    //ShiftJISにするエンコーダー
    Encoding m_EncodingShiftJIS;

    //読み込むテキストを格納する変数
    private string m_Text;

    //表示するテキストを保存しておくための変数
    private string m_TempText = null;

    //前の文字を表示してからの時間
    private float m_ReadCurrentTime;

    //現在の文字の位置
    private int m_CurrentTextNum = 0;

    //フェードオブジェクトのコンポーネント
    private Fade m_FadeObjectComponent;

    //次のテキスト読み込みまで待つ時間
    private float m_WaitTime;

    //現在の待ち時間
    private float m_WaitCurrentTime;

    //現在話すキャラの番号
    private int m_ConversationNum;

    //次に移行するステート
    private ConversationState m_AfterState;

    //=============================================================================
    //
    // Purpose : スタート関数．
    //           Update関数の前に1度だけ呼び出される
    //
    // Return : なし．
    //
    //=============================================================================
    void Start()
    {
        m_FadeObject.gameObject.SetActive(true);

        m_FadeObjectComponent = m_FadeObject.GetComponent<Fade>();
        m_FadeObjectComponent.FadeIn();

        m_Log.gameObject.SetActive(false);
        m_LogText.text = null;

        m_State = ConversationState.Read;

        //Shift_JISのエンコーダーを取得
        m_EncodingShiftJIS = Encoding.GetEncoding("Shift_JIS");

        //現在設定されているテキストを読み込む
        TextRead();
    }

    //=============================================================================
    //
    // Purpose : アップデート関数
    //
    // Return : なし．
    //
    //=============================================================================
    void Update()
    {
        switch (m_State)
        {
            //テキストを一文づつ確認して表示
            case ConversationState.Read:

                Read();

                if (Input.GetKeyDown(KeyCode.L))
                {
                    m_State = ConversationState.Log;

                    m_AfterState = ConversationState.Read;

                    m_Log.gameObject.SetActive(true);
                }

                break;

            case ConversationState.Wait:

                if ((m_WaitCurrentTime += Time.deltaTime) > m_WaitTime)
                {
                    m_State = ConversationState.Read;
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    m_State = ConversationState.Log;

                    m_AfterState = ConversationState.Wait;

                    m_Log.gameObject.SetActive(true);
                }

                break;

            //フェード
            case ConversationState.Fade:

                if (m_FadeObjectComponent.m_IsFadeFinish)
                {
                    m_State = ConversationState.Read;
                }

                break;

            //ログの表示
            case ConversationState.Log:

                if (Input.GetKeyDown(KeyCode.L))
                {
                    m_State = m_AfterState;

                    m_Log.gameObject.SetActive(false);
                }

                break;

            //テキスト読み込み終了
            case ConversationState.Finish:

                if (Input.GetKeyDown(KeyCode.L))
                {
                    m_State = ConversationState.Log;

                    m_AfterState = ConversationState.Finish;

                    m_Log.gameObject.SetActive(true);
                }

                break;

        }
    }

    //=============================================================================
    //
    // Purpose : テキストを表示用の変数に読み込む．
    //
    // Return : なし．
    //
    //=============================================================================
    void TextRead()
    {
        //ストリームリーダーを作成
        m_Sr = new StreamReader(Application.dataPath + "/Text/" + m_TextPath + ".txt", m_EncodingShiftJIS);

        //最後まで読み込む
        m_Text = m_Sr.ReadToEnd();
        m_Sr.Close();
    }

    //=============================================================================
    //
    // Purpose : 現在の行に表示する文字を追加．
    //
    // Return : なし．
    //
    //=============================================================================
    void AddText(string AddText = null)
    {
        //追加するテキストが指定されていなかったら
        if (AddText == null)
        {
            m_TempText += m_Text[m_CurrentTextNum];

            m_CurrentTextNum++;
        }
        else
        {
            m_TempText += AddText;
        }

    }

    //=============================================================================
    //
    // Purpose : テキストの表示．
    //
    // Return : なし．
    //
    //=============================================================================
    void Read()
    {
        //まだ全部読んでいなかったら
        if (m_CurrentTextNum < m_Text.Length)
        {
            //特殊文字じゃなかったら
            if (CheckWord())
            {
                //現在の行に文字を追加
                AddText();

                //文字を表示する経過時間のリセット
                m_ReadCurrentTime = 0;
            }
        }
        else
        {
            //全部表示されていたら全部表示された状態にする
            m_State = ConversationState.Finish;
        }
    }
 
    //=============================================================================
    //
    // Purpose : フェード開始関数
    //
    // Return : なし
    //
    //=============================================================================
    void FadeStart(bool FadeIn, ConversationState AfterState = ConversationState.Read)
    {
        //フェード状態にする
        m_State = ConversationState.Fade;

        if (FadeIn)
        {
            m_FadeObjectComponent.FadeIn();
        }
        else
        {
            m_FadeObjectComponent.FadeOut();
        }
    }

    //=============================================================================
    //
    // Purpose : 文字列の取得関数
    //          
    // Return : ""で囲まれている文字を取得して返す。
    //          "で始まらなかったり、"が来る前に特殊文字がきたらnullを返す。
    //
    //=============================================================================
    string PickString()
    {
        //次の文字にする
        m_CurrentTextNum++;

        //"から始まっていたら
        if (m_Text[m_CurrentTextNum] == '\"')
        {
            //読みだした文字列を格納する変数
            string Text = null;

            for (int i = m_CurrentTextNum + 1; i < m_Text.Length && m_Text[i] != '#'; i++)
            {
                //文字列終わりの記号が見つかったら
                if (m_Text[i] == '\"')
                {
                    //カウントを見つかった次のカウントにする。
                    m_CurrentTextNum = i + 1;

                    return Text;
                }

                Text += m_Text[i];
            }
        }

        //規則にあった文字列が見つからないのでnullを返す。
        return null;
    }

    //=============================================================================
    //
    // Purpose : 文字列の分割関数
    //          
    // Return : |で文字列を分割しその配列を返す。
    //
    //=============================================================================
    List<string> SplitString(string Text, char SplitKey)
    {
        List<string> SplitText = new List<string>();

        string OnceText = null;

        for (int i = 0; i < Text.Length; i++)
        {
            if (Text[i] == SplitKey)
            {
                SplitText.Add(OnceText);
                OnceText = null;
            }
            else
            {
                OnceText += Text[i];
            }
        }

        if (Text[Text.Length - 1] != SplitKey)
        {
            SplitText.Add(OnceText);
        }

        return SplitText;
    }

    //=============================================================================
    //
    // Purpose : 通常文字化のチェック
    //           通常文字ではなかったらそれに見合った処理をする
    //
    // Return : 通常文字ならtrue,違ったらfalseを返す。．
    //
    //=============================================================================
    bool CheckWord()
    {
        //操作用の文字列を格納する配列
        string OperationText;

        switch (m_Text[m_CurrentTextNum])
        {
            //改行文字は無視する
            case '\r':
            case '\n':

                m_CurrentTextNum++;

                break;

            //以下の文字の場合は強制的に今の行に追加する
            case '、':
            case '。':
            case '!':
            case '?':
            case '！':
            case '？':

                AddText();

                break;

            //特殊文字
            case '#':

                //次に進める
                m_CurrentTextNum++;

                //文字を確認
                switch (m_Text[m_CurrentTextNum])
                {
                    case '#':

                        AddText();

                        break;

                    //改行
                    case '\\':

                        if (m_Text[m_CurrentTextNum + 1] == 'n')
                        {
                            AddText("\n");

                            //テキストのカウントを\\とn分進める
                            m_CurrentTextNum += 2;
                        }

                        break;

                    case 'w':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            m_State = ConversationState.Wait;

                            m_WaitTime = int.Parse(OperationText);
                            m_WaitCurrentTime = 0;
                        }

                            break;

                    case 'd':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            m_TextBox[m_ConversationNum].GetComponent<CharaText>().SetText(m_TempText,int.Parse(OperationText));

                            m_LogText.text += m_TempText + "\n";

                            m_TempText = null;
                        }

                        break;

                    case 'c':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            if(m_TextBox.Length >= int.Parse(OperationText))
                            {
                                m_ConversationNum = int.Parse(OperationText) -1;

                                m_LogText.text += m_TextBox[m_ConversationNum].GetComponent<CharaText>().m_CharaName + "\n";
                            }
                        }

                        break;

                    //フェード
                    case 'f':

                        OperationText = PickString();

                        //文字列が返ってきたら
                        if (OperationText != null)
                        {
                            switch (OperationText)
                            {
                                //フェードイン
                                case "in":

                                    FadeStart(true);

                                    break;

                                //フェードアウト
                                case "out":

                                    FadeStart(false);

                                    break;

                                //テキストがあっていない場合
                                default:

                                    //エラーログの表示
                                    Debug.Log("フェードの操作が間違っています。/nテキストを確認してください。");

                                    break;
                            }
                        }

                        break;
                }

                break;

            //他は通常文字
            default:

                //通常文字なのでtrueを返す。
                return true;
        }

        return false;
    }
}
