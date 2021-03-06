using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Visualis.Extractor;

namespace BusinessLogic.Management.FileManagement
{
    public  class FileReader
    {
        public static TextExtractorD extractor = new TextExtractorD();
        public static int n = 0;
        public static Dictionary<DirectoryInfo, long> Dir = new Dictionary<DirectoryInfo, long>();

        /// <summary>
        /// Reads a file from given fullFilePath and sets the given lastModified date
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <param name="lastModified"></param>
        /// <returns>FileItem with all properties OR null when unreadable filetype was detected</returns>
        public FileItem ReadFile(string fullFilePath, DateTime lastModified)
        {
            FileItem fileItem = new FileItem
            {
                Path = fullFilePath,
                Name = ParseFileName(fullFilePath),
                Type = ParseType(fullFilePath),
                Partition = ParsePartition(fullFilePath),
                LastModified = lastModified
            };

            if (fileItem.Type == FileType.pdf)
                fileItem.Content = extractor.ExtractPdf(fullFilePath);
            else if (fileItem.Type == FileType.other)
                return null;
            else
                fileItem.Content = extractor.Extract(fullFilePath);

            return fileItem;
        }

        private string ParsePartition(string fullFilePath)
        {
            return fullFilePath.Substring(0, 1);
        }

        private char FindPathSeparator(string fullFilePath)
        {
            if (fullFilePath.Contains('\\'))
            {
                return '\\';
            }
            else if (fullFilePath.Contains('/'))
            {
                return '/';
            }
            else
            {
                throw new Exception("File does not contain a path delimiter. This should never happen :)");
            }
        }

        private FileType ParseType(string fullFilePath)
        {
            char pathSeparator = FindPathSeparator(fullFilePath);

            string[] filePathParts = fullFilePath.Split(pathSeparator);
            string[] fileNameParts = filePathParts[filePathParts.Length - 1].Split('.');
            string typeString = fileNameParts[fileNameParts.Length - 1];


            if (Enum.TryParse(typeString, out FileType fileType))
            {
                return fileType;
            }
            else
            {
                return FileType.other;
            }
        }

        private string ParseFileName(string fullFilePath)
        {
            char pathSeparator = FindPathSeparator(fullFilePath);

            string[] filePathParts = fullFilePath.Split(pathSeparator);

            return filePathParts[filePathParts.Length - 1];
        }

    }
  
}


