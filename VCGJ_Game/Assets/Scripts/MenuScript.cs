using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private Button levels_button;
    private Button end_game_button;
    private Button return_button;
    private Button level1_button, level2_button, level3_button;
    // Start is called before the first frame update
    void Start()
    {

        levels_button = GameObject.Find("Levels Button").GetComponent<Button>();
        end_game_button = GameObject.Find("End Game Button").GetComponent<Button>();
        return_button = GameObject.Find("Return Button").GetComponent<Button>();
        level1_button = GameObject.Find("Level 1 Button").GetComponent<Button>();
        level2_button = GameObject.Find("Level 2 Button").GetComponent<Button>();
        level3_button = GameObject.Find("Level 3 Button").GetComponent<Button>();

        levels_button.onClick.AddListener(() => ButtonPress(levels_button));
        end_game_button.onClick.AddListener(() => ButtonPress(end_game_button));
        return_button.onClick.AddListener(() => ButtonPress(return_button));
        level1_button.onClick.AddListener(() => ButtonPress(level1_button));
        level2_button.onClick.AddListener(() => ButtonPress(level2_button));
        level3_button.onClick.AddListener(() => ButtonPress(level3_button));

        return_button.gameObject.SetActive(false);
        level1_button.gameObject.SetActive(false);
        level2_button.gameObject.SetActive(false);
        level3_button.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetStartScreen()
    {
        levels_button.gameObject.SetActive(true);
        end_game_button.gameObject.SetActive(true);

        return_button.gameObject.SetActive(false);
        level1_button.gameObject.SetActive(false);
        level2_button.gameObject.SetActive(false);
        level3_button.gameObject.SetActive(false);
    }

    private void SetLevelsScreen()
    {
        levels_button.gameObject.SetActive(false);
        end_game_button.gameObject.SetActive(false);

        return_button.gameObject.SetActive(true);
        level1_button.gameObject.SetActive(true);
        level2_button.gameObject.SetActive(true);
        level3_button.gameObject.SetActive(true);
    }

    void ButtonPress(Button b)
    {
        if (b == levels_button)
        {
            SetLevelsScreen();
        }
        if (b == end_game_button)
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }
        if (b == return_button)
        {
            SetStartScreen();
        }
        if (b == level1_button)
        {
            SceneManager.LoadScene(sceneName: "Level 1");
        }
        if (b == level2_button)
        {
            SceneManager.LoadScene(sceneName: "Level 2");
        }
        if (b == level3_button)
        {
            SceneManager.LoadScene(sceneName: "Level 3");
        }
    }
}
