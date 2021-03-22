namespace MyJetWallet.BitGo.Settings.Services
{
    public interface IAssetMapper
    {
        (string, string) AssetToBitgoCoinAndWallet(string brokerId, string assetSymbol);

        (string, string) BitgoCoinToAsset(string coin, string walletId);

        long ConvertAmountToBitgo(string coin, double amount);

        double ConvertAmountFromBitgo(string coin, long amount);
    }
}