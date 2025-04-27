using System;
using System.ComponentModel;
using System.Reflection;
using Domain.Enums;

namespace Domain.ExtensionsMethods;

public static class ContentTypeExtensions
{
    public static string GetMimeType(this ContentType contentType)
    {
        if (contentType == null)
        {
            return "application/octet-stream"; 
        }

        return contentType
            .GetType()
            .GetMember(contentType.ToString())[0]
            .GetCustomAttribute<DescriptionAttribute>()?
            .Description ?? contentType.ToString();
    }

    public static string GetMimeType(this ContentType? contentType)
    {
        if (contentType == null)
        {
            return "application/octet-stream"; 
        }

        return contentType
            .GetType()
            .GetMember(contentType.ToString())[0]
            .GetCustomAttribute<DescriptionAttribute>()?
            .Description ?? contentType.ToString();
    }

    public static ContentType? ParseMimeType(string mimeType)
    {
        foreach (ContentType value in Enum.GetValues(typeof(ContentType)))
        {
            var description = value.GetMimeType();
            if (string.Equals(description, mimeType, StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }
        }
        Console.WriteLine("ContentType não Configurado.");
        return null; 
    }
}