using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemChecklistManager : MonoBehaviour
{
    [System.Serializable]
    public class ChecklistEntry
    {
        public GameObject itemObject;
        public TMP_Text itemText;
        public bool isCollected = false;
    }

    public GameObject checklistUI;  
    public GameObject timerUI;  

    public List<ChecklistEntry> checklistEntries;
    public GameObject winPanel;
    public GameObject losePanel;
    public PlayerMovement playerMovement;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
        if (losePanel != null)
            losePanel.SetActive(false);
    }

    public void CrossOutItem(GameObject item)
    {
        foreach (ChecklistEntry entry in checklistEntries)
        {
            if (entry.itemObject == item && !entry.isCollected)
            {
                entry.isCollected = true;
                entry.itemText.text = "<s> ✓ " + entry.itemText.text.Replace("-<s>", "").Replace("</s>", "") + "</s>";

                CheckWinCondition();
                break;
            }
        }
    }

    private void CheckWinCondition()
    {
        int collectedCount = 0;

        foreach (ChecklistEntry entry in checklistEntries)
        {
            if (entry.isCollected)
                collectedCount++;
        }

        if (collectedCount >= checklistEntries.Count)
        {
            Debug.Log("All items collected! You win!");
            if (winPanel != null)
                winPanel.SetActive(true);

            if (checklistUI != null) checklistUI.SetActive(false);
            if (timerUI != null) timerUI.SetActive(false);

            Time.timeScale = 0f;
        }
    }

    public void TriggerLoseCondition()
    {
        Debug.Log("You lost!");
        if (losePanel != null)
            losePanel.SetActive(true);

        if (checklistUI != null) checklistUI.SetActive(false);
        if (timerUI != null) timerUI.SetActive(false);

        Time.timeScale = 0f;
    }

}
