using DataAccess.Events;
using Mk.DBConnector;
using Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class DB : IODB
    {
        public event InfosMessageEventHandler infoMessage;
        public event ErrorMessageEH ErrorMessage;
        bool connection = false;
        DBAdapter dbmysql;
        ConnectionState connectionState;

        string sql;
        public DB()
        {
            Connection();

        }

        // Verbindung zur DB SQL
        public bool Connection()
        {
            dbmysql = new DBAdapter(DatabaseType.MySql,
             Instance.NewInstance, "localhost", 3306, "datastore", "root", "", "mysql.log");
            dbmysql.Adapter.LogFile = true; // LogFile = protokoll
            connectionState = dbmysql.Adapter.CheckConnectionState();
            if (connectionState == ConnectionState.Open)
            {
                connection = true;
            }
            else infoMessage?.Invoke($"Keine Verbindung zur DB");
            return connection;
        }

        // Alle Dateien von DB holen für Monitoring, um zu wissen, ob die Dateien gelöscht oder geändert werden.
        public List<FileItem> GetAllFileItems()
        {
            List<FileItem> listItems = new List<FileItem>();

            sql = string.Format($"SELECT * FROM filesql;");
            DataTable dt = dbmysql.Adapter.GetDataTable(sql);
            foreach (DataRow r in dt.Rows)
            {
                FileItem item = new FileItem();
                item.Id = Convert.ToInt64(r[0]);
                item.Path = r[1].ToString();
                item.Content = r[2].ToString();
                item.Partition = r[3].ToString();
                item.LastModified = Convert.ToDateTime(r[4]);
                item.Type = (FileType)Enum.Parse(typeof(FileType), r[5].ToString());
                item.Name = r[6].ToString();
                listItems.Add(item);
            }

            return listItems;
        }
        // Es wird verwendet, um eine Datei zu überprüfen, bevor sie hochgeladen wird.
        public DateTime GetFile(string path)
        {
           
            //SELECT * FROM `filesql` WHERE Path = 'D:\\brot.docx';
            sql = string.Format($"SELECT * FROM filesql WHERE Path = '{ParsePath(path)}';");
            DataTable dt = dbmysql.Adapter.GetDataTable(sql);
            foreach (DataRow r in dt.Rows)
            {
                return Convert.ToDateTime(r[4]);
            }
            throw new DataException("No file was found.");
        }

        /// Dateien im DB Speichern
        public void InsertData(FileItem newItem)
        {
            // INSERT INTO `filesql` (`ID`, `Path`, `Content`, `Partitions`, `LastModified`, `DataType`, `DataName`)
            // VALUES(NULL, 'C:\\Users\\winkler\\OneDrive - frrfrfrf44\\Desktop\\Deutschkurs', 'Test 123', 'C', '2021-09-14', 'txt', 'Text');

            //string dateTimeString = ParseDateTime(newItem.LastModified);
            //string parsedPath = ParsePath(newItem.Path);
            sql = string.Format($"INSERT INTO `filesql` (`ID`, `Path`, `Content`, `Partitions`, `LastModified`, `DataType`, `DataName`) VALUES (NULL, '{ ParsePath(newItem.Path)}', '{newItem.Content}', '{newItem.Partition}', '{ ParseDateTime(newItem.LastModified)}', '{newItem.Type}', '{newItem.Name}'); ");

            try
            {
                dbmysql.Adapter.ExecuteSQL(sql);
                infoMessage?.Invoke($"Successfully Insert");
            }
            catch (Exception ex)
            {
                if (ErrorMessage != null)
                {
                    ErrorMessage(ex.Message);
                }
            }
        }

        //Ersetzt Einzelne \ mit \\, damit sie in DB SQL den \ annehmen
        private string ParsePath(string path)
        {
            return path.Replace("\\", "\\\\");
        }

        // Die Datum und Zeit wird in das SQL Format geändert.
        private string ParseDateTime(DateTime lastModified)
        {
            string dateTimeString = lastModified.Year + "-" + lastModified.Month + "-" + lastModified.Day + " " + lastModified.Hour + ":" + lastModified.Minute + ":" + lastModified.Second + ".000000";

            return dateTimeString;
        }


        /// Dateien im DB holen -> Fulltext Search
        public List<FileItem> GetFileItemsByText(string searchText)
        {
            List<FileItem> listItems = new List<FileItem>();
            if (searchText != null)
            {
                // SELECT* FROM filesql WHERE MATCH(Content) AGAINST('Datenbank');
                sql = string.Format($"SELECT * from filesql where MATCH (Content) AGAINST('{searchText}');");
                DataTable dt = dbmysql.Adapter.GetDataTable(sql);
                foreach (DataRow r in dt.Rows)
                {
                    FileItem item = new FileItem();
                    item.Id = Convert.ToInt64(r[0]);
                    item.Path = r[1].ToString();
                    item.Content = r[2].ToString();
                    item.Partition = r[3].ToString();
                    item.LastModified = Convert.ToDateTime(r[4]);
                    item.Type = (FileType)Enum.Parse(typeof(FileType), r[5].ToString());
                    item.Name = r[6].ToString();

                    listItems.Add(item);
                }
                infoMessage?.Invoke($" erfolgreich Insert");
            }
            return listItems;
        }


        /// Dateien im DB löschen     
        public void DelItems(FileItem fileItem)
        {
            //DELETE FROM `filesql` WHERE `filesql`.`ID` = 6
            sql = string.Format($"DELETE FROM filesql Where Path = '{ParsePath(fileItem.Path)}' AND DataName = '{fileItem.Name}';");
            dbmysql.Adapter.ExecuteSQL(sql);
            try
            {
                infoMessage?.Invoke($"{fileItem.Name} erfolgreich gelöscht");
            }
            catch (Exception ex)
            {
                if (ErrorMessage != null)
                {
                    ErrorMessage($"{fileItem.Name}" + ex.Message);
                }
            }
        }


    
        /// Dateienname,Inhalt ändern
        public void UpdateItems(FileItem currentItem, string oldPath)
        {
            // UPDATE `filesql` SET `Path` = 'C:Users?winklerDownload\\Enum.pdf', `Content` = 'HHHHH', `LastModified` = '2021-09-19', `DataType` = 'pfd', `DataName` = 'Enum' WHERE `filesql`.`ID` = 11;
            sql = string.Format($"UPDATE  `filesql` SET `Path` = '{ParsePath(currentItem.Path)}',`Content` = '{currentItem.Content}', `LastModified` = '{ParseDateTime(currentItem.LastModified)}', `DataType` = '{currentItem.Type}', `DataName` = '{currentItem.Name}' WHERE `filesql`.`Path` = '{ParsePath(oldPath)}' ;");
            dbmysql.Adapter.ExecuteSQL(sql);

            try
            {
                infoMessage?.Invoke($" erfolgreich Update: NeuName  + {currentItem.Name}");
            }
            catch (Exception ex)
            {
                if (ErrorMessage != null)
                {
                    ErrorMessage(ex.Message);
                }
            }
        }
    }
}
