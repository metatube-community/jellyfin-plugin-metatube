namespace Jellyfin.Plugin.MetaTube.Helpers;

public static class Levenshtein
{
    public static int Distance(string value1, string value2)
    {
        if (value2.Length == 0)
        {
            return value1.Length;
        }

        int[] costs = new int[value2.Length];

        for (int i = 0; i < costs.Length;)
        {
            costs[i] = ++i;
        }

        for (int i = 0; i < value1.Length; i++)
        {
            int cost = i;
            int previousCost = i;

            char value1Char = value1[i];

            for (int j = 0; j < value2.Length; j++)
            {
                int currentCost = cost;

                cost = costs[j];

                if (value1Char != value2[j])
                {
                    if (previousCost < currentCost)
                    {
                        currentCost = previousCost;
                    }

                    if (cost < currentCost)
                    {
                        currentCost = cost;
                    }

                    ++currentCost;
                }

                costs[j] = currentCost;
                previousCost = currentCost;
            }
        }

        return costs[costs.Length - 1];
    }
}