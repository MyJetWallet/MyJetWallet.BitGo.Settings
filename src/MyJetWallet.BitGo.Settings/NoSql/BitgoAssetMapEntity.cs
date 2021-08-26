using System.Linq;
using MyNoSqlServer.Abstractions;

namespace MyJetWallet.BitGo.Settings.NoSql
{
    public class BitgoAssetMapEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-map-asset-to-bitgo";

        public static string GeneratePartitionKey(string brokerId) => $"broker:{brokerId}";
        public static string GenerateRowKey(string assetSymbol) => $"asset:{assetSymbol}";

        public string BrokerId { get; set; }
        public string AssetSymbol { get; set; }
        public string BitgoWalletId { get; set; }
        public string EnabledBitgoWalletIds { get; set; }
        public string BitgoCoin { get; set; }
        
        public double MinBalance { get; set; }

        public string TagSeparator { get; set; }

        public static BitgoAssetMapEntity Create(string brokerId, string assetSymbol, string bitgoWalletId,
            string enabledBitgoWalletIds, string bitgoCoin, double minBalance, string tagSeparator = null)
        {
            var entity = new BitgoAssetMapEntity()
            {
                PartitionKey = GeneratePartitionKey(brokerId),
                RowKey = GenerateRowKey(assetSymbol),
                BrokerId = brokerId,
                AssetSymbol = assetSymbol,
                BitgoCoin = bitgoCoin,
                BitgoWalletId = bitgoWalletId,
                EnabledBitgoWalletIds = enabledBitgoWalletIds,
                MinBalance = minBalance,
                TagSeparator = tagSeparator
            };

            if (!entity.EnabledBitgoWalletIds.Contains(entity.BitgoWalletId))
            {
                entity.EnabledBitgoWalletIds += ";" + entity.BitgoWalletId;
            }

            return entity;
        }

        public bool IsWalletEnabled(string bitgoWalletId)
        {
            return EnabledBitgoWalletIds.Split(";").ToList().Contains(bitgoWalletId);
        }
    }
}