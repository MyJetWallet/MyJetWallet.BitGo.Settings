using MyJetWallet.Domain;

namespace MyJetWallet.BitGo.Settings.Services
{
    public interface IWalletMapper
    {
        IJetWalletIdentity BitgoLabelToWallet(string label);
        string WalletToBitgoLabel(IJetWalletIdentity wallet);
    }
}