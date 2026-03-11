namespace WMS.Presentation.Utilities
{
    public static class ResultCode
    {
        public const string Success = "SUCCESS";
        public const string Created = "CREATED_SUCCESSFULLY";
        public const string Updated = "UPDATED_SUCCESSFULLY";
        public const string Deleted = "DELETED_SUCCESSFULLY";

        public const string InvalidRequest = "INVALID_REQUEST";
        public const string BadRequest = "BAD_REQUEST";
        public const string ValidationError = "VALIDATION_ERROR";
        public const string NotFound = "NOT_FOUND";

        public const string AlreadyExists = "ALREADY_EXISTS";

        public const string Unauthorized = "UNAUTHORIZED";
        public const string Forbidden = "FORBIDDEN";

        public const string InternalError = "INTERNAL_SERVER_ERROR";
    }
}