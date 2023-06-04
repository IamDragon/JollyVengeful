using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text dialogueText;

    [SerializeField] Canvas dialogueGUICanvas;
    [SerializeField] Canvas dialogueBoxGUICanvas;

    [SerializeField] float letterDelay = 0.1f;
    [SerializeField] float letterMultiplier = 0.5f;

    [SerializeField] KeyCode dialogueInput = KeyCode.F;

    //Behöver ändras på fler platser osäker exakt vad den gör //Lucas
    public string nameFor;

    public string[] dialogueLines;

    [SerializeField] bool letterIsMultiplied = false;
    [SerializeField] bool dialogueActive = false;
    [SerializeField] bool dialogueEnded = false;
    [SerializeField] bool outOfRange = true;

    //public AudioClip audioClip;
   // AudioSource audioSource;

    private void Start()
    {
        dialogueText.text = "";

    }

    public void EnterRangeOfNPC()
    {
        outOfRange = false;
        dialogueGUICanvas.enabled = true;
        dialogueGUICanvas.gameObject.SetActive(true);

        if (dialogueActive == true)
        {
            dialogueGUICanvas.enabled = true;
            dialogueGUICanvas.gameObject.SetActive(true);
        }
    }

    public void NPCName()
    {
        outOfRange = false;

        dialogueBoxGUICanvas.enabled = true;
        dialogueBoxGUICanvas.gameObject.SetActive(true);
        nameText.text = name;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!dialogueActive)
            {
                dialogueActive = true;
                StartCoroutine(StartDialogue());
            }
        }
    }

    private IEnumerator StartDialogue()
    {
        if (outOfRange == false)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;
            while (currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if (!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    if (dialogueLines[currentDialogueIndex++] != null)
                        StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));                   
                    if (currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = false;
                    }
                }
                yield return 0;
            }
            //När konversationen pågår, alla meningar som bygger upp konversationen har sagts klart och spelaren trycker F.
            //Går programmet vidare och stänger chatfönstret.
            while (true)
            {
                if (Input.GetKeyDown(dialogueInput) && dialogueEnded == true)
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = true;
            dialogueActive = false;
            DropDialogue();
        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if (outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;
            dialogueText.text = "";
            
            while (currentCharacterIndex < stringLength)
            {
                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if (currentCharacterIndex < stringLength)
                {
                    //Skippar text utskrivningen och sätter ut hela texten direkt på skärmen vid tryckning av V
                    if (Input.GetKey(KeyCode.V))
                    {
                        int temp = currentCharacterIndex;
                        currentCharacterIndex = stringLength;
                        for (int i = temp; i < stringLength; i++)
                        {
                            dialogueText.text += stringToDisplay[i];
                        }
                    }

                    if (Input.GetKey(dialogueInput))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);

                    }
                    else
                    {
                        yield return new WaitForSeconds(letterDelay);
                    }
                }
                else
                {
                    dialogueEnded = true;
                    break;
                }
            }
            //Whileloop som gör att inte koden går vidare till att dialogen avslutas innan den är över.
            while (true)
            {
                if (Input.GetKeyDown(dialogueInput))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = true;
            letterIsMultiplied = false;
            dialogueText.text = "";
        }
    }

    public void DropDialogue()
    {
        dialogueGUICanvas.gameObject.SetActive(false);
        dialogueBoxGUICanvas.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if (outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            dialogueGUICanvas.gameObject.SetActive(false);
            dialogueBoxGUICanvas.gameObject.SetActive(false);
        }
    }
}
