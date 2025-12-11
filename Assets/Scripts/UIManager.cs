using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text infoText;
    public Button restartButton;
    public GameObject winPanel;

    public Text turnsText;

    void Start()
    {
        restartButton.onClick.AddListener(OnRestartClicked);
        HideWin();
        UpdateScore(0);
        UpdateTurns(0);
    }

    public void UpdateScore(int score)
    {
        if (scoreText) scoreText.text = "Score: " + score;
    }


    public void UpdateTurns(int turns)
    {
        if (turnsText) turnsText.text = "Turns: " + turns;
    }

    public void ShowWin(int score, int turns)
    {
        if (winPanel) winPanel.SetActive(true);

        if (infoText)
        {
            infoText.text =
                "You won!\n" +
                "Score: " + score + "\n" +
                "Attempts: " + turns;
        }
    }

    public void HideWin()
    {
        if (winPanel) winPanel.SetActive(false);
    }

    void OnRestartClicked()
    {
        FindObjectOfType<GameManager>().Restart();
        HideWin();
        if (infoText) infoText.text = "";
        UpdateTurns(0);
    }
}
