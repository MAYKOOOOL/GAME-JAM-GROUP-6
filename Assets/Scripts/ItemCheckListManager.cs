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
    }

    public List<ChecklistEntry> checklistEntries;

    public void CrossOutItem(GameObject item)
    {
        foreach (ChecklistEntry entry in checklistEntries)
        {
            if (entry.itemObject == item)
            {
                entry.itemText.text = "<s> ✓ " + entry.itemText.text.Replace("-<s>", "").Replace("</s>", "") + "</s>";
                break;
            }
        }
    }
}
