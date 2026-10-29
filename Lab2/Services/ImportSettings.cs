namespace Lab2.Services;

public class ImportSettings
{
    private static ImportSettings? _instance;

    private ImportSettings() { }

    public static ImportSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ImportSettings();
            }
            return _instance;
        }
    }

    public decimal MdlPerEur { get; } = 19.9863m;
    public decimal ShippingEur { get; } = 900m;
    public decimal ProcedureFeeRate { get; } = 0.004m;
}
