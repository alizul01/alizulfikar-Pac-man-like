using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private int score = 0;
        [SerializeField] private int maxScore = 0;
        
        private void UpdateUI()
        {
            scoreText.text = "Score: " + score + "/" + maxScore;
        }
        
        public void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
            UpdateUI();
        }
        
        public void SetMaxScore(int value)
        {
            maxScore = value;
            UpdateUI();
        }
        
        public void ResetScore()
        {
            score = 0;
            UpdateUI();
        }

        private void Start()
        {
            if (scoreText == null)
            {
                Debug.LogError("Score text is null");
            }
            UpdateUI();
        }
    }
}