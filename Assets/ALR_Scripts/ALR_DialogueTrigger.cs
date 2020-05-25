using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALR_DialogueTrigger : MonoBehaviour
{ 
    public ALR_Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<ALR_DialogueManager>().StartDialogue(dialogue);
    }


}
