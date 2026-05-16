using System;

[Serializable]
public class DialogueData
{
    public string id;
    public string[] lines;
}

[Serializable]
public class DialogueList
{
    public DialogueData[] dialogues;
}