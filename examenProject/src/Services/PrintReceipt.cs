public class PrintReceipt
{
    const double PantValue = 1.50;
    private Dictionary<char, Item> itemsDatabase;
    private Dictionary<char, int> itemQuantities;

    public PrintReceipt(Dictionary<char, Item> itemsDatabase, Dictionary<char, int> itemQuantities)
    {
        this.itemsDatabase = itemsDatabase;
        this.itemQuantities = itemQuantities;
    }

    public void PrintToConsole()
    {
        double total = CalculateTotal();
        double totalSavings = CalculateTotalSavings();
        Console.WriteLine("\nReceipt:");

        foreach (var item in itemQuantities.OrderBy(x => itemsDatabase[x.Key].Price))
        {
            var itemDetail = itemsDatabase[item.Key];
            double totalPrice = itemDetail.Price * item.Value;
            Console.WriteLine($"Item: {itemDetail.Code}, Quantity: {item.Value}, Price: {totalPrice.ToString("F2")}kr");
        }

        Console.WriteLine(); // Blank line
        Console.WriteLine($"Discount: {totalSavings.ToString("F2")}kr");
        Console.WriteLine($"Total Price: {total.ToString("F2")}kr");
    }

    public void PrintToFile()
    {
        double total = CalculateTotal();
        double totalSavings = CalculateTotalSavings();
        using (StreamWriter file = new StreamWriter("Receipt.txt"))
        {
            file.WriteLine("Receipt:\n");

            var groupedItems = itemQuantities
                                .GroupBy(item => itemsDatabase[item.Key].Group)
                                .OrderBy(group => group.Key);

            foreach (var group in groupedItems)
            {
                file.WriteLine($"Group {group.Key}:");
                foreach (var item in group.OrderBy(x => itemsDatabase[x.Key].Price))
                {
                    var itemDetail = itemsDatabase[item.Key];
                    double totalPrice = itemDetail.Price * item.Value;
                    file.WriteLine($" - Item: {itemDetail.Code}, Quantity: {item.Value}, Price: {totalPrice.ToString("F2")}kr");
                }
                file.WriteLine(); // Blank line after each group
            }

            file.WriteLine($"Discount: {totalSavings.ToString("F2")}kr");
            file.WriteLine($"Total Price: {total.ToString("F2")}kr");
        }
    }

    private double CalculateTotal()
    {
        double total = 0;
        foreach (var item in itemQuantities)
        {
            var itemDetail = itemsDatabase[item.Key];
            double totalPrice = itemDetail.Price * item.Value;

            if (itemDetail.Group == 2) // If the item is in group 2 (drinks)
            {
                totalPrice += PantValue * item.Value; // Add pant value to the total price
            }

            total += totalPrice;
        }
        return total;
    }

    private double CalculateTotalSavings()
    {
        double savings = 0;
        foreach (var item in itemQuantities)
        {
            var itemDetail = itemsDatabase[item.Key];
            double standardPrice = itemDetail.Price * item.Value;
            double discountedPrice = CalculateDiscountedPrice(item.Key, item.Value, itemDetail);
            savings += standardPrice - discountedPrice;
        }
        return savings;
    }

    private double CalculateDiscountedPrice(char itemCode, int quantity, Item itemDetail)
    {
        double price = itemDetail.Price;
        if (itemDetail.MultipackQuantity > 0 && quantity >= itemDetail.MultipackQuantity)
        {
            price = itemDetail.CampaignPrice / itemDetail.MultipackQuantity;
        }
        else if (itemDetail.CampaignPrice > 0 && itemDetail.CampaignPrice < itemDetail.Price)
        {
            price = itemDetail.CampaignPrice;
        }
        return price * quantity;
    }
}
