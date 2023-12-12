public class SimplePriceCalculator
{
    private double total = 0.0;

    public void OnItemScanned(Item item)
    {
        // Use campaign price if available and lower than regular price
        double priceToUse = (item.CampaignPrice > 0 && item.CampaignPrice < item.Price) ? item.CampaignPrice : item.Price;
        total += priceToUse;
        Console.WriteLine($"Current Total: {total}");
    }
}
