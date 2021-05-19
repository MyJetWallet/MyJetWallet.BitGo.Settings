using System.Linq;
using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;
using MyNoSqlServer.Abstractions;

namespace MyJetWallet.BitGo.Settings.Services
{
    public class AssetMapperSettingsService : IAssetMapperSettingsService
    {
        private readonly IMyNoSqlServerDataWriter<BitgoAssetMapEntity> _assetMap;
        private readonly IMyNoSqlServerDataWriter<BitgoCoinEntity> _bitgoCoins;

        public AssetMapperSettingsService(IMyNoSqlServerDataWriter<BitgoAssetMapEntity> assetMap,
            IMyNoSqlServerDataWriter<BitgoCoinEntity> bitgoCoins)
        {
            _assetMap = assetMap;
            _bitgoCoins = bitgoCoins;
        }

        public async ValueTask<(bool, string)> CreateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId, string bitgoCoin)
        {
            if (string.IsNullOrEmpty(brokerId)) return (false, "Cannot create asset map. BrokerId cannot be empty");
            if (string.IsNullOrEmpty(assetSymbol))
                return (false, "Cannot create asset map. AssetSymbol cannot be empty");
            if (string.IsNullOrEmpty(bitgoWalletId))
                return (false, "Cannot create asset map. BitgoWalletId cannot be empty");
            if (string.IsNullOrEmpty(bitgoCoin)) return (false, "Cannot create asset map. BitgoCoin cannot be empty");

            var coin = await _bitgoCoins.GetAsync(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(bitgoCoin));
            if (coin == null) return (false, "Cannot create asset map. Unknown BitgoCoin.");

            var entity = BitgoAssetMapEntity.Create(brokerId, assetSymbol, bitgoWalletId, bitgoCoin);

            var existingItem = await _assetMap.GetAsync(entity.PartitionKey, entity.RowKey);
            if (existingItem != null) return (false, "Cannot create asset map. Already exist");

            await _assetMap.InsertAsync(entity);

            return (true, null);
        }

        public async ValueTask<(bool, string)> UpdateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId, string bitgoCoin)
        {
            if (string.IsNullOrEmpty(brokerId)) return (false, "Cannot update asset map. BrokerId cannot be empty");
            if (string.IsNullOrEmpty(assetSymbol))
                return (false, "Cannot update asset map. AssetSymbol cannot be empty");
            if (string.IsNullOrEmpty(bitgoWalletId))
                return (false, "Cannot update asset map. BitgoWalletId cannot be empty");
            if (string.IsNullOrEmpty(bitgoCoin)) return (false, "Cannot update asset map. BitgoCoin cannot be empty");

            var coin = await _bitgoCoins.GetAsync(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(bitgoCoin));
            if (coin == null) return (false, "Cannot update asset map. Unknown BitgoCoin.");

            var entity = await _assetMap.GetAsync(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                BitgoAssetMapEntity.GenerateRowKey(assetSymbol));
            if (entity == null) return (false, "Cannot update asset map. Asset map not found");

            entity.BitgoWalletId = bitgoWalletId;
            entity.BrokerId = bitgoCoin;

            await _assetMap.InsertOrReplaceAsync(entity);

            return (true, null);
        }

        public async ValueTask<(bool, string)> DeleteBitgoAssetMapEntityAsync(string brokerId, string assetSymbol)
        {
            if (string.IsNullOrEmpty(brokerId)) return (false, "Cannot delete asset map. BrokerId cannot be empty");
            if (string.IsNullOrEmpty(assetSymbol))
                return (false, "Cannot delete asset map. AssetSymbol cannot be empty");

            var entity = await _assetMap.GetAsync(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                BitgoAssetMapEntity.GenerateRowKey(assetSymbol));

            if (entity != null)
            {
                await _assetMap.DeleteAsync(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                    BitgoAssetMapEntity.GenerateRowKey(assetSymbol));
            }

            return (true, null);
        }

        public async ValueTask<BitgoAssetMapEntity[]> GetAllAssetMapsAsync()
        {
            var entities = await _assetMap.GetAsync();
            return entities.ToArray();
        }
    }
}