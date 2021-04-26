namespace OnlineElection.Services
{
    public interface ISendEmailAsync:ISendAsync
    {
   public   string Subject { get;  set; }
        public string EmailTo { get; set; }
        public string Message { get; set; }
        void SetData(string emailTo, string subject, string message);
       

    }
}
