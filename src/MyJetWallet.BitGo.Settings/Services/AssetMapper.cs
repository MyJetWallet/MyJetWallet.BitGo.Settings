using System;
using System.Linq;
using MyJetWallet.BitGo.Settings.NoSql;
using MyNoSqlServer.Abstractions;
using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace MyJetWallet.BitGo.Settings.Services
{
    public class AssetMapper : IAssetMapper
    {
        private readonly IMyNoSqlServerDataReader<BitgoAssetMapEntity> _assetMap;
        private readonly IMyNoSqlServerDataReader<BitgoCoinEntity> _bitgoCoins;

        public AssetMapper(IMyNoSqlServerDataReader<BitgoAssetMapEntity> assetMap,
            IMyNoSqlServerDataReader<BitgoCoinEntity> bitgoCoins)
        {
            _assetMap = assetMap;
            _bitgoCoins = bitgoCoins;
        }

        public (string, string) AssetToBitgoCoinAndWallet(string brokerId, string assetSymbol)
        {
            var map = _assetMap.Get(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                BitgoAssetMapEntity.GenerateRowKey(assetSymbol));

            if (map == null)
            {
                return (string.Empty, string.Empty);
            }

            return (map.BitgoCoin, map.BitgoWalletId);
        }

        public (string, string) BitgoCoinToAsset(string coin, string walletId)
        {
            var entities = _assetMap.Get().Where(e => e.BitgoWalletId == walletId && e.BitgoCoin == coin).ToList();

            if (!entities.Any())
            {
                return (string.Empty, string.Empty);
            }

            if (entities.Count > 1)
            {
                throw new Exception(
                    $"Cannot map BitGo wallet {walletId} coin {coin} to Asset. Table: {BitgoAssetMapEntity.TableName}. Find many assets: {JsonConvert.SerializeObject(entities)}");
            }

            var entity = entities.First();

            return (entity.BrokerId, entity.AssetSymbol);
        }

        public bool IsWalletEnabled(string coin, string bitgoWalletId)
        {
            var entities = _assetMap.Get().Where(e => e.BitgoCoin == coin).ToList();

            if (!entities.Any())
            {
                return false;
            }

            if (entities.Count > 1)
            {
                throw new Exception(
                    $"Cannot map BitGo coin {coin} to Asset. Table: {BitgoAssetMapEntity.TableName}. Find many assets: {JsonConvert.SerializeObject(entities)}");
            }

            var entity = entities.First();

            return entity.IsWalletEnabled(bitgoWalletId);
        }
        public decimal ConvertAmountToBitgo(string coin, double amount)
        {
            var coinSettings = _bitgoCoins.Get(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(coin));

            if (coinSettings == null)
            {
                throw new Exception(
                    $"Do not found settings for bitgo coin {coin} in nosql table {BitgoCoinEntity.TableName}");
            }

            return coinSettings.AmountToAbsoluteValueDecimal(amount);
        }

        public double ConvertAmountFromBitgo(string coin, decimal amount)
        {
            var coinSettings = _bitgoCoins.Get(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(coin));

            if (coinSettings == null)
            {
                throw new Exception(
                    $"Do not found settings for bitgo coin {coin} in nosql table {BitgoCoinEntity.TableName}");
            }

            return coinSettings.AmountFromAbsoluteValueDecimal(amount);
        }

        public int GetRequiredConfirmations(string coin)
        {
            var coinSettings = _bitgoCoins.Get(BitgoCoinEntity.GeneratePartitionKey(),
                BitgoCoinEntity.GenerateRowKey(coin));

            if (coinSettings == null)
            {
                throw new Exception(
                    $"Do not found settings for bitgo coin {coin} in nosql table {BitgoCoinEntity.TableName}");
            }

            return coinSettings.RequiredConfirmations;
        }

        public string GetTagSeparator(string brokerId, string assetSymbol)
        {
            var map = _assetMap.Get(BitgoAssetMapEntity.GeneratePartitionKey(brokerId),
                BitgoAssetMapEntity.GenerateRowKey(assetSymbol));

            if (map == null)
            {
                return string.Empty;
            }

            return map.TagSeparator;
        }
    }
}