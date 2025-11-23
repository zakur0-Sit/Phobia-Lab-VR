using UnityEngine;
using TMPro;

public class TaskTrigger : MonoBehaviour
{
    [TextArea] public string mesajTask;
    public bool ascundeTextCandIese = true;

    private static GameObject taskTextObject;
    private static TextMeshProUGUI taskText;

    private void Awake()
    {
        // Găsește referința statică o singură dată
        if (taskText == null)
        {
            taskTextObject = GameObject.Find("TaskText");
            if (taskTextObject != null)
            {
                taskText = taskTextObject.GetComponent<TextMeshProUGUI>();
                taskTextObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Nu s-a găsit obiectul UI 'TaskText' în scenă!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && taskText != null)
        {
            taskText.text = mesajTask;
            taskTextObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && taskText != null && ascundeTextCandIese)
        {
            taskTextObject.SetActive(false);
        }
    }
}