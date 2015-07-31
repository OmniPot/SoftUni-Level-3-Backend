namespace ATM.Data
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class ATMEntities : DbContext
    {
        public ATMEntities()
            : base("ATMEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ATMEntities, Configuration>());
        }

        public virtual IDbSet<CardAccount> CardAccounts { get; set; }

        public virtual IDbSet<TransactionHistoryEntry> TransactionHistoryEntries { get; set; }
    }
}