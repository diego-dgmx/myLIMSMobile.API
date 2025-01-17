namespace LabsoftAPI {
    public class Recipient
    {
        public int? Id { get; set; }
        public string? RecipientUID { get; set; }
        public string? Identification { get; set; }
        public int? Type { get; set; }
        public object? Email { get; set; }
        public int? AccountToId { get; set; }
        public string? AccountToIdentification { get; set; }
        public object? Read { get; set; }
    }

    public class MessageBasic
    {
        public int? Id { get; set; }
        public DateTime? Sent { get; set; }
        public object? SentExternal { get; set; }
        public int? EditionUserId { get; set; }
        public DateTime? EditionDateTime { get; set; }
        public bool? Active { get; set; }
        public string? Subject { get; set; }
        public string? MessageID { get; set; }
        public object? Folder { get; set; }
        public string? EmailFrom { get; set; }
        public object? AccountFromId { get; set; }
        public object? AccountFromIdentification { get; set; }
        public int? MessageHtmlFileId { get; set; }
        public string? FromUID { get; set; }
        public string? FromIdentification { get; set; }
        public int? MessageTypeId { get; set; }
        public string? MessageTypeIdentification { get; set; }
        public object? MessageHtml { get; set; }
        public string? MessageTextPlan { get; set; }
        public bool? Draft { get; set; }
        public bool? Urgent { get; set; }
        public bool? Read { get; set; }
        public DateTime? OrderDateTime { get; set; }
        public List<Recipient>? Recipients { get; set; }
        public List<MessageFile>? Files { get; set; }
        public object? Works { get; set; }
        public object? Samples { get; set; }
        public object? SampleMethods { get; set; }
    }

    public class MessageFile {
        public int? Id { get; set; }
        public int? FileId { get; set; }
        public string? FileIdentification { get; set; }
    }
}