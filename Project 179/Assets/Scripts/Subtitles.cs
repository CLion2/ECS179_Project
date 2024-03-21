using UnityEngine;
using TMPro; 

public class Subtitles : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitles;
    [SerializeField] string[] lines;
    public void UpdateText(int index)
    {
        subtitles.text = lines[index];
    }
}
