using MyJetWallet.Domain;
// ReSharper disable UnusedMember.Global

namespace MyJetWallet.BitGo.Settings.Services
{
    public class WalletMapper : IWalletMapper
    {
        public IJetWalletIdentity BitgoLabelToWallet(string label)
        {
            if (string.IsNullOrEmpty(label))
                return null;

            var prm = label.Split("|-|");

            if (prm.Length != 3)
                return null;

            if (string.IsNullOrEmpty(prm[0]) || string.IsNullOrEmpty(prm[1]) || string.IsNullOrEmpty(prm[2]))
                return null;

            return new JetWalletIdentity(prm[0], string.Empty, prm[1], prm[2]);
        }

        public string WalletToBitgoLabel(IJetWalletIdentity wallet)
        {
            return $"{wallet.BrokerId}|-|{wallet.ClientId}|-|{wallet.WalletId}";
        }
    }
}