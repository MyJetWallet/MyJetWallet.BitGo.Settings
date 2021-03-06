using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;
using MyNoSqlServer.DataWriter;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var Divider = Math.Pow(10, 18);


            Console.WriteLine(decimal.ToDouble(decimal.Round(new decimal(2000000000000000000 / Divider), 18)));
            Console.WriteLine(decimal.ToInt64(decimal.Round(new decimal(2 * Divider), 0)));
            
            // Console.Write("Press enter to start");
            // Console.ReadLine();
            //
            // await GenerateAsstToBitgoMap();
            //
            //
            // MyNoSqlTcpClient myNoSqlClient =
            //     new MyNoSqlTcpClient(() => "192.168.10.80:5125", "test-bitgo-settings-app");
            //
            // var mapper = new AssetMapper(
            //     new MyNoSqlReadRepository<BitgoAssetMapEntity>(myNoSqlClient, BitgoAssetMapEntity.TableName),
            //     new MyNoSqlReadRepository<BitgoCoinEntity>(myNoSqlClient, BitgoCoinEntity.TableName));
            //
            // myNoSqlClient.Start();
            //
            // await Task.Delay(5000);
            //
            // var (coin, wallet) = mapper.AssetToBitgoCoinAndWallet("jetwallet", "BTC");
            // Console.WriteLine($"BTC (coin|wallet): {coin}|{wallet}");
            //
            // Console.WriteLine("End");
            // Console.ReadLine();
        }

        private static async Task GenerateAsstToBitgoMap()
        {
            var broker = "jetwallet";
            var nosqlWriterUrl = "http://192.168.10.80:5123";

            var clientAsset =
                new MyNoSqlServerDataWriter<BitgoAssetMapEntity>(() => nosqlWriterUrl, BitgoAssetMapEntity.TableName,
                    true);

            var list = new List<BitgoAssetMapEntity>();

            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "6054ba9ca9cc0e0024a867a7d8b401b2", "", "tbtc", 0));
            list.Add(BitgoAssetMapEntity.Create(broker, "XLM", "6054bc003dc1af002b0d54bf5b552f28", "", "txlm", 0));
            list.Add(BitgoAssetMapEntity.Create(broker, "LTC", "6054be73b765620006aa87311f43bd47", "", "tltc", 0));
            list.Add(BitgoAssetMapEntity.Create(broker, "XRP", "60584aaded0090000628ce59c01f3a5e", "", "txrp", 0));
            list.Add(BitgoAssetMapEntity.Create(broker, "BCH", "60584b79fd3e0500669e2cf9654d726b", "", "tbch", 0));
            list.Add(BitgoAssetMapEntity.Create(broker, "ALGO", "60584becbc3e2600240548d78e61c02b", "", "talgo", 0));
            list.Add(BitgoAssetMapEntity.Create(broker, "EOS", "60584dcc6f5d31001d5a59371aeeb60a", "", "teos", 0));

            await clientAsset.CleanAndKeepMaxPartitions(0);
            await clientAsset.BulkInsertOrReplaceAsync(list);


            var clientCoin =
                new MyNoSqlServerDataWriter<BitgoCoinEntity>(() => nosqlWriterUrl, BitgoCoinEntity.TableName, true);

            var listCoin = new List<BitgoCoinEntity>();

            listCoin.Add(BitgoCoinEntity.Create("algo", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("bch", 8, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("btc", 8, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("dash", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("eos", 4, 1, false));
            //listCoin.Add(BitgoCoinEntity.Create("eth", 18));
            //listCoin.Add(BitgoCoinEntity.Create("hbar", 0));
            listCoin.Add(BitgoCoinEntity.Create("ltc", 8, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("trx", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("xlm", 7, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("xrp", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("zec", 8, 1, false));

            listCoin.Add(BitgoCoinEntity.Create("talgo", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("tbch", 8, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("tbtc", 8, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("tdash", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("teos", 4, 1, false));
            //listCoin.Add(BitgoCoinEntity.Create("teth", 18));
            //listCoin.Add(BitgoCoinEntity.Create("thbar", 0));
            listCoin.Add(BitgoCoinEntity.Create("tltc", 8, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("ttrx", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("txlm", 7, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("txrp", 6, 1, false));
            listCoin.Add(BitgoCoinEntity.Create("tzec", 8, 1, false));

            await clientCoin.CleanAndKeepMaxRecords(BitgoCoinEntity.TableName, 0);
            await clientCoin.BulkInsertOrReplaceAsync(listCoin);
        }
    }
}