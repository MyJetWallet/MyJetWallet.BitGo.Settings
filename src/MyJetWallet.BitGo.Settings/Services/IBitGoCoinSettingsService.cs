using System.Threading.Tasks;
using MyJetWallet.BitGo.Settings.NoSql;

namespace MyJetWallet.BitGo.Settings.Services
{
    public interface IBitGoCoinSettingsService
    {
        ValueTask<bool> CreateBitgoCoinEntityAsync(string coin, int accuracy, int requiredConfirmations);

        ValueTask<bool> UpdateBitgoCoinEntityAsync(string coin, int accuracy, int requiredConfirmations);

        ValueTask<bool> DeleteBitgoCoinEntityAsync(string coin);

        ValueTask<BitgoCoinEntity[]> GetAllCoinsAsync();
    }
}