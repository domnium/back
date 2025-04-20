namespace Domain;

public static class Configuration
{
    public const int DefaultStatusCode = 200;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 25;

    public static string ConnectionString { get; set; } = string.Empty;
    public static string BackendUrl { get; set; } = string.Empty;
    public static string FrontendUrl { get; set; } = string.Empty;
    public static string JwtKey { get; set; } = string.Empty;
    public static string VersionApi { get; set; } = string.Empty;
    public static string ApiKey { get; set; } = string.Empty;
    public static string PublicUrlFrontEnd { get; set; } = string.Empty;
    public static string ConnectionStringPostgresql { get; set; } = string.Empty;
    public static string ApiKeyAttribute { get; set; } = string.Empty;
    public static string PicturesStudensPath {get; set;} = "/Images/Students/";
    public static string PicturesTeacherPath {get; set;} = "/Images/Teachers/";
    public static string PicturesCoursesPath {get; set;} = "/Images/Courses/";
    public static string PicturesIAPath {get; set;} = "/Images/IA/";
    public static string PicturesCategoriesPath {get; set;} = "/Images/Categories/";
    public static string VideoCoursesTrailer {get; set;} = "/Videos/Courses/Trailers/";
    public static string VideoLecturesPath {get; set;} = "/Videos/Courses/Lectures/";
    public static string SmtpServer { get; set; } = string.Empty;
    public static int SmtpPort { get; set; } = 587;
    public static string SmtpUser { get; set; } = string.Empty;
    public static string SmtpPass { get; set; } = string.Empty;
    public static long PremiumPrice { get; set; } = 79990;

    public static string RabbitMQUser { get; set; } = string.Empty;
    public static string RabbitMQHost { get; set; } = string.Empty;
    public static string RabbitMQPassword { get; set; } = string.Empty;
    public static string AwsKeyId { get; set; } = string.Empty;
    public static string AwsKeySecret { get; set; } = string.Empty;
    public static string AwsRegion { get; set; } = string.Empty;  // Região da AWS
    public static string BucketArchives { get; set; } = string.Empty;  // bucketname
    public static string BucketVideos { get; set; } = string.Empty;  // bucketname
    public static int DurationUrlTempVideos { get; set; } = 24;
    public static bool IsDevelopment { get; set; } = true;
    public static int DurationUrlTempImage { get; set; } = 24;
    public static string CorsPolicyName { get; set; } = "DommnumCorsPolicy";
}
