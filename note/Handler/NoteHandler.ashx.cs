using MyClassLibrary.JSON;
using note.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace note.Handler
{
    public class NoteHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string option = context.Request.QueryString["option"].ToString();
                JsonData data = new JsonData();
                switch (option)
                {
                    case "InitMenu":
                        {
                            data = InitMenu();
                            break;
                        }
                    case "AddNode":
                        {
                            string parentcode = context.Request.QueryString["parentcode"].ToString();
                            string type = context.Request.QueryString["type"].ToString();
                            data = AddNode(parentcode, type);
                            break;
                        }
                }
                context.Response.Write(JsonMapper.ToJson(data));
            }
            catch
            {

            }
        }

        private JsonData InitMenu()
        {
            JsonData data = new JsonData();
            Menu instance = new Menu();
            data = instance.GetMenu();
            return data;
        }

        private JsonData AddNode(string parentcode, string type)
        {
            JsonData data = new JsonData();
            Menu instance = new Menu();
            data = instance.AddNode(parentcode, type);
            return data;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}