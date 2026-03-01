using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPlacementScript : MonoBehaviour
{
    [SerializeField] Image contentImage;
    [SerializeField] TextMeshProUGUI placementText;
    [SerializeField] Image placementIcon;
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] GameObject lina;
    [SerializeField] GameObject mirana;
    [SerializeField] TextMeshProUGUI dateOfScore;
    [SerializeField] TextMeshProUGUI timeSurvived;
    [SerializeField] TextMeshProUGUI difficulty;
    [SerializeField] TextMeshProUGUI score;


    public void Init(Color contentColor, int placement, Sprite placementIcon, string username, string heroName,
        string dateOfScore, string timeSurvived, string difficulty, string score, Sprite contentImageSprite, Color contentImageColor)
    {
        contentImage.color = contentColor;
        placementText.text = placement.ToString();
        this.placementIcon.sprite = placementIcon;
        if (heroName.Equals(CharacterSprite.LinaSprite.ToString()))
        {
            lina.SetActive(true);
            mirana.SetActive(false);
        }
        else if (heroName.Equals(CharacterSprite.MiranaSprite.ToString()))
        {
            lina.SetActive(false);
            mirana.SetActive(true);
        }
        this.username.text = username;
        this.dateOfScore.text = dateOfScore;
        this.timeSurvived.text = timeSurvived;
        this.difficulty.text = difficulty;
        this.score.text = score;
        this.contentImage.sprite = contentImageSprite;
        this.contentImage.color = contentImageColor;
        if (placement > 5)
        {
            this.username.color = Color.white;
            this.placementText.color = Color.white;
        }
    }
}
