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

        string sql;
        public DB()
        {
            Connection();
        }

        public bool Connection()
        {
            dbmysql = new DBAdapter(DatabaseType.MySql,
             Instance.NewInstance, "localhost", 3306, "datastore", "root", "", "mysql.log");
            dbmysql.Adapter.LogFile = true; // LogFile = protokoll

            try
            {
                connection = true;

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
            return connection;


        }
        /// <summary>
        /// Dateien im DB Speichern
        /// </summary>
        /// <param name="newItem"></param>

        public void InsertData(FileItem newItem)
        {
            // INSERT INTO `filesql` (`ID`, `Path`, `Content`, `Partitions`, `LastModified`, `DataType`, `DataName`)
            // VALUES(NULL, 'C:\\Users\\winkler\\OneDrive - frrfrfrf44\\Desktop\\Deutschkurs', 'Test 123', 'C', '2021-09-14', 'txt', 'Text');
            sql = string.Format($"INSERT INTO `filesql` (`ID`, `Path`, `Content`, `Partitions`, `LastModified`, `DataType`, `DataName`) VALUES (NULL, '{newItem.Path}', '{newItem.Content}', '{newItem.Partition}', '{newItem.LastModified}', '{newItem.Type}', '{newItem.Name}'); ");
            DataTable t1 = dbmysql.Adapter.GetDataTable(sql);
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

        /// <summary>
        /// Dateien im DB holen
        /// </summary>
        /// <param name="searchText"></param>
        public List<FileItem> GetFileItemsByText(string searchText)
        {
            sql = string.Format($"SELECT * FROM filesql Where Content Like '%{searchText}%' ;");
            DataTable dt = dbmysql.Adapter.GetDataTable(sql);

            List<FileItem> listItems = new List<FileItem>();
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

            return listItems;
        }

        /// <summary>
        /// Dateien im DB löschen
        /// </summary>
        /// <param name="id"></param>  Wann wurden die Datein gelöscht, Vorgangsweise ??
        public void DelItems(FileItem fileItem)
        {
            //DELETE FROM `filesql` WHERE `filesql`.`ID` = 6
            sql = string.Format($"DELETE FROM filesql Where Path = {fileItem.Path} AND DataName = {fileItem.Name};");
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


        /// <summary>
        /// Dateienname,Inhalt ändern
        /// </summary>
        /// <param name="currentItem"></param>

        public void UpdateItems(FileItem currentItem, string oldPath)
        {
            // UPDATE `filesql` SET `Path` = 'C:Users?winklerDownload\\Enum.pdf', `Content` = 'HHHHH', `LastModified` = '2021-09-19', `DataType` = 'pfd', `DataName` = 'Enum' WHERE `filesql`.`ID` = 11;
            sql = string.Format($"UPDATE  `filesql` SET `Path` = '{currentItem.Path}',`Content` = '{currentItem.Content}', `LastModified` = '{currentItem.LastModified}', `DataType` = '{currentItem.Type}', `DataName` = '{currentItem.Name}' WHERE `filesql`.`Path` = {oldPath} ;");
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
