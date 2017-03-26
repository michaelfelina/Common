using System.Collections.Generic;

namespace Common.Methods
{
    public abstract class AOperationResult
    {
        public bool Success { get; set; }
        public List<string> MessageList { get; private set; }
        public string ErrorMessage => SetValue();
        
        public AOperationResult()
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
    public class OperationResult<T>: AOperationResult where T : class
    {
        public List<T> results { get; set; }
        public T result { get; set; }
    }

    public class OperationResult : AOperationResult
    {
        
    }
}
