using System;
using System.Collections.Generic;

namespace Utility
{
    [Serializable]
    public class Message
    {
        public Message()
        {
            this._additionalMessages = new Dictionary<string, object>();
        }

        private Dictionary<string, object> _additionalMessages;

        public string Text { get; set; }
        public int Code { get; set; }
        public string Source { get; set; }       
        public Dictionary<string, object> AdditionalMessages
        {
            get { return _additionalMessages; }
        }
    }
}
