namespace Talabat.Domain.Shared.Constants
{
    public static class FileSettings
    {
        public const string ProductPicturesFolderPath = "images/products";
        public const string UserPicturesFolderPath = "images/users";
        public const long MaxImageSize = 2097152; // 1048576
        public static readonly List<string> AllowedExtensions = [".jpg", ".jpeg", ".png"];
    }
}
