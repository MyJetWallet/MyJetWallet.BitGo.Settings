using System;
using MyNoSqlServer.Abstractions;

namespace MyJetWallet.BitGo.Settings.NoSql
{
    public class BitgoCoinEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-bitgo-coin";

        public static string GeneratePartitionKey() => "coins";
        public static string GenerateRowKey(string coin) => coin;

        public string Coin { get; set; }
        public int Accuracy { get; set; }
        public Double Divider { get; set; }
        public int RequiredConfirmations { get; set; }

        public static BitgoCoinEntity Create(string coin, int accuracy, int requiredConfirmations)
        {
            var entity = new BitgoCoinEntity()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(coin),
                Coin = coin,
                Accuracy = accuracy,
                Divider = Math.Pow(10, accuracy),
                RequiredConfirmations = requiredConfirmations
            };

            return entity;
        }

        public long AmountToAbsoluteValue(double amount)
        {
            return (long) Math.Round(amount * Divider, 0);
        }

        public double AmountFromAbsoluteValue(long value)
        {
            var res = value / Divider;
            return Math.Round(res, Accuracy);
        }
    }
}