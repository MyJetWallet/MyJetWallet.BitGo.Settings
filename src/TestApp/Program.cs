using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Press enter to start");
            Console.ReadLine();

            await GenerateAsstToBitgoMap();

            Console.WriteLine("End");
            Console.ReadLine();
        }

        private static async Task GenerateAsstToBitgoMap()
        {
            var broker = "jetwallet";
            var nosqlWriterUrl = "http://192.168.10.80:5123";

            var clientAsset = new MyNoSqlServer.DataWriter.MyNoSqlServerDataWriter<BitgoAssetMapEntity>(() => nosqlWriterUrl, BitgoAssetMapEntity.TableName, true);

            var list = new List<BitgoAssetMapEntity>();

            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "6054ba9ca9cc0e0024a867a7d8b401b2", "tbtc"));
            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "6054bc003dc1af002b0d54bf5b552f28", "txlm"));
            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "6054be73b765620006aa87311f43bd47", "tltc"));
            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "60584aaded0090000628ce59c01f3a5e", "txrp"));
            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "60584b79fd3e0500669e2cf9654d726b", "tbch"));
            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "60584becbc3e2600240548d78e61c02b", "talgo"));
            list.Add(BitgoAssetMapEntity.Create(broker, "BTC", "60584dcc6f5d31001d5a59371aeeb60a", "teos"));

            await clientAsset.CleanAndKeepMaxRecords(BitgoAssetMapEntity.TableName, 0);
            await clientAsset.BulkInsertOrReplaceAsync(list);



            var clientCoin = new MyNoSqlServer.DataWriter.MyNoSqlServerDataWriter<BitgoCoinEntity>(() => nosqlWriterUrl, BitgoCoinEntity.TableName, true);

            var listCoin = new List<BitgoCoinEntity>();

            listCoin.Add(BitgoCoinEntity.Create("algo", 6));
            listCoin.Add(BitgoCoinEntity.Create("bch", 8));
            listCoin.Add(BitgoCoinEntity.Create("btc", 8));
            listCoin.Add(BitgoCoinEntity.Create("dash", 6));
            listCoin.Add(BitgoCoinEntity.Create("eos", 4));
            //listCoin.Add(BitgoCoinEntity.Create("eth", 18));
            //listCoin.Add(BitgoCoinEntity.Create("hbar", 0));
            listCoin.Add(BitgoCoinEntity.Create("ltc", 8));
            listCoin.Add(BitgoCoinEntity.Create("trx", 6));
            listCoin.Add(BitgoCoinEntity.Create("xlm", 7));
            listCoin.Add(BitgoCoinEntity.Create("xrp", 6));
            listCoin.Add(BitgoCoinEntity.Create("zec", 8));

            listCoin.Add(BitgoCoinEntity.Create("talgo", 6));
            listCoin.Add(BitgoCoinEntity.Create("tbch", 8));
            listCoin.Add(BitgoCoinEntity.Create("tbtc", 8));
            listCoin.Add(BitgoCoinEntity.Create("tdash", 6));
            listCoin.Add(BitgoCoinEntity.Create("teos", 4));
            //listCoin.Add(BitgoCoinEntity.Create("teth", 18));
            //listCoin.Add(BitgoCoinEntity.Create("thbar", 0));
            listCoin.Add(BitgoCoinEntity.Create("tltc", 8));
            listCoin.Add(BitgoCoinEntity.Create("ttrx", 6));
            listCoin.Add(BitgoCoinEntity.Create("txlm", 7));
            listCoin.Add(BitgoCoinEntity.Create("txrp", 6));
            listCoin.Add(BitgoCoinEntity.Create("tzec", 8));

            await clientCoin.CleanAndKeepMaxRecords(BitgoCoinEntity.TableName, 0);
            await clientCoin.BulkInsertOrReplaceAsync(listCoin);
        }
    }
}
