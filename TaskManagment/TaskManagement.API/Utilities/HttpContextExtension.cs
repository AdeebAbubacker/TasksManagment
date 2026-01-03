namespace TaskManagement.API.Utilities
{
    public static class HttpContextExtensions
    {
        public static void InsertPaginationInformationHeader(this HttpContext httpContext, int totalAmountOfRecords)
        {
            if (httpContext == null) return;
            const string headerName = "X-Total-Count";
            if (httpContext.Response.Headers.ContainsKey(headerName))
                httpContext.Response.Headers.Remove(headerName);
            httpContext.Response.Headers.Append(headerName, totalAmountOfRecords.ToString());
        }
    }
}
