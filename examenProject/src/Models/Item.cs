public class Item
{
    public char Code { get; set; }
    public double Price { get; set; }
    public int Group { get; set; } // Group categorization (1-9)
    public double CampaignPrice { get; set; } // Price during campaigns
    public int MultipackQuantity { get; set; } // Quantity in a multipack (0 if not applicable)
    public char MultipackEquivalent { get; set; } // Equivalent single item for multipack (e.g., 'F' for 'R')
}
