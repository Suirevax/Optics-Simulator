using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject selectedGameObject = null;

    public void newSelection(GameObject newSelectedGameObject)
    {
        if (selectedGameObject)
        {
            if (!ReferenceEquals(selectedGameObject, newSelectedGameObject))
            {
                selectedGameObject.GetComponent<Image>().color = Color.white;
                newSelectedGameObject.GetComponent<Image>().color = Color.red;
                selectedGameObject = newSelectedGameObject;
            }
        }
        else
        {
            newSelectedGameObject.GetComponent<Image>().color = Color.red;
            selectedGameObject = newSelectedGameObject;
        }
    }

    public GameObject GetSelectedObject()
    {
        if (selectedGameObject) return selectedGameObject;
        return null;
    }
}
