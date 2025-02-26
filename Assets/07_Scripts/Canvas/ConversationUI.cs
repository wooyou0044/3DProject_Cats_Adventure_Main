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

    public bool isAlready { get; set; }
    public bool isEnd { get; set; }
    public bool isEnterPressed { get; set; }

    int charIndex;
    int conversationsIndex;
    char[] converseArr;

    void Awake()
    {
        isAlready = true;
    }

    void Start()
    {
        textArrow.gameObject.SetActive(false);
        charIndex = 0;
        isEnd = false;
    }

    void Update()
    {
        
    }

    public void SetConverstation(int converseIndex)
    {
        charIndex = 0;
        conversationsIndex = converseIndex;
        conversationText.text = string.Empty;
        textArrow.gameObject.SetActive(false);
        if (converseIndex <= conversations.Length - 1)
        {
            SeparateConversation(converseIndex);
            StartCoroutine(PrintAllConversation(converseArr));
        }
        else 
        {
            StartCoroutine(ConversationDone());
        }
    }

    void SeparateConversation(int index)
    {
        if(index <= conversations.Length - 1)
        {
            string converse = conversations[index];
            converseArr = new char[conversations[index].Length];
            for (int i = 0; i < converseArr.Length; i++)
            {
                converseArr[i] = converse[i];
            }
        }
    }

    IEnumerator PrintAllConversation(char[] chars)
    {
        // 나중에 글자 나오고 스페이스바 누르게 만들 bool형
        isAlready = false;
        while(charIndex < chars.Length - 1)
        {
            if (isEnterPressed == true)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
            conversationText.text += chars[charIndex];
            charIndex++;
        }

        if(isEnterPressed)
        {
            while (charIndex < chars.Length - 1)
            {
                conversationText.text += chars[charIndex++];
            }
        }

        textArrow.gameObject.SetActive(true);
        isAlready = true;
        isEnterPressed = false;

        if (conversationsIndex == conversations.Length - 1)
        {
            isEnd = true;
        }
    }

    IEnumerator ConversationDone()
    {
        gameObject.SetActive(false);
        yield return new WaitWhile(() => isEnterPressed == true);
    }
}
