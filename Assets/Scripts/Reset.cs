using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    public Button myResetButton;

    void Start()
    {
        if (myResetButton != null)
        {
            // This manually wires up the click event without using the dropdown!
            myResetButton.onClick.AddListener(ResetScene);
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
