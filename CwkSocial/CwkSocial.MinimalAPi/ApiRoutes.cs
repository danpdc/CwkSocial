namespace CwkSocial.MinimalAPi;

public static class ApiRoutes
{
    public const string BaseRoute = "api/v1";

    public static class UserProfiles
    {
        public const string UserProfileBase = "/api/v1/userprofiles";
        public const string IdRoute = "/{id}";
    }

    public static class Posts
    {
        public const string PostBase = "/api/b1/posts";
        public const string IdRoute = "/{id}";
        public const string PostComments = "/{postId}/comments";
        public const string CommentById = "/{postId}/comments/{commentId}";
        public const string InteractionById = "/{postId}/interactions/{interactionId}";
        public const string PostInteractions = "/{postId}/interactions";
    }

    public static class Identity
    {
        public const string IdentityBase = "/api/v1/identity";
        public const string Login = "/login";
        public const string Registration = "/registration";
        public const string IdentityById = "/{identityUserId}";
        public const string CurrentUser = "/currentuser";
    }
}