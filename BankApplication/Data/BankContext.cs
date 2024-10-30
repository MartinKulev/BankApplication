using BankApplication_Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BankApplication_Web.Data
{
    public class BankContext : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<UserBankInfo> UserBankInfos { get; set; }

        public DbSet<UserIBANInfo> UserIBANInfos { get; set; }
        public DbSet<CreditBooleanInfo> CreditBooleanInfos { get; set; }
        public DbSet<CreditDateInfo> CreditDateInfos { get; set; }
        public DbSet<CreditMoneyInfo> CreditMoneyInfos { get; set; }

        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
        }
    }
}
