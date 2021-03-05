using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject presetsPanel = null;
    [SerializeField] GameObject propertiesPanel = null;
    [SerializeField] GameObject selectionManagerObject = null;
    [SerializeField] GameObject lensPanel = null;
    [SerializeField] GameObject laserPanel = null;

    [SerializeField] GameObject heightInputField;
    [SerializeField] GameObject widthInputField;

    SelectionManager selectionManager = null;
    GameObject selectedGameObject = null;

    private void Start()
    {
        selectionManager = selectionManagerObject.GetComponent<SelectionManager>();
        laserPanel.SetActive(false);
        lensPanel.SetActive(false);
    }

    private void Update()
    {
        if (propertiesPanel.activeInHierarchy)
        {
            selectedGameObject = selectionManager.GetSelectedObject().GetComponent<UIDrag>().GetLinkedObject();
            if (selectedGameObject.CompareTag("Transparent"))
            {
                lensPanel.SetActive(true);
                laserPanel.SetActive(false);
            }
            else if (selectedGameObject.CompareTag("Laser"))
            {
                lensPanel.SetActive(false);
                laserPanel.SetActive(true);
            }
            else
            {
                lensPanel.SetActive(false);
                laserPanel.SetActive(false);
            }
        }
    }

    public void PresetsButtonPressed()
    {
        presetsPanel.SetActive(true);
        propertiesPanel.SetActive(false);
    }

    public void PropertiesButtonPressed()
    {
        presetsPanel.SetActive(false);
        propertiesPanel.SetActive(true);
    }

    public void HeightValueChanged()
    {
        string value = heightInputField.GetComponent<InputField>().text;
        Debug.Log("new Height = " + value);
        if (float.TryParse(value, out float n))
        {
            selectedGameObject.GetComponent<LensGenerator>().lensSize.y = n;
        }
    }
    
    public void WidthValueChanged()
    {
        string value = widthInputField.GetComponent<InputField>().text;
        if (float.TryParse(value, out float n))
        {
            selectedGameObject.GetComponent<LensGenerator>().lensSize.x = n;
        }
    }
}
