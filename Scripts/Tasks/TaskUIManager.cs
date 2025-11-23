using UnityEngine;
using UnityEngine.UI;

public class TaskUIManager : MonoBehaviour
{
    public static TaskUIManager Instance;

    [SerializeField] private GameObject taskPanel; // UI Panel
    [SerializeField] private Text taskText; // Text component

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        taskPanel.SetActive(false);
    }

    public void ShowTasks(string tasks)
    {
        taskText.text = tasks;
        taskPanel.SetActive(true);
    }

    public void HideTasks()
    {
        taskPanel.SetActive(false);
    }
}