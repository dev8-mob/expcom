

using SQLite;

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
        public bool IsLogin { get; set; }
        public double Income { get; set; }
        public double TotalCommitment { get; set; }
        public double NetIncome { get; set; }
    }
}
