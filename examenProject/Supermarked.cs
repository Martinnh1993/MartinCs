class Supermarket
{
    private static Dictionary<char, Item> itemsDatabase = new Dictionary<char, Item>();
    private static Dictionary<char, int> itemQuantities = new Dictionary<char, int>();
    private static double runningTotal = 0.0;

    static Supermarket()
    {
        // Initialize items database
        itemsDatabase.Add('A', new Item { Code = 'A', Price = 2.50, Group = 1 });
        itemsDatabase.Add('B', new Item { Code = 'B', Price = 3.00, Group = 2, MultipackQuantity = 6, CampaignPrice = 15.00 }); // 6-pack with discount
        itemsDatabase.Add('C', new Item { Code = 'C', Price = 1.75, Group = 3 });
        itemsDatabase.Add('D', new Item { Code = 'D', Price = 0.99, Group = 1 });
        itemsDatabase.Add('E', new Item { Code = 'E', Price = 5.00, Group = 2 ,MultipackQuantity = 2, CampaignPrice = 9.00});
        itemsDatabase.Add('F', new Item { Code = 'F', Price = 4.25, Group = 4 });
        itemsDatabase.Add('G', new Item { Code = 'G', Price = 2.10, Group = 3 });
        itemsDatabase.Add('H', new Item { Code = 'H', Price = 3.20, Group = 5 });
        itemsDatabase.Add('I', new Item { Code = 'I', Price = 6.00, Group = 1, MultipackQuantity = 3, CampaignPrice = 17.00 }); // 3-pack with discount
        itemsDatabase.Add('J', new Item { Code = 'J', Price = 7.00, Group = 2 });
        itemsDatabase.Add('K', new Item { Code = 'K', Price = 3.75, Group = 2 });
        itemsDatabase.Add('L', new Item { Code = 'L', Price = 1.50, Group = 4 });
        itemsDatabase.Add('M', new Item { Code = 'M', Price = 2.30, Group = 3, MultipackQuantity = 10, CampaignPrice = 20.00});
        itemsDatabase.Add('N', new Item { Code = 'N', Price = 4.50, Group = 5 });
        itemsDatabase.Add('O', new Item { Code = 'O', Price = 5.25, Group = 1 });
        itemsDatabase.Add('P', new Item { Code = 'P', Price = 6.75, Group = 2, MultipackQuantity = 4, CampaignPrice = 24.00 }); // 4-pack with discount
        itemsDatabase.Add('Q', new Item { Code = 'Q', Price = 7.50, Group = 3 });

    }

    static async Task Main(string[] args)
    {
        SimplePriceCalculator simpleCalculator = new SimplePriceCalculator();
        ComplexPriceCalculator complexCalculator = new ComplexPriceCalculator();
        PrintReceipt printReceipt = new PrintReceipt(itemsDatabase, itemQuantities);

        Scanner scanner = new Scanner();
        scanner.ItemScanned += itemCode =>
        {
            if (itemCode == 'X' || itemCode == 'x')
            {
                printReceipt.PrintToConsole(); // Print to console
                printReceipt.PrintToFile();    // Print to file
                Environment.Exit(0);
            }

            if (itemsDatabase.TryGetValue(itemCode, out Item? scannedItem) && scannedItem != null)
            {
                double priceToAdd = scannedItem.Price;

                // Update item quantities
                if (!itemQuantities.ContainsKey(itemCode))
                {
                    itemQuantities[itemCode] = 0;
                }
                itemQuantities[itemCode]++;

                // Check for multipack discount
                if (scannedItem.MultipackQuantity > 0 && itemQuantities[itemCode] % scannedItem.MultipackQuantity == 0)
                {
                    priceToAdd = scannedItem.CampaignPrice - (scannedItem.MultipackQuantity - 1) * scannedItem.Price;
                }
                // Check for campaign price
                else if (scannedItem.CampaignPrice > 0 && scannedItem.CampaignPrice < scannedItem.Price)
                {
                    priceToAdd = scannedItem.CampaignPrice;
                }

                runningTotal += priceToAdd; // Update running total
                simpleCalculator.OnItemScanned(scannedItem);
                complexCalculator.OnItemScanned(scannedItem);

                Console.WriteLine($"Item scanned: {scannedItem.Code}, Price: {scannedItem.Price}");
            }
            else
            {
                Console.WriteLine("Unknown item scanned.");
            }
        };

        scanner.OnScanningComplete = () =>
        {
            printReceipt.PrintToConsole(); // Print to console
            printReceipt.PrintToFile();    // Print to file
        };

        // Start scanning items
        await scanner.ScanItemAsync();
    }
}