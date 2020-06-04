using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ALR_DialogueManager : MonoBehaviour
{

    [SerializeField] private GameObject dialogueUI;
    //public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;
    public ALR_PlayerInputHandler pInput;
    private ALR_SoundManager soundManager;
    


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        soundManager = GameObject.FindObjectOfType<ALR_SoundManager>();
    }


    public void StartDialogue (ALR_Dialogue dialogue)
    {
        dialogueUI.SetActive(true);
        //nameText.text = dialogue.name;
        

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {

        soundManager.Whispering();
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        FindObjectOfType<ALR_PlayerInputHandler>().endingDialogue = true;
        //Debug.Log("End of the Conversation");
    }

}
