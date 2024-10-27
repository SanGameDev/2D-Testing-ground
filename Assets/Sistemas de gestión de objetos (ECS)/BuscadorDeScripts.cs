using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscadorDeScripts : MonoBehaviour
{
    public string scriptName;

    void Start()
    {
        AddScriptByName(scriptName);
    }

    void AddScriptByName(string name)
    {
        // Find the type of the script by name
        System.Type scriptType = System.Type.GetType(name);
        
        if (scriptType != null)
        {
            // Add the script to the GameObject
            gameObject.AddComponent(scriptType);
        }
        else
        {
            Debug.LogError("Script not found: " + name);
        }
    }
}
