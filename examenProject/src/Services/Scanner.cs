public class Scanner
{
    public delegate void ItemScannedEventHandler(char itemCode);
    public event ItemScannedEventHandler? ItemScanned;
    public Action? OnScanningComplete;

    public async Task ScanItemAsync()
    {
        while (true)
        {
            Console.WriteLine("Scan an item (Enter item code, 'X' to stop):");

            char itemCode = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Move to next line

            if (itemCode == 'X' || itemCode == 'x')
            {
                break; // Exit the loop if 'X' is pressed
            }

            // Trigger the event
            ItemScanned?.Invoke(itemCode);

            await Task.Delay(500); // Delay for the next scan
        }

        OnScanningComplete?.Invoke(); // Call the action after scanning is complete
    }
}
