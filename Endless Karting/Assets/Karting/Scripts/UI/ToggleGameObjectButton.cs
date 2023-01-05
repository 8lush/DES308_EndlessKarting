using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleGameObjectButton : MonoBehaviour
{
    public GameObject objectToToggle;
    public GameObject objectToToggle2;
    public GameObject mainScenekart;
    public bool resetSelectionAfterClick;


    private void Start()
    {
        if (!PlayerPrefs.HasKey("firstTime")) 
        {
            PlayerPrefs.SetInt("firstTime", 0);
            OpenControls();
        }
    }

    void Update()
    {
        if (objectToToggle.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel))
        {
            SetGameObjectActive(false);
        }
    }

    public void SetGameObjectActive(bool active)
    {
        objectToToggle.SetActive(active);

        if (mainScenekart != null)
            mainScenekart.SetActive(!active);

        if (resetSelectionAfterClick)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControls()
    {
        objectToToggle.SetActive(true);
        objectToToggle2.SetActive(false);
    }
}
