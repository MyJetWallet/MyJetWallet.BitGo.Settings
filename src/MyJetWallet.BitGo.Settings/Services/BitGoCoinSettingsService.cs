using System;
using System.Linq;
using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;
using MyNoSqlServer.Abstractions;

namespace MyJetWallet.BitGo.Settings.Services
{
    public class BitGoCoinSettingsService : IBitGoCoinSettingsService
    {
        private readonly IMyNoSqlServerDataWriter<BitgoAssetMapEntity> _assetMap;
        private readonly IMyNoSqlServerDataWriter<BitgoCoinEntity> _bitgoCoins;

        public BitGoCoinSettingsService(IMyNoSqlServerDataWriter<BitgoAssetMapEntity> assetMap,
            IMyNoSqlServerDataWriter<BitgoCoinEntity> bitgoCoins)
        {
            _assetMap = assetMap;
            _bitgoCoins = bitgoCoins;
        }

        public async ValueTask<bool> CreateBitgoCoinEntityAsync(string coin, int accuracy, int requiredConfirmations)
        {
            if (string.IsNullOrEmpty(coin)) throw new Exception("Cannot create coin. Coin cannot be empty");
            if (accuracy < 0) throw new Exception("Cannot create coin. Accuracy can't be less then 0");
            if (requiredConfirmations < 0)
                throw new Exception("Cannot create coin. RequiredConfirmations can't be less then 0");

            var entity = BitgoCoinEntity.Create(coin, accuracy, requiredConfirmations);

            var existingItem = await _bitgoCoins.GetAsync(entity.PartitionKey, entity.RowKey);
            if (existingItem != null) throw new Exception("Cannot create coin. Already exist");

            await _bitgoCoins.InsertAsync(entity);

            return true;
        }

        public async ValueTask<bool> UpdateBitgoCoinEntityAsync(string coin, int accuracy, int requiredConfirmations)
        {
            if (string.IsNullOrEmpty(coin)) throw new Exception("Cannot update coin. Coin cannot be empty");
            if (accuracy < 0) throw new Exception("Cannot update coin. Accuracy can't be less then 0");
            if (requiredConfirmations < 0)
                throw new Exception("Cannot update coin. RequiredConfirmations can't be less then 0");

            var entity = BitgoCoinEntity.Create(coin, accuracy, requiredConfirmations);

            var existingItem = await _bitgoCoins.GetAsync(entity.PartitionKey, entity.RowKey);
            if (existingItem == null) throw new Exception("Cannot update coin. Coin not found");

            await _bitgoCoins.InsertOrReplaceAsync(entity);

            return true;
        }

        public async ValueTask<bool> DeleteBitgoCoinEntityAsync(string coin)
        {
            if (string.IsNullOrEmpty(coin)) throw new Exception("Cannot update coin. Coin cannot be empty");

            var entity = await _bitgoCoins.GetAsync(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(coin));

            if (entity != null)
            {
                var assets = await _assetMap.GetAsync();
                var existAssets = assets.Any(e => e.BitgoCoin == coin);
                if (existAssets) throw new Exception("Cannot delete coin. Asset used it as coin");

                await _bitgoCoins.DeleteAsync(BitgoCoinEntity.GeneratePartitionKey(),
                    BitgoCoinEntity.GenerateRowKey(coin));
            }

            return true;
        }

        public async ValueTask<BitgoCoinEntity[]> GetAllCoinsAsync()
        {
            var entities = await _bitgoCoins.GetAsync();
            return entities.ToArray();
        }
    }
}