﻿using Models;
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
        public static string baseDir = @"D:\Test";

        public FileItem ReadFile(string fullFilePath)
        {
            FileItem fileItem = new FileItem
            {
                Path = fullFilePath,
                Name = ParseFileName(fullFilePath),
                Type = ParseType(fullFilePath),
                Content = extractor.Extract(fullFilePath),
                Partition = ParsePartition(fullFilePath)
                //TODO last modified herausfinden & setzen
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

            FileType fileType;
            //TODO das zu einem schönen switch machen, schadewa java 7.3 oder so erlaubt das nicht :(
            if (typeString.Equals(FileType.pdf))
            {
                fileType = FileType.pdf;
            }
            else if (typeString.Equals(FileType.txt))
            {
                fileType = FileType.txt;
            }
            else if (typeString.Equals(FileType.docx))
            {
                fileType = FileType.docx;
            }
            else if (typeString.Equals(FileType.xlsx))
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

