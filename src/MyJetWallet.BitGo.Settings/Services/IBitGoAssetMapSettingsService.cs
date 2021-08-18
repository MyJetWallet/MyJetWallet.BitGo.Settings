using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;

namespace MyJetWallet.BitGo.Settings.Services
{
    public interface IBitGoAssetMapSettingsService
    {
        ValueTask<bool> CreateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId,
            string enabledBitgoWalletIds,
            string bitgoCoin, 
            double minBalance);

        ValueTask<bool> UpdateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId,
            string enabledBitgoWalletIds,
            string bitgoCoin, 
            double minBalance);

        ValueTask<bool> DeleteBitgoAssetMapEntityAsync(string brokerId, string assetSymbol);

        ValueTask<BitgoAssetMapEntity[]> GetAllAssetMapsAsync();
    }
}