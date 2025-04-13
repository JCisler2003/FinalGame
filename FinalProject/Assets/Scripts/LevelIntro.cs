using TMPro;
using UnityEngine;

public class LevelIntro : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public float displayDuration = 2f;

    void Start()
    {
        if (levelText != null)
        {
            levelText.gameObject.SetActive(true);
            Invoke(nameof(HideText), displayDuration);
        }
    }

    void HideText()
    {
        levelText.gameObject.SetActive(false);
    }
}

