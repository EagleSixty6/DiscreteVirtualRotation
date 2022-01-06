using System.Collections.Generic;
using UnityEngine;

public class ScriptHandling : MonoBehaviour
{
    // List of the rotation scripts
    public List<RotationMethod> scripts;

    // Start is called before the first frame update
    void Start()
    {
        // Add scripts to list
        scripts.Clear();
        scripts.AddRange(gameObject.GetComponents<RotationMethod>());
        scripts.Sort((g1, g2) => g1.GetType().ToString().CompareTo(g2.GetType().ToString()));
        if(GetComponent<Main>().methodOrder.Length != 0)
        {
            SortByArray(GetComponent<Main>().methodOrder);
        }
    }

    void Update()
    {
        
    }

    // Making sure that only one script is enabled
    public void OnlyEnableOne(int e, List<RotationMethod> scriptList)
    {
        for (int i = 0; i < scriptList.Count; i++)
        {
            if (i == e)
            {
                scriptList[i].enabled = true;
            }
            else
            {
                scriptList[i].enabled = false;
            }
        }
    }

    public void DisableAll()
    {
        foreach(RotationMethod r in scripts)
        {
            r.enabled = false;
        }
    }

    public void SortByArray(int[] array)
    {
        if(scripts.Count == 0)
        {
            return;
        }

        List<RotationMethod> copy = new List<RotationMethod>();

        foreach (int i in array)
        {
            copy.Add(scripts[i]);
        }

        scripts.Clear();
        scripts.AddRange(copy);
    }
}
