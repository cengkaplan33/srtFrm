using Newtonsoft.Json;
using Surat.Base;
using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Common.Data;
using Surat.Common.Entity;
using Surat.Common.ViewModel;
using Surat.Document.Business.Application;
using Surat.Document.Common.ViewModel;
using Surat.SerendipApplication.Business;
using Surat.WebServer.Application;
using Surat.WebServiceBase.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Resources;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Surat.TestApplication
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrameworkApplicationManager appManager = new FrameworkApplicationManager();
            UserDetailedView currentUser = null;

            try
            {
                FrameworkApplicationManager framework = new FrameworkApplicationManager();
                currentUser = framework.Security.ValidateUser("edeniz", "2");
                throw new WrongPasswordException(framework.Context, "X", 0, "Test");
            }
            catch (Exception exception)
            {
                appManager.Exception.Publish(appManager.Context, exception, currentUser);
                string s = exception.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WebApplicationManager webApplicationManager = new WebApplicationManager();

            //ADUserDetailView a = webApplicationManager.Framework.ActiveDirectory.GetADUser("ertan.deniz");

            //ADUserView a = new ADUserView();
            //a.DisplayName = "IKTest1 Kullanici";
            //a.UserName = "IKTest1";
            //a.Password = "sd.12345";

            //webApplicationManager.Framework.ActiveDirectory.AddADUser(a);

             //ADUserDetailView a2 = webApplicationManager.Framework.ActiveDirectory.GetADUser("IKTest1");

             webApplicationManager.Framework.ActiveDirectory.SetPassword("IKTest1","ad.12345");
            //webApplicationManager.Framework.ActiveDirectory.EnableADUser("IKTest1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var configuration = Surat.Base.Migrations.Initial();
            //var migrator = new DbMigrator()
            //migrator.Update();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UserDetailedView currentUser = null;               
               
            FrameworkApplicationManager framework = new FrameworkApplicationManager();
            currentUser = framework.Security.ValidateUser("edeniz", "1");
            framework.StartUserSession(currentUser);
            SerendipApplicationManager appmanager = new SerendipApplicationManager(framework);
            appmanager.Login();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string url = "http://172.27.30.37/WebService/Documents/UploadDocument";
            DocumentUploadView document = new DocumentUploadView();

            document.FileName = "Client1";
            document.FileTypeId = 1;
            document.Notes = " Client1 dokümanı";
            document.Id = 1;
            document.RelatedObjectType = "İrsaliye";
            document.RelatedObjectId = 10;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 600000; //10 Dk Debug amaçlı
            request.ContentType = "application/json;charset=utf-8";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DocumentUploadView));
                ser.WriteObject(stream, document);
                String json = Encoding.UTF8.GetString(stream.ToArray());
                streamWriter.Write(json);                
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<DocumentUploadView> documents = new List<DocumentUploadView>();
            string url = "http://172.27.30.37/WebService/Documents/UploadDocuments";
            
            DocumentUploadView document = new DocumentUploadView();

            document.FileName = "Client1";
            document.FileTypeId = 1;
            document.Notes = " Client1 dokümanı";
            document.Id = 1;
            document.RelatedObjectType = "İrsaliye";
            document.RelatedObjectId = 11;
            documents.Add(document);

            DocumentUploadView document2 = new DocumentUploadView();

            document2.FileName = "Client2";
            document2.FileTypeId = 1;
            document2.Notes = " Client2 dokümanı";
            document2.Id = 2;
            document2.RelatedObjectType = "İrsaliye";
            document2.RelatedObjectId = 11;
            documents.Add(document2);
            
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 600000; //10 Dk Debug amaçlı
            request.ContentType = "application/json;charset=utf-8";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IEnumerable<DocumentUploadView>));
                ser.WriteObject(stream, documents);
                String json = Encoding.UTF8.GetString(stream.ToArray());
                streamWriter.Write(json);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var result = Upload();
        }
        static async Task Upload()
        {
            string url = "http://172.27.30.37/WebService/Documents/UploadDocument";
            DocumentUploadView document = new DocumentUploadView();

            document.FileName = "Client12.txt";
            document.FileTypeId = 1;
            document.Notes = " Client1 dokümanı";
            document.Id = 64;
            document.RelatedObjectType = "İrsaliye";
            document.RelatedObjectId = 10;
            document.DocumentData = File.ReadAllBytes(@"C:\Actions.txt");

            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:15192");

                // Set the Accept header for BSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

                // POST using the BSON formatter.
                MediaTypeFormatter bsonFormatter = new BsonMediaTypeFormatter();
                var result = await client.PostAsync(url, document, bsonFormatter);
                result.EnsureSuccessStatusCode();

                
                try
                {
                    MediaTypeFormatter[] formatters = new MediaTypeFormatter[] { new BsonMediaTypeFormatter() };
                    var message = result.Content.ReadAsAsync<DocumentUploadResultView>(formatters).Result;
                    
                }
                catch
                {                    
                    throw;
                }                
             
                //var message = result.Content.ReadAsStringAsync().Result;
                //DocumentUploadResultView x = (DocumentUploadResultView)JsonConvert.DeserializeObject(message, typeof(DocumentUploadResultView));
                //var message = result.Content.ReadAsAsync<DocumentUploadResultView>(); 
                //var message = result.Content.ReadAsAsync<DocumentUploadResultView>(bsonFormatter);
            }
        }

        static async Task UploadMultipleDocuments()
        {
            List<DocumentUploadView> documents = new List<DocumentUploadView>();
            string url = "http://172.27.30.37/WebService/Documents/UploadDocuments";
            DocumentUploadView document = new DocumentUploadView();

            document.FileName = "Client1";
            document.FileTypeId = 1;
            document.Notes = " Client1 dokümanı";
            document.Id = 1;
            document.RelatedObjectType = "İrsaliye";
            document.RelatedObjectId = 10;           
            document.DocumentData = File.ReadAllBytes(@"C:\Actions.txt");
            documents.Add(document);

            DocumentUploadView document2 = new DocumentUploadView();

            document2.FileName = "Client2";
            document2.FileTypeId = 1;
            document2.Notes = " Client2 dokümanı";
            document2.Id = 2;
            document2.RelatedObjectType = "İrsaliye";
            document2.RelatedObjectId = 10;
            document2.DocumentData = File.ReadAllBytes(@"C:\Actions.txt");            
            documents.Add(document2);


            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:15192");

                // Set the Accept header for BSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

                // POST using the BSON formatter.
                MediaTypeFormatter bsonFormatter = new BsonMediaTypeFormatter();
                var result = await client.PostAsync(url, documents, bsonFormatter);
                result.EnsureSuccessStatusCode();

                //var message = result.Content.ReadAsStringAsync().Result; 
                //Dönen cevap üzerinden mesaj/nesneyi almak. 
                //Nesneye Deserialize etmek : Örnek
                //var returnedListOfPolicy = (List<Policy>)JsonConvert.DeserializeObject(returnedObjects, typeof(List<Policy>));
            }
        }

        static async Task Download()
        {
            string url = "http://172.27.30.37/WebService/Documents/GetDocument?Id=64";
            DocumentDownloadView document;

            using (HttpClient client = new HttpClient())
            {               

                // Set the Accept header for BSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

                // Send GET request.
                var result = await client.GetAsync(url);
                result.EnsureSuccessStatusCode();

                // Use BSON formatter to deserialize the result.
                MediaTypeFormatter[] formatters = new MediaTypeFormatter[] {new BsonMediaTypeFormatter()};

                document = await result.Content.ReadAsAsync<DocumentDownloadView>(formatters);

                File.WriteAllBytes(@"C:\Download.txt", document.DocumentData);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var result = UploadMultipleDocuments();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var result = Download();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FrameworkApplicationManager appManager = new FrameworkApplicationManager();
            UserDetailedView currentUser = null;

            FrameworkApplicationManager framework = new FrameworkApplicationManager();
            currentUser = framework.Security.ValidateUser("edeniz", "1");
            framework.StartUserSession(currentUser);

            //int contextId = framework.Context.InitializeDBContext2();
            //framework.Context.CommitDBChanges(contextId);
            //framework.Context.CommitDBChanges(2);
            framework.Dispose();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("tr-TR");
            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            FrameworkApplicationManager framework = new FrameworkApplicationManager();
            framework.Globalization.Context.CurrentCulture = Culture.English;
            UserDetailedView currentUser = null;

            try
            {
                currentUser = framework.Security.ValidateUser("edeniz", "1");
                framework.StartUserSession(currentUser);
                framework.Globalization.GetGlobalizationKeyValue(framework.Context.SystemId,Constants.Message.OperationCompleted);
                framework.Globalization.GetGlobalizationKeyValue(framework.Context.SystemId, Constants.Message.OperationCompleted);
            }
            catch (Exception exception)
            {
                framework.PublishException(exception);               
            }
            

            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string url = "http://172.27.30.37/WebService/Documents/DeleteDocumentById?Id=1";
            
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 600000; //10 Dk Debug amaçlı
            request.ContentType = "application/json;charset=utf-8";
            request.Method = "DELETE";            

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            UserDetailedView currentUser = null;
            FrameworkApplicationManager framework = new FrameworkApplicationManager();
            currentUser = framework.Security.ValidateUser("edeniz", "1");
            framework.StartUserSession(currentUser);

            GlobalizationKey key1 = new GlobalizationKey();
           
            using (System.Data.Entity.DbContextTransaction dbTran = framework.Context.DBContext.Database.BeginTransaction())
            {
                try
                {
                    key1.Name = "Test200";
                    key1.SystemId = 1;
                    framework.Context.DBContext.GlobalizationKeys.Add(key1);

                    framework.Context.DBContext.SaveChanges();

                    key1.Name = "Test201";
                    key1.SystemId = 1;
                    //framework.Context.DBContext.GlobalizationKeys.Add(key1);

                    framework.Context.DBContext.SaveChanges();
                   
                    //saves all above operations within one transaction

                    GlobalizationKey key2 = framework.Context.DBContext.GlobalizationKeys.Where(p => p.Id == 13).FirstOrDefault();
                    key2.Name = "SaveXXX";

                    GlobalizationKeyRepository r = new GlobalizationKeyRepository(framework.Context.Globalization);
                    r.Update(key2);

                    framework.Context.DBContext.SaveChanges();

                    //commit transaction
                    dbTran.Commit();
                }
                catch
                {
                    //Rollback transaction if exception occurs
                    dbTran.Rollback();                    
                }                

            }            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string url = "http://172.27.30.37/WebService/Documents/DeleteDocumentById?Id=1";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 600000; //10 Dk Debug amaçlı
            request.ContentType = "application/json;charset=utf-8";
            //request.ContentLength = 0;
            request.Method = "DELETE";
            request.Headers.Add("SuratAuthenticationKey", "eyJVc2VyTmFtZSI6ImVkZW5peiIsIlBhc3N3b3JkIjoiMSIsIlN0YXJ0VGltZSI6IjIwMTYtMDEtMDZUMTE6NDM6MTYuMjk4NDYxOCswMjowMCJ9");

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var account = new AuthenticationToken() { UserName = "edeniz", Password = "1", StartTime = DateTime.Now };
            string json = JsonConvert.SerializeObject(account);
            string base64EncodedExternalAccount = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            
            byte[] byteArray = Convert.FromBase64String(base64EncodedExternalAccount);
            string jsonBack = Encoding.UTF8.GetString(byteArray);
            var accountBack = JsonConvert.DeserializeObject<AuthenticationToken>(jsonBack);            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string url = "http://172.27.30.37/WebService/Authentication/Login?userName=edeniz&&password=1";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 600000; //10 Dk Debug amaçlı
            request.ContentType = "application/json;charset=utf-8";           

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            UserDetailedView currentUser = null;
            FrameworkApplicationManager framework = new FrameworkApplicationManager();
            try
            {

                currentUser = framework.Security.ValidateUser("Admin2", "1");

                // throw new WrongPasswordException(framework.Context, "X", 0, "Test");
            }
            catch (Exception exception)
            {
                framework.Exception.Publish(framework.Context, exception, currentUser);
                string s = exception.Message;
            }
        }
    }
}
