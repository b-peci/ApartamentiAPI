namespace Application.Features;

public class GlobalFunctions
{
    public static string GetDatabaseDirectoryPath(string parentFolderName)
    {
        var currentDirectoryPath = Environment.CurrentDirectory;
        var directoryInfo = Directory.GetParent(currentDirectoryPath);
        string databaseFileDirectoryPath = directoryInfo.FullName + $"/Application/DatabaseFileDirectory/Images/{parentFolderName}";
        return databaseFileDirectoryPath;
    }
}