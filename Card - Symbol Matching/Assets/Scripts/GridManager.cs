using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Grid Configuration")]
    public GridLayoutGroup gridLayoutGroup;
    
    public void SetupGridConstraint(CardData cardData)
    {
        if (cardData == null || gridLayoutGroup == null)
        {
            Debug.Log("CardData or GridLayoutGroup is missing!");
            return;
        }
        
        var symbolCount = cardData.symbols.Count;
        var totalCards = symbolCount * 2;
        
        int columns = CalculateOptimalColumns(symbolCount, totalCards);
        
        // Setting the constraint count based on symbol count
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columns;
    }

    private int CalculateOptimalColumns(int symbolCount, int totalCards)
    {
        // For 5 or fewer symbols, use the symbol count as columns
        if (symbolCount <= 5)
        {
            return symbolCount;
        }
        
        var bestColumns = symbolCount;
        var bestRatio = float.MaxValue;
        
        var sqrtCards = Mathf.RoundToInt(Mathf.Sqrt(totalCards));
        
        var minCols = Mathf.Max(2, sqrtCards - 2);
        var maxCols = Mathf.Min(symbolCount, sqrtCards + 3);
        
        for (var cols = minCols; cols <= maxCols; cols++)
        {
            var rows = Mathf.CeilToInt((float)totalCards / cols);
            
            if (cols * rows - totalCards > cols - 1) continue;
        
            var ratio = Mathf.Max((float)cols / rows, (float)rows / cols);

            if (ratio < bestRatio || (Mathf.Approximately(ratio, bestRatio) && cols > bestColumns))
            {
                bestRatio = ratio;
                bestColumns = cols;
            }
        }

        return bestColumns;
    }
}
