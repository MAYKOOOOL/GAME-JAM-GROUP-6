using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemChecklistManager : MonoBehaviour
{
    public List<string> requiredItemNames;
    public List<TMP_Text> checklistTexts;

    public void CrossOutItem(string itemName)
    {
        for (int i = 0; i < requiredItemNames.Count; i++)
        {
            if (requiredItemNames[i] == itemName)
            {
                checklistTexts[i].text = "<s>- " + requiredItemNames[i] + "</s>";
                break;
            }
        }
    }
}
