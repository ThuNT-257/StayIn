using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogPageSummaryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI summaryText;

    public void Setup(List<string> content) {
        summaryText.text = string.Join("\n\n", content);
        summaryText.pageToDisplay = 1;
        summaryText.ForceMeshUpdate();
    }

    public bool CanFlipNext() {
        return summaryText.pageToDisplay < summaryText.textInfo.pageCount;
    }

    public bool CanFlipBack() {
        return summaryText.pageToDisplay > 1;
    }

    public void FlipNext() {
        summaryText.pageToDisplay++;
    }

    public void FlipBack() {
        summaryText.pageToDisplay--;
    }

    public void FlipToLast() {
        summaryText.ForceMeshUpdate();
        summaryText.pageToDisplay = summaryText.textInfo.pageCount;
    }
}
