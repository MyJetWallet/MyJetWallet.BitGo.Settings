using System;
using Autofac;
using MyJetWallet.BitGo.Settings.NoSql;
using MyJetWallet.BitGo.Settings.Services;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;
using MyNoSqlServer.DataWriter;

namespace MyJetWallet.BitGo.Settings.Ioc
{
    public static class AutofacHelper
    {
        public static void RegisterBitgoSettingsReader(this ContainerBuilder builder, IMyNoSqlSubscriber myNoSqlClient)
        {
            builder
                .RegisterInstance(new MyNoSqlReadRepository<BitgoAssetMapEntity>(myNoSqlClient, BitgoAssetMapEntity.TableName))
                .As<IMyNoSqlServerDataReader<BitgoAssetMapEntity>>()
                .SingleInstance();

            builder
                .RegisterInstance(new MyNoSqlReadRepository<BitgoCoinEntity>(myNoSqlClient, BitgoCoinEntity.TableName))
                .As<IMyNoSqlServerDataReader<BitgoCoinEntity>>()
                .SingleInstance();

            builder
                .RegisterType<WalletMapper>()
                .As<IWalletMapper>()
                .SingleInstance();

            builder
                .RegisterType<AssetMapper>()
                .As<IAssetMapper>()
                .SingleInstance();
        }

        public static void RegisterBitgoSettingsWriter(this ContainerBuilder builder, Func<string> myNoSqlWriterUrl)
        {
            builder
                .RegisterInstance(new MyNoSqlServerDataWriter<BitgoAssetMapEntity>(myNoSqlWriterUrl, BitgoAssetMapEntity.TableName, true))
                .As<IMyNoSqlServerDataWriter<BitgoAssetMapEntity>>()
                .SingleInstance();

            builder
                .RegisterInstance(new MyNoSqlServerDataWriter<BitgoCoinEntity>(myNoSqlWriterUrl, BitgoCoinEntity.TableName, true))
                .As<IMyNoSqlServerDataWriter<BitgoCoinEntity>>()
                .SingleInstance();

            builder.RegisterType<AssetMapperSettingsService>()
                .As<IAssetMapperSettingsService>()
                .SingleInstance();
        }
    }
}