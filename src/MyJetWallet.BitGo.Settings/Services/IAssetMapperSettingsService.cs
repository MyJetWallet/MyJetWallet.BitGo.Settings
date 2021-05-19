using System.Collections.Generic;
using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;

namespace MyJetWallet.BitGo.Settings.Services
{
    public interface IAssetMapperSettingsService
    {
        ValueTask<(bool, string)> CreateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId,
            string bitgoCoin);

        ValueTask<(bool, string)> UpdateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId,
            string bitgoCoin);

        ValueTask<(bool, string)> DeleteBitgoAssetMapEntityAsync(string brokerId, string assetSymbol);

        ValueTask<BitgoAssetMapEntity[]> GetAllAssetMapsAsync();
    }
}