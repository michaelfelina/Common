using System.Collections.Generic;

namespace Common.Methods
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public List<string> MessageList { get; private set; }
        public string ErrorMessage => SetValue();

        public OperationResult()
        {
            Success = true;
            MessageList = new List<string>();
        }
        public void Add(List<string> errorMessage)
        {
            MessageList.AddRange(errorMessage);
            //SetValue();
        }

        public void Add(string processError)
        {
            MessageList.Add(processError);
            //SetValue();
        }

        private string SetValue()
        {
            string result = "";
            if (MessageList != null)
            {
                foreach (var message in MessageList)
                {
                    if (!string.IsNullOrWhiteSpace(result)) result = result + "\n";
                    result = result + message;
                }
            }
            return result;
        }
    }
}
