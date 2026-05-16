using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText; // Texto donde se muestra el diálogo
    [SerializeField] private float typingTime = 0.05f; // Velocidad de escritura del texto

    private DialogueList dialogueList; // Lista de diálogos cargados desde el JSON
    private bool isPlayerInRange; // ¿El jugador está en rango para hablar?
    private bool didDialogueStart; // ¿El diálogo ha comenzado?
    private int lineIndex; // Índice de la línea actual
    private string currentDialogueId = "greeting"; // Diálogo actual (puedes cambiarlo según la situación)

    void Start()
    {
        LoadDialogues(); // Cargar los diálogos al inicio
    }

    void LoadDialogues(){
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogue"); //va a la carpeta en donde se encuentran los dialogos
        
        if (jsonFile == null){
            Debug.LogError("No se pudo cargar el archivo JSON. Verifica la ruta en Resources."); //Si el json no se encuentra en la localidad entonces  sacará esto
            return;
        }

        dialogueList = JsonUtility.FromJson<DialogueList>(jsonFile.text);//Si lo anteriorr no sucede, es porque si existe el json y lo convertirá en una lista(creo)

        if (dialogueList == null || dialogueList.dialogues == null){
            Debug.LogError("El JSON se cargó, pero no tiene diálogos válidos."); //Si el json existe pero no tiene contenido pasará esto
        }
        
    }

    void Update(){
        if (isPlayerInRange && Input.GetButtonDown("Fire1")){
            if (!didDialogueStart){ //Si el dialogo comienza entonces irá a esta función
                StartDialogue();
            }
            else if (dialogueText.text == GetCurrentLine()){ //Si el dialogo termina y hay más entonces irá al siguiente dialogo
                NextDialogue();
            }
            else{
                StopAllCoroutines(); //Esto detiene todo
                dialogueText.text = GetCurrentLine();
            }
        }
    }

    void StartDialogue(){
        didDialogueStart = true;
        //dialoguePanel.SetActive(true);
        dialogueText.gameObject.SetActive(true);//Esto mostrará el texto
        //dialogueMark.SetActive(false); //Borrará la marca de exclamación
        lineIndex = 0;
        Time.timeScale = 0f; // Pausar el juego
        StartCoroutine(ShowLine());
    }

    void NextDialogue(){
        lineIndex++;
        if (lineIndex < GetCurrentDialogue().lines.Length){
            StartCoroutine(ShowLine());
        }
        else{
            didDialogueStart = false;
            //dialoguePanel.SetActive(false);
            // dialogueMark.SetActive(true);
            //dialogueMark.SetActive(true);
            dialogueText.gameObject.SetActive(true);
            Time.timeScale = 1f; // Reanudar el juego
        }
    }

    IEnumerator ShowLine(){
        dialogueText.text = string.Empty;
        foreach (char ch in GetCurrentLine()){
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    DialogueData GetCurrentDialogue(){
        foreach (var dialogue in dialogueList.dialogues){
            if (dialogue.id == currentDialogueId){
                return dialogue;
            }
        }
        return null;
    }

    string GetCurrentLine(){
        DialogueData dialogue = GetCurrentDialogue();
        if (dialogue == null){
            Debug.LogError($"No se encontró un diálogo con el ID '{currentDialogueId}'");
            return " "; // Evitar que el texto sea nulo devolviendo un espacio
        }

        if (lineIndex >= dialogue.lines.Length){
            Debug.LogError($"El índice de línea {lineIndex} está fuera de rango.");
            return " ";
        }
        return dialogue.lines[lineIndex];
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            isPlayerInRange = true;
            //dialogueMark.SetActive(true);
            // Player.obj.dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){
            isPlayerInRange = false;
            //dialogueMark.SetActive(false);
            // Player.obj.dialogueMark.SetActive(false);
        }
    }


    //Getters y setters
    public void SetCurrentDialogueId(string newId){
        currentDialogueId = newId;
    }
}