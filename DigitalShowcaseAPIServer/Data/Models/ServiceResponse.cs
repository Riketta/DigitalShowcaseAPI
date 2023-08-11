namespace DigitalShowcaseAPIServer.Data.Models
{
    public class ServiceResponse<T> where T : class
    {
        public T? Data { get; set; } = null;
        public bool Success { get; set; } = true;
        
        /// <summary>
        /// If <see cref="Success"/> set to false, consider <see cref="Message"/> as error message
        /// </summary>
        public string Message = string.Empty;
    }
}
