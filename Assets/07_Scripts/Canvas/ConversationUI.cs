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
    public bool isAlready { get; private set; }
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
        charIndex = 0;
        conversationText.text = string.Empty;
        SeparateConversation(converseIndex);
        StartCoroutine(PrintAllConversation(converseArr));
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

    IEnumerator PrintAllConversation(char[] chars)
    {
        // 나중에 글자 나오고 스페이스바 누르게 만들 bool형
        isAlready = false;
        while(charIndex < chars.Length - 1)
        {
            yield return new WaitForSeconds(0.1f);

            conversationText.text += chars[charIndex++];
        }
        if (charIndex >= chars.Length)
        {
            Debug.Log("글자 수 넘어감");
            isAlready = true;
            yield break;
        }
        yield return new WaitWhile(() => charIndex < chars.Length - 1);
        // 옆에 조건문이 참일때 다음으로 넘어감
        textArrow.gameObject.SetActive(true);
        isAlready = true;
    }
}
