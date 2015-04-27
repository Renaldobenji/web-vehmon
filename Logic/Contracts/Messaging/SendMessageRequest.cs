using System;

namespace Logic.Contracts.Messaging
{
    public class SendMessageRequest
    {
        public int ConversationId;

        public DateTime MessageSentDate;

        public String Message;
    }
}
