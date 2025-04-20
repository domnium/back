using System.ComponentModel;

namespace Domain.Enums;

public enum ContentType
{
    [Description("image/jpeg")]
    ImageJpeg,

    [Description("image/png")]
    ImagePng,

    [Description("image/gif")]
    ImageGif,

    [Description("image/bmp")]
    ImageBmp,

    [Description("video/mp4")]
    VideoMp4,

    [Description("application/octet-stream")]
    ApplicationOctetStream,

    [Description("image/svg+xml")]
    SvgXml
}