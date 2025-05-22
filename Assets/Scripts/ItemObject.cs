using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;
    
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}";
        return str;
    }
}
