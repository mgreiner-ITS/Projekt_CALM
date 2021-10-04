using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Visualis.Extractor;

namespace BusinessLogic.Management
{
    public  class FileItemReader
    {
        public static TextExtractorD extractor = new TextExtractorD();
        public static int n = 0;
        public static Dictionary<DirectoryInfo, long> Dir = new Dictionary<DirectoryInfo, long>();

        public FileItem ReadFile(string fullFilePath, DateTime lastModified)
        {
            FileItem fileItem = new FileItem
            {
                Path = fullFilePath,
                Name = ParseFileName(fullFilePath),
                Type = ParseType(fullFilePath),
                Content = extractor.Extract(fullFilePath),
                Partition = ParsePartition(fullFilePath),
                LastModified = lastModified
            };

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

            //TODO das zu einem schönen switch machen, schadewa java 7.3 oder so erlaubt das nicht :(
            //So vielleicht?
            //Enum.TryParse(typeString, out FileType fileType);
            FileType fileType;

            if (typeString == FileType.pdf.ToString())
            {
                fileType = FileType.pdf;
            }
            else if (typeString == FileType.txt.ToString())
            {
                fileType = FileType.txt;
            }
            else if (typeString == FileType.docx.ToString())
            {
                fileType = FileType.docx;
            }
            else if (typeString == FileType.xlsx.ToString())
            {
                fileType = FileType.xlsx;
            }
            else
            {
                fileType = FileType.other;
            }

            return fileType;
        }

        private string ParseFileName(string fullFilePath)
        {
            char pathSeparator = FindPathSeparator(fullFilePath);

            string[] filePathParts = fullFilePath.Split(pathSeparator);

            return filePathParts[filePathParts.Length - 1];
        }

    }
  
}


