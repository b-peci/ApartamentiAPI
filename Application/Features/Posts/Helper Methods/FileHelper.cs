using Application.GlobalDtos;

namespace Application.Features.Posts.Helper_Methods;

public static class FileHelper
{
    public static string[] SaveImagesInFileAndGetTheirPaths(FileDto[] files, string parentFolderName)
    {
        string[] filePaths = new string[files.Length];
        int index = 0;
        foreach (var item in files)
        {
            string databaseFileDirectoryPath = GlobalFunctions.GetDatabaseDirectoryPath(parentFolderName);
            using var fs = File.Create(databaseFileDirectoryPath + $"/{Guid.NewGuid()}.{item.Extension}");
            byte[] fileContent = Convert.FromBase64String(item.Base64Content);
            // if file is bigger than 2mb don't save it
            if (fileContent.Length >= 2000000) continue;
            fs.Write(fileContent, 0, fileContent.Length);
            filePaths[index] = fs.Name;
            index++;
        }

        return filePaths;
    }
}


