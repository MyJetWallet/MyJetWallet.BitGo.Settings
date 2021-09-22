namespace MyJetWallet.BitGo.Settings.Services
{
    public interface IAssetMapper
    {
        (string, string) AssetToBitgoCoinAndWallet(string brokerId, string assetSymbol);

        (string, string) BitgoCoinToAsset(string coin, string walletId);

        bool IsWalletEnabled(string coin, string bitgoWalletId);

        decimal ConvertAmountToBitgo(string coin, double amount);

        double ConvertAmountFromBitgo(string coin, decimal amount);

        int GetRequiredConfirmations(string coin);

        string GetTagSeparator(string brokerId, string assetSymbol);
    }
}