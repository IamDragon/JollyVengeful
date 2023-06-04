using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class DialogueManager : MonoBehaviour
{
    public static event Action dialogueEvent;
    public TextMeshProUGUI speakerName, dialogue;
    public Image speakerSprite;
    private bool lineFinish;
    private int currentIndex;
    private Conversation currentConvo;
    private static DialogueManager instance;
    private Animator anim;
    [SerializeField] float defaultTextSpeed = 0.042f;
    private float fastTextSpeed;
    private bool useFastTextSpeed = false;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            anim = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fastTextSpeed = defaultTextSpeed / 10;
    }
    public void Update()
    {
        if (lineFinish)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ReadNext();
                useFastTextSpeed = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
            useFastTextSpeed = true;
    }
    public static void StartConversation(Conversation convo)
    {
        instance.anim.SetBool("isOpen", true);
        instance.currentIndex = 0;
        instance.currentConvo = convo;
        instance.speakerName.text = "";
        instance.dialogue.text = "";

        instance.ReadNext();
    }
    public void ReadNext()
    {
        if (currentIndex > currentConvo.GetLength() && lineFinish)
        {
            instance.anim.SetBool("isOpen", false);
            dialogueEvent?.Invoke();
            lineFinish = false;
            return;
        }

        speakerName.text = currentConvo.GetLineByIndex(currentIndex).speaker.GetName();
        instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));
        speakerSprite.sprite = currentConvo.GetLineByIndex(currentIndex).speaker.GetSprite();
        currentIndex++;        
    }
    private IEnumerator TypeText(string text)
    {
        dialogue.text = "";
        bool complete = false;
        lineFinish = false;
        int index = 0;
        while(!complete)
        {
            dialogue.text += text[index];
            index++;
            if (useFastTextSpeed)
                yield return new WaitForSeconds(fastTextSpeed);
            else
                yield return new WaitForSeconds(defaultTextSpeed);
          
            if (index == text.Length)
            {
                complete = true;
                lineFinish = true;
            }
        }
    }
}
