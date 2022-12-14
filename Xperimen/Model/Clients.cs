

using SQLite;
using System;

namespace Xperimen.Model
{
    public class Clients
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public byte[] ProfileImage { get; set; }
        public string AppTheme { get; set; }
        public DateTime AccountCreated { get; set; }
        public DateTime AccountUpdated { get; set; }
        public DateTime Logout { get; set; }
        public bool IsLogin { get; set; }
        public bool HaveOnetimeLogin { get; set; }
        public bool HaveUpdated { get; set; }
        public double Income { get; set; }
        public double TotalCommitment { get; set; }
        public double NetIncome { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
    }
}
