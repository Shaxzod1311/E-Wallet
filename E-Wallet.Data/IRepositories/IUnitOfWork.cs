namespace E_Wallet.Data.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IWalletRepository Wallets { get; }
        public IUserRepository Users { get; }
        public ITransactionRepository Transaction { get; }
        Task SaveChangesAsync();
    }
}
