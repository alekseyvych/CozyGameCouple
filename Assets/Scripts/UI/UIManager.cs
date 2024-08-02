using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject furniturePanel;
    void Start()
    {
        furniturePanel.SetActive(false);

        // Assign button listeners
        Button openPanelButton = GameObject.Find("OpenPanelButton").GetComponent<Button>();
        openPanelButton.onClick.AddListener(() => TogglePanel(true));

        Button[] furnitureButtons = furniturePanel.GetComponentsInChildren<Button>();
        foreach (Button button in furnitureButtons)
        {
            button.onClick.AddListener(() => SelectFurniture(button.name.Replace("Button", "")));
        }
    }

    public void TogglePanel(bool isActive)
    {
        furniturePanel.SetActive(isActive);
    }

    public void SelectFurniture(string furnitureName)
    {
        TogglePanel(false);
    }
}
