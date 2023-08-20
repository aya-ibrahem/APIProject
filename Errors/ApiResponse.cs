namespace APIProject.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You made a bad Request!",
                401 => "You Are not Authorized",
                404 => "Resource Not found",
                500 => "Server is crached!",
                _ => String.Empty
            };
        }
       
    }
}
