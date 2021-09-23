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

        public bool IsMainNet { get; set; } = false;

        public static BitgoCoinEntity Create(string coin, int accuracy, int requiredConfirmations, bool isMainNet)
        {
            var entity = new BitgoCoinEntity()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(coin),
                Coin = coin,
                Accuracy = accuracy,
                Divider = Math.Pow(10, accuracy),
                RequiredConfirmations = requiredConfirmations,
                IsMainNet = isMainNet
            };

            return entity;
        }

        public long AmountToAbsoluteValue(double amount)
        {
            return decimal.ToInt64(decimal.Round(new decimal(amount * Divider), 0));
        }
        public decimal AmountToAbsoluteValueDecimal(double amount)
        {
            return decimal.Round(new decimal(amount * Divider), 0);
        }
        public double AmountFromAbsoluteValue(long value)
        {
            return decimal.ToDouble(decimal.Round(new decimal(value / Divider), Accuracy));
        }

        public double AmountFromAbsoluteValueDecimal(decimal value)
        {
            return decimal.ToDouble(decimal.Round(value / new decimal(Divider), Accuracy));
        }
    }
}