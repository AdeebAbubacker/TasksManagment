namespace TaskManagement.API.Utilities
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationInformationHeader(this HttpContext httpContext, int totalAmountOfRecords)
        {
            if (httpContext == null) return;

            // Always use a valid header name — no spaces.
            const string headerName = "X-Total-Count";

            // Remove existing header to avoid KeyAlreadyExists exception
            if (httpContext.Response.Headers.ContainsKey(headerName))
                httpContext.Response.Headers.Remove(headerName);

            // Append using the safe API
            httpContext.Response.Headers.Append(headerName, totalAmountOfRecords.ToString());
        }
    }
}
