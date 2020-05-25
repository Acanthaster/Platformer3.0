﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ALR_DialogueManager : MonoBehaviour
{

    [SerializeField] private GameObject dialogueUI;
    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;

    


    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }


    /*private void Update()
    {
        if (Input.GetButtonDown("Start Dialogue"))
        {
            StartDialogue(dialogue);
        }

    }*/



    public void StartDialogue (ALR_Dialogue dialogue)
    {
        Time.timeScale = 0;
        dialogueUI.SetActive(true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
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
        Time.timeScale = 1;
        dialogueUI.SetActive(false);
        Debug.Log("End of the Conversation");
    }

}
