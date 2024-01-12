namespace AuthoMaui.Domain.Models.Results
{
    public class CommandResult<T>
    {
        //private constructor ensures control over the creation of instances
        private CommandResult(bool isSuccesfull, string message, T data )
        { 
            IsSuccesfull = isSuccesfull;
            Message = message;
            Data = data;
        }
        
        public bool IsSuccesfull { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // Factory method for success
        public CommandResult<T> Success(string message, T data ) 
        {
            return new CommandResult<T>(true, message, data);
        }

        //Factory method for success
        public CommandResult<T>  Failure (string message, T data) 
        {
            return new CommandResult<T>(false, message, data);
        }
    }
}
