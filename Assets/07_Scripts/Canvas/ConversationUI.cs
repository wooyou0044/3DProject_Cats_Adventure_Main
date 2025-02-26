using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    [SerializeField] Text conversationText;
    [SerializeField] Image textArrow;
    [TextArea]
    [SerializeField] string[] conversations;

    Animator arrowAni;
    bool isAlready;
    int charIndex;
    char[] converseArr;

    void Awake()
    {
        arrowAni = textArrow.GetComponent<Animator>();
    }

    void Start()
    {
        textArrow.gameObject.SetActive(false);
        charIndex = 0;
    }

    void Update()
    {
        
    }

    public void SetConverstation(int converseIndex)
    {
        conversationText.text = string.Empty;
        SeparateConversation(converseIndex);
        //StartCoroutine(PrintAllConversation(converseArr));
        StartCoroutine(PrintConverstion(converseArr));
    }

    void SeparateConversation(int index)
    {
        string converse = conversations[index];
        converseArr = new char[conversations[index].Length];
        for (int i = 0; i < converseArr.Length; i++)
        {
            converseArr[i] = converse[i];
        }
    }

    //IEnumerator PrintAllConversation(char[] chars)
    //{
    //    if (charIndex >= chars.Length)
    //    {
    //        Debug.Log("글자 수 넘어감");
    //        isAlready = true;
    //        yield break;
    //    }

    //    isAlready = false;

    //    StartCoroutine(PrintConverstion(chars[charIndex]));

    //    yield return new WaitWhile(() => charIndex <= chars.Length - 1);

    //    textArrow.gameObject.SetActive(true);
    //}

    IEnumerator PrintConverstion(char[] chars)
    {
        yield return new WaitForSeconds(0.5f);
        conversationText.text += chars[charIndex];
        charIndex++;
        yield return new WaitWhile(() => charIndex <= chars.Length - 1);
    }
}
