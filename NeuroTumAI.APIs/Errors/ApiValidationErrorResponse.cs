namespace NeuroTumAI.APIs.Errors
{
	public class ApiValidationErrorResponse : ApiResponse
	{
		public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse(string? Message = null) : base(400, Message)
        {

        }
        public ApiValidationErrorResponse()
			: base(400)
		{
			Errors = new List<string>();
		}
	}
}
