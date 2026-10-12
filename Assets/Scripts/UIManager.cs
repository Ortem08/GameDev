using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private DiceMixer diceMixer;
    [SerializeField] private Button throwButton;
    [SerializeField] private TMP_InputField dieCountInput;
    [SerializeField] private TMP_InputField scoreAimInput;
    [SerializeField] private TMP_Text dieCountText;
    [SerializeField] private TMP_Text scoreAimText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text winText;

    private void Awake()
    {
        if (diceMixer is null)
        {
            Debug.LogError($"{nameof(diceMixer)} is null");
            return;
        }
        
        throwButton.onClick.AddListener(diceMixer.ThrowDice);
        throwButton.onClick.AddListener(ClearWinText);
        dieCountInput.onEndEdit.AddListener(diceMixer.SetDieCount);
        scoreAimInput.onEndEdit.AddListener(diceMixer.SetScoreAim);

        diceMixer.OnDieCountChange += SetDieCountText;
        diceMixer.OnScoreAimChange += SetScoreAimText;
        diceMixer.OnScoreChange += SetScoreText;
    }

    private void ClearWinText()
    {
        winText.text = "";
    }

    private void SetDieCountText(int dieCount)
    {
        dieCountText.text = $"{StringConstants.DieCountFieldName}: {dieCount}";
    }
    
    private void SetScoreAimText(int scoreAim)
    {
        scoreAimText.text = $"{StringConstants.ScoreAimFieldName}: {scoreAim}";
    }

    private void SetScoreText(int score, RoundResult roundResult)
    {
        scoreText.text = $"{StringConstants.ScoreFieldName}: {score}";
        winText.text = roundResult.GetDescription();
    }
}