using SQLite;
using System;

namespace Xperimen.Model
{
    public class MediaItem
    {
        [PrimaryKey]
        public string Id { get; set; }
        public byte[] Picture { get; set; }
        public double Amount { get; set; }
        public string Title { get; set; }
        public bool HasAttachment { get; set; }
        public DateTime ExpensesDt { get; set; }
        public string ExpenseDateTime { get; set; }
    }
}
