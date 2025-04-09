namespace Domain;

public static class Configuration
{
    public const int DefaultStatusCode = 200;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 25;

    public static string ConnectionString { get; set; } = string.Empty;
    public static string BackendUrl { get; set; } = string.Empty;
    public static string FrontendUrl { get; set; } = string.Empty;
    public static string PicturesStudensPath {get; set;} = "/Images/Students/";
    public static string SmtpServer { get; set; } = string.Empty;
    public static int SmtpPort { get; set; } = 587;
    public static string SmtpUser { get; set; } = string.Empty;
    public static string SmtpPass { get; set; } = string.Empty;
    public static long PremiumPrice { get; set; } = 79990;
    public static string AwsKeyId { get; set; } = string.Empty;
    public static string AwsKeySecret { get; set; } = string.Empty;
    public static string AwsRegion { get; set; } = string.Empty;  // Região da AWS
    public static string BucketArchives { get; set; } = string.Empty;  // bucketname
    public static string BucketVideos { get; set; } = string.Empty;  // bucketname
    public static int DurationUrlTempVideos { get; set; } = 24;
    public static bool IsDevelopment { get; set; } = true;
    public static int DurationUrlTempImage { get; set; } = 24;
}
