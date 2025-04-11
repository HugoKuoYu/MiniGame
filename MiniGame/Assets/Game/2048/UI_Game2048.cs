using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UISystem;
using UnityEngine.UI;
using undoSystem;

public class UI_Game2048 : MonoBehaviour
{
    // Start is called before the first frame update
    public TileBoard tileBoard;
    public CanvasGroup gameOver;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public Button ClearBestScoreButton;
    public Button newGameButton;
    public Button UndoButton;
    public Button RedoButton;
    public void Init()
    {
        tileBoard.InitUndoSystem();
        NewGame();
        ClearBestScoreButton.onClick.AddListener(ClearBestScore);
        newGameButton.onClick.AddListener(NewGame);
        UndoButton.onClick.AddListener(()=>tileBoard.undoManager.Undo());
        RedoButton.onClick.AddListener(() => tileBoard.undoManager.Redo());

    }

    public void NewGame()
    {
        Debug.Log("NewGame");
        tileBoard.undoManager.isInitialising = true;
        SetScore(0);
        bestScoreText.text = LoadBestScore().ToString();
        gameOver.alpha = 0;
        gameOver.interactable = false;
        tileBoard.ClearBoard();
        tileBoard.CreateTile();
        tileBoard.CreateTile();
        tileBoard.undoManager.isInitialising = false;
        tileBoard.undoManager.ClearSnapShots();

        tileBoard.enabled = true;
    }
    public void GameOver()
    {
        Debug.Log("GameOver");
        tileBoard.enabled = false;
        StartCoroutine(Fade(gameOver, 1f, 0.5f));
        gameOver.interactable = true;
    }
    IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    { 
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;
        while (elapsed < duration)
        { 
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }
    private void SetScore(int score)
    { 
        this.score = score;
        scoreText.text = score.ToString();
        SaveBestScore();
        
    }
    private void SaveBestScore()
    {
        int bestScore = LoadBestScore();
        if (score > bestScore)
            PlayerPrefs.SetInt("BestScore",score);
    }
    private int LoadBestScore()
    {
        return PlayerPrefs.GetInt("BestScore", 0);
    }
    public void ClearBestScore()
    {
        PlayerPrefs.DeleteKey("BestScore");
    }
    public void SetScoreExternally(int value) //將分數還原
    {
        score = value;
        scoreText.text = value.ToString();
    }

}
