public class ComplexPriceCalculator
{
    private Dictionary<char, List<Item>> groupedItems = new Dictionary<char, List<Item>>();

    public void OnItemScanned(Item item)
    {
        if (!groupedItems.ContainsKey(item.Code))
        {
            groupedItems[item.Code] = new List<Item>();
        }
        groupedItems[item.Code].Add(item);

        // Additional logic for multipack and campaign prices
    }

    public void DisplaySummary()
    {
        foreach (var group in groupedItems)
{
    var groupCode = group.Key;
    var itemsInGroup = group.Value;

    // Calculate total price for this group, considering multipacks
    double groupTotal = 0.0;
    int itemCount = itemsInGroup.Count;

    if (itemsInGroup[0].MultipackQuantity > 0 && itemCount >= itemsInGroup[0].MultipackQuantity)
    {
        // Calculate multipack price
        int multipacks = itemCount / itemsInGroup[0].MultipackQuantity;
        groupTotal += multipacks * itemsInGroup[0].CampaignPrice;

        // Remaining items at standard price
        int remainingItems = itemCount % itemsInGroup[0].MultipackQuantity;
        groupTotal += remainingItems * itemsInGroup[0].Price;
    }
    else
    {
        // Total price at standard rate
        groupTotal = itemsInGroup.Sum(item => item.Price);
    }

    Console.WriteLine($"Group {groupCode}: Total = {groupTotal}");
}

    }
}
