using MyClassLibrary.JSON;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace note.Table
{
    public class Menu
    {
        private string table;

        public string TableName
        {
            get { return "t_menu"; }
        }

        public JsonData GetMenu()
        {
            JsonData data = GetMenuTree("0");
            return data;
        }


        private JsonData GetMenuTree(string ParentCode)
        {
            JsonData data = new JsonData();
            data.SetJsonType(JsonType.Array);
            JsonData par = ExecuteQuery(String.Format("select * from {0} where MenuParentCode='{1}' order by MenuCode asc", TableName, ParentCode));
            foreach (JsonData temp in par)
            {
                JsonData children = new JsonData();
                children.SetJsonType(JsonType.Array);
                children = GetMenuTree(temp["MenuCode"].ToString());
                temp["children"] = children;
                data.Add(temp);
            }
            return data;
        }

        private JsonData ExecuteQuery(string queryText)
        {
            MySqlConnection connection = new MySqlConnection(Global.MYSqlConn);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(queryText, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataSet set = new DataSet();
            adapter.Fill(set, TableName);
            connection.Close();
            DataTable table = set.Tables[TableName];
            JsonData res_collection = new JsonData();
            res_collection.SetJsonType(JsonType.Array);
            foreach (DataRow row in table.Rows)
            {
                JsonData ob = new JsonData();
                foreach (DataColumn column in table.Columns)
                {
                    ob[column.ColumnName] = row[column].ToString();
                }
                res_collection.Add(ob);
            }
            return res_collection;
        }

        public JsonData AddNode(string parentcode, string type)
        {
            //JsonData data = new JsonData();
            //MySqlConnection connection = new MySqlConnection(Global.MYSqlConn);
            //connection.Open();
            //MySqlCommand cmd = new MySqlCommand("select count(*) as count from " + TableName, connection);
            //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            //DataSet set = new DataSet();
            //adapter.Fill(set, TableName);
            //DataRow row = set.Tables[TableName].Rows[0];
            //int count = int.Parse(row["count"].ToString());
            //if (type == "Parent")
            //{
            //    count = (count + 11);
            //}
            //else
            //{
            //    count=()
            //}
            return new JsonData();
        }
    }
}