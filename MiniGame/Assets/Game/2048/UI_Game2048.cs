using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UISystem;

public class UI_Game2048 : MonoBehaviour
{
    // Start is called before the first frame update
    public TileBoard tileBoard;
    public CanvasGroup gameOver;
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public void Init()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        bestScoreText.text = LoadBestScore().ToString();
        gameOver.alpha = 0;
        gameOver.interactable = false;
        tileBoard.ClearBoard();
        tileBoard.CreateTile();
        tileBoard.CreateTile();
        tileBoard.enabled = true;
    }
    public void GameOver()
    { 
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
}
