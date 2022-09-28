using SQLite;

namespace Xperimen.Model
{
    public class SelfCommitment
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public bool IsDone { get; set; }
        public bool HasAccNo { get; set; }
        public bool HasAttachment { get; set; }
        public int AccountNo { get; set; }
        public byte[] Picture { get; set; }
    }
}
