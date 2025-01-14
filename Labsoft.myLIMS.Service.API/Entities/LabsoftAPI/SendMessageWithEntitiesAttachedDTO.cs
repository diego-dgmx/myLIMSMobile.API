namespace LabsoftAPI
{
    public class SendMessageWithEntitiesAttachedDTO
    {
        public List<int>? WorkIds { get; set; }
        public List<int>? SampleIds { get; set; }
        public List<int>? MethodIds { get; set; }
        public SendMessage? Message { get; set; }
        public bool? SampleSelected { get; set; }
    }

    public class SendMessageRecipient
    {
        public string? Email { get; set; }
        public int? AccountToId { get; set; }
        public int? Type { get; set; }
    }

    public class SendMessage
    {
        public Message? Message { get; set; }
        public List<SendRecipient>? Recipients { get; set; }
        public List<int>? Files { get; set; }
        public string? MessageHtml { get; set; }
        public int? ReplyToMessageId { get; set; }
        public int? MessageAction { get; set; }
    }

    public class SendRecipient
    {
        public string? Email { get; set; }
        public int? AccountToId { get; set; }
        public int? Type { get; set; }
    }

    public class Message {
        public string? Subject { get; set; }
        public string? EmailFrom { get; set; }
        public int? AccountFromId { get; set; }
        public bool? Active { get; set; }
        public bool? Draft { get; set; }
        public bool? Urgent { get; set; }
        public string? MessageID { get; set; }
        public int? MessageTypeId { get; set; }
    }
}