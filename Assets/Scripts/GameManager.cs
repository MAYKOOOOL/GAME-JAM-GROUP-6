using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ItemChecklistManager checklistManager;
    public GameObject winPanel;

    private int collectedCount = 0;

    void Start()
    {
        collectedCount = 0;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void ItemCollected(GameObject item)
    {
        checklistManager.CrossOutItem(item);
        collectedCount++;

        if (collectedCount >= checklistManager.checklistEntries.Count)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        Debug.Log("All items collected! You win!");

        // Activate win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
