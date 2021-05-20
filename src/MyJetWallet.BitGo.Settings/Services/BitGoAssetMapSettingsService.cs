using System;
using System.Linq;
using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;
using MyNoSqlServer.Abstractions;

namespace MyJetWallet.BitGo.Settings.Services
{
    public class BitGoAssetMapSettingsService : IBitGoAssetMapSettingsService
    {
        private readonly IMyNoSqlServerDataWriter<BitgoAssetMapEntity> _assetMap;
        private readonly IMyNoSqlServerDataWriter<BitgoCoinEntity> _bitgoCoins;

        public BitGoAssetMapSettingsService(IMyNoSqlServerDataWriter<BitgoAssetMapEntity> assetMap,
            IMyNoSqlServerDataWriter<BitgoCoinEntity> bitgoCoins)
        {
            _assetMap = assetMap;
            _bitgoCoins = bitgoCoins;
        }

        public async ValueTask<bool> CreateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId, string enabledBitgoWalletIds, string bitgoCoin)
        {
            if (string.IsNullOrEmpty(brokerId))
                throw new Exception("Cannot create asset map. BrokerId cannot be empty");
            if (string.IsNullOrEmpty(assetSymbol))
                throw new Exception("Cannot create asset map. AssetSymbol cannot be empty");
            if (string.IsNullOrEmpty(bitgoWalletId))
                throw new Exception("Cannot create asset map. BitgoWalletId cannot be empty");
            if (string.IsNullOrEmpty(bitgoCoin))
                throw new Exception("Cannot create asset map. BitgoCoin cannot be empty");

            var coin = await _bitgoCoins.GetAsync(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(bitgoCoin));
            if (coin == null) throw new Exception("Cannot create asset map. Unknown BitgoCoin.");

            var entity =
                BitgoAssetMapEntity.Create(brokerId, assetSymbol, bitgoWalletId, enabledBitgoWalletIds, bitgoCoin);

            var existingItem = await _assetMap.GetAsync(entity.PartitionKey, entity.RowKey);
            if (existingItem != null) throw new Exception("Cannot create asset map. Already exist");

            await _assetMap.InsertAsync(entity);

            return true;
        }

        public async ValueTask<bool> UpdateBitgoAssetMapEntityAsync(string brokerId, string assetSymbol,
            string bitgoWalletId, string enabledBitgoWalletIds, string bitgoCoin)
        {
            if (string.IsNullOrEmpty(brokerId))
                throw new Exception("Cannot update asset map. BrokerId cannot be empty");
            if (string.IsNullOrEmpty(assetSymbol))
                throw new Exception("Cannot update asset map. AssetSymbol cannot be empty");
            if (string.IsNullOrEmpty(bitgoWalletId))
                throw new Exception("Cannot update asset map. BitgoWalletId cannot be empty");
            if (string.IsNullOrEmpty(bitgoCoin))
                throw new Exception("Cannot update asset map. BitgoCoin cannot be empty");

            var coin = await _bitgoCoins.GetAsync(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(bitgoCoin));
            if (coin == null) throw new Exception("Cannot update asset map. Unknown BitgoCoin.");

            var entity = BitgoAssetMapEntity.Create(brokerId, assetSymbol, bitgoWalletId,
                enabledBitgoWalletIds, bitgoCoin);

            var existingEntity = await _assetMap.GetAsync(entity.PartitionKey, entity.RowKey);
            if (existingEntity == null) throw new Exception("Cannot update asset map. Asset map not found");

            await _assetMap.InsertOrReplaceAsync(entity);

            return true;
        }

        public async ValueTask<bool> DeleteBitgoAssetMapEntityAsync(string brokerId, string assetSymbol)
        {
            if (string.IsNullOrEmpty(brokerId))
                throw new Exception("Cannot delete asset map. BrokerId cannot be empty");
            if (string.IsNullOrEmpty(assetSymbol))
                throw new Exception("Cannot delete asset map. AssetSymbol cannot be empty");

            var entity = await _assetMap.GetAsync(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                BitgoAssetMapEntity.GenerateRowKey(assetSymbol));

            if (entity != null)
            {
                await _assetMap.DeleteAsync(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                    BitgoAssetMapEntity.GenerateRowKey(assetSymbol));
            }

            return true;
        }

        public async ValueTask<BitgoAssetMapEntity[]> GetAllAssetMapsAsync()
        {
            var entities = await _assetMap.GetAsync();
            return entities.ToArray();
        }
    }
}