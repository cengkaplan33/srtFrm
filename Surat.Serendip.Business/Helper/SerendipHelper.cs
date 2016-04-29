using Serendip.Common;
using Serendip.Entity.Sistem;
using Surat.Base.Application;
using Surat.Base.Cache;
using Surat.Base.Exceptions;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Business.Log;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.SerendipApplication.Base;
using Surat.SerendipApplication.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Surat.SerendipApplication.Business
{
    public static class SerendipHelper
    {
        //public static Int64 GetNewObjId(string entityName, IDbConnection connection)
        //{
        //    Int64 longObjID = 0;
        //    int etkilenenKayitSayisi = new SqlUpdate(SerendipEntityObjIdRow.TableName)
        //       .RowLock(true)
        //       .Inc(SerendipEntityObjIdRow.Fields.LastObjId, 1)
        //       .Where("EntityName = '" +entityName + "'") 
        //       .Execute(connection);


        //    if (etkilenenKayitSayisi == 0) // yeni kayıt demek oluyor
        //    {
        //        Int64 newId = 0;

        //        using (IDataReader reader = DataHelper.ExecuteReader(connection,
        //            new SqlSelect(
        //                SqlSelect.Max(SerendipEntityObjIdRow.Fields.ObjId.Name))
        //                         .From(SerendipEntityObjIdRow.TableName)))
        //        {
        //            if (reader.Read() && !reader.IsDBNull(0))
        //                newId = Convert.ToInt64(reader.GetValue(0)) + 1;
        //        }

        //        Int64 lastObjID = Int64.Parse(System.Configuration.ConfigurationManager.AppSettings["SerendipEntityObjIdDefaultValue"]);
        //        lastObjID = lastObjID + 1;

        //        // newObjID buradaki ObjID alanıda identity değil bir artırmak için kullanılıyor
        //        SqlInsert SerendipEntityObjId = new SqlInsert(SerendipEntityObjIdRow.TableName);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.ObjId, newId);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.EntityName, entityName);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.LastObjId, lastObjID);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.SirketiObjId, InkaSettings.Current.ActiveFirmId);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.Aktif, true);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.SistemInsertLogin, UserService.LoggedUser().Username);
        //        SerendipEntityObjId.Set(SerendipEntityObjIdRow.Fields.SistemInsertTime, DateTime.Now);
        //        SerendipEntityObjId.Execute(connection);

        //        longObjID = lastObjID;
        //    }
        //    else
        //    {
        //        using (IDataReader reader = DataHelper.ExecuteReader(connection,
        //            new SqlSelect(SerendipEntityObjIdRow.Fields.LastObjId)
        //                         .From(SerendipEntityObjIdRow.TableName).WhereEqual(SerendipEntityObjIdRow.Fields.EntityName, entityName)))
        //        {
        //            if (reader.Read() && !reader.IsDBNull(0))
        //                longObjID = Convert.ToInt64(reader.GetValue(0));
        //        }
        //    }


        //    return longObjID;
        //}

        private static string GetDecryptedMasterDbConnectionString()
        {
            string base64EncryptedString = System.Configuration.ConfigurationManager.AppSettings["SerendipMasterDbEncryptedConnectionString"];
            if (string.IsNullOrEmpty(base64EncryptedString))
            {
                string message = "SerendipMasterDbEncryptedConnectionString config değeri okunamadı!";
                throw new Exception(message);
            }
            byte[] data = Convert.FromBase64String(base64EncryptedString);
            string encryptedText = UTF8Encoding.UTF8.GetString(data);
            string connectionString = Decrypt(encryptedText);

            return connectionString;
        }

        public static DataTable GetConnectionString(string firmaKodu)
        {
            List<string> connectionStringList = new List<string>();
            DataTable result = new DataTable();
            StringBuilder queryString = new StringBuilder();
            queryString.AppendLine(@"SELECT f.SirketObjId, fb.VeritabaniAdi, fb.KullaniciAdi, fb.KullaniciSifresi, fb.WindowsLogin, s.SunucuAdi 
	                                FROM FirmaDonemi fd 
	                                    LEFT JOIN Firma f ON f.Id = fd.Firma_Id 
                                        LEFT JOIN FirmaBaglantisi fb ON fb.Id = fd.FirmaBaglantisi_Id
                                        LEFT JOIN Sunucu s ON s.Id = fb.Sunucu_Id
                                    WHERE fd.VarsayilanDonem = 1 ");
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(firmaKodu))
            {
                parameters.Add("@prmFirmaKodu", firmaKodu);
                queryString.AppendLine("AND  f.Kodu = @prmFirmaKodu");
            }
            //string masterDbConnectionString = GetDecryptedMasterDbConnectionString();


            //IDbConnection connection = DataHelper.NewConnection(masterDbConnectionString);
            //using (connection)
            //{
            //    result = DataHelper.ExecuteDataTable(connection, queryString.ToString(), parameters);
            //    result.Columns.Add("ConnectionString");
            //    result.AcceptChanges();
            //}

            //var dt= new DataTable();
            using (var da = new SqlDataAdapter(queryString.ToString(), GetDecryptedMasterDbConnectionString()))
            {
                da.SelectCommand.Parameters.Add(parameters);
                da.Fill(result);
                result.Columns.Add("ConnectionString");
                result.AcceptChanges();
            }

            if (result.Rows.Count > 0)
            {
                string connectionString = string.Empty;
                foreach (DataRow row in result.Rows)
                {
                    if ((bool)row["WindowsLogin"])
                    {
                        row["ConnectionString"] = PrepareConnectionString(row, true);
                    }
                    else
                    {
                        row["ConnectionString"] = PrepareConnectionString(row, false);
                    }
                }
            }

            return result;
        }

        public static DataTable GetMasterDb(KonfigurasyonTuru konfigurasyonTuru)
        {
            string query = string.Empty;
            string queryWhere = string.Empty;
            FirmaDonemiTipi firmaDonemiTipi = (FirmaDonemiTipi)Enum.Parse(typeof(FirmaDonemiTipi), System.Configuration.ConfigurationManager.AppSettings["SerendipMasterDbFirmaDonemiTipi"].ToString());

            //            query = @"SELECT k.FirmaDonemi_Id, k.IsyeriObjId, k.KullaniciLogin, k.KonfigurasyonTuru, k.Dizin, k.Ozellik, k.Deger,
            //	                        f.Kodu, f.Adi, f.SirketObjId,
            //	                        fd.DonemAdi, fd.DonemNo, fb.VeritabaniAdi, s.SunucuAdi
            //                    FROM Konfigurasyon k 
            //                    LEFT JOIN FirmaDonemi fd ON fd.Id=k.FirmaDonemi_Id
            //                    LEFT JOIN Firma f ON f.Id=fd.Firma_Id
            //                    LEFT JOIN FirmaBaglantisi fb ON fb.Id=fd.FirmaBaglantisi_Id
            //                    LEFT JOIN Sunucu s ON s.Id=fb.Sunucu_Id";

            query = @"Select f.SirketObjId, f.Kodu as CustomerId, f.Adi as FirmaAdi, fg.Kodu as FirmaGrubu,
                                        fd.FirmaDonemiTipi, fd.DonemNo, fd.DonemAdi, fd.VarsayilanDonem, fd.Id, fd.DonemBaslangici, fd.DonemBitisi,
                                        fb.VeritabaniAdi SirketVeritabani, fb.KullaniciAdi, fb.KullaniciSifresi, fb.WindowsLogin, 
                                        ISNULL(fb.Encrypted, 0) AS Encrypted,
                                        s.SunucuAdi
	                            From FirmaDonemi fd 
	                            LEFT JOIN Firma f ON f.Id=fd.Firma_Id 
                                LEFT JOIN FirmaBaglantisi fb ON fb.Id = fd.FirmaBaglantisi_Id
                                LEFT JOIN Sunucu s ON s.Id = fb.Sunucu_Id
                                LEFT JOIN FirmaGrubu fg ON fg.Id = f.FirmaGrubu_Id
                                ORDER BY f.Kodu, fd.DonemNo";


            //switch (konfigurasyonTuru)
            //{
            //    case KonfigurasyonTuru.Sistem:  // FirmaDonemiTipi sadece Sistem ve Usersistem konf için geçerli
            //        queryWhere = string.Format("WHERE k.FirmaDonemiTipi={0} AND k.KonfigurasyonTuru={1}", (int)firmaDonemiTipi, (int)konfigurasyonTuru);
            //        break;
            //case KonfigurasyonTuru.UserSistem:  // FirmaDonemiTipi sadece Sistem ve Usersistem konf için geçerli
            //    queryWhere = string.Format("WHERE k.FirmaDonemiTipi={0} AND k.KonfigurasyonTuru={1} AND k.KullaniciLogin='{2}'", (int)firmaDonemiTipi, (int)konfigurasyonTuru, userName);
            //    break;
            //case KonfigurasyonTuru.Sirket:
            //    queryWhere = string.Format("WHERE k.KonfigurasyonTuru={0} AND s.SunucuAdi='{1}' AND fb.VeritabaniAdi='{2}' AND f.SirketObjId={3}", (int)konfigurasyonTuru, serverName, databaseName, sirketObjId);
            //    break;
            //case KonfigurasyonTuru.UserSirket:
            //    queryWhere = string.Format("WHERE k.KonfigurasyonTuru={0} AND s.SunucuAdi='{1}' AND fb.VeritabaniAdi='{2}' AND f.SirketObjId={3} AND k.KullaniciLogin='{4}'", (int)konfigurasyonTuru, serverName, databaseName, sirketObjId, userName);
            //    break;
            //case KonfigurasyonTuru.Isyeri:
            //    queryWhere = string.Format("WHERE k.KonfigurasyonTuru={0} AND s.SunucuAdi='{1}' AND fb.VeritabaniAdi='{2}' AND f.SirketObjId={3} AND k.IsyeriObjId={4}", (int)konfigurasyonTuru, serverName, databaseName, sirketObjId, isyeriObjId);
            //    break;
            //case KonfigurasyonTuru.UserIsyeri:
            //    queryWhere = string.Format("WHERE k.KonfigurasyonTuru={0} AND s.SunucuAdi='{1}' AND fb.VeritabaniAdi='{2}' AND f.SirketObjId={3} AND k.IsyeriObjId={4} AND k.KullaniciLogin='{5}'", (int)konfigurasyonTuru, serverName, databaseName, sirketObjId, isyeriObjId, userName);
            //    break;
            //}

            query = query + " " + queryWhere;

            var dt = new DataTable();
            using (var da = new SqlDataAdapter(query, GetDecryptedMasterDbConnectionString()))
            {
                da.Fill(dt);
            }

            return dt;
        }

        private static string PrepareConnectionString(DataRow row, bool windowsLogin)
        {
            if (windowsLogin)
            {
                return string.Format("Server={0};Database={1};Integrated Security=True;MultipleActiveResultSets=true", row["SunucuAdi"].ToString(), row["VeritabaniAdi"].ToString());
            }
            else
            {
                return string.Format("Server={0};Database={1};user={2};pwd={3};MultipleActiveResultSets=true",
                                        row["SunucuAdi"].ToString(), row["VeritabaniAdi"].ToString(), row["KullaniciAdi"].ToString(), row["KullaniciSifresi"].ToString());
            }
        }

        public static string GetMasterDBConnectionString()
        {
            string encryptedText = System.Configuration.ConfigurationManager.AppSettings["SerendipMasterDbEncryptedConnectionString"];
            string connectionString = Decrypt(encryptedText, true);
            return connectionString;
        }

        public static string Decrypt(string encryptedString, bool IsBase64EncryptedString)
        {
            if (IsBase64EncryptedString)
            {
                byte[] data = Convert.FromBase64String(encryptedString);
                encryptedString = UTF8Encoding.UTF8.GetString(data);
            }
            return Decrypt(encryptedString);
        }

        private static System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        private static byte[] key = new byte[]{
									107,140,141,112,75,57,66,129,174,222,144,225,130,0,246,45
									   };
        private static byte[] iv = new byte[]{
									190,226,124,192,102,141,26,30,194,227,131,65,24,68,255,94
									  };
        public static string Decrypt(string encryptedString)
        {
            string decryptedString = "";
            char[] encryptedChars = encryptedString.ToCharArray();
            byte[] encryptedBytes = new byte[encryptedChars.Length];
            for (int i = 0; i < encryptedBytes.Length; i++)
                encryptedBytes[i] = (byte)encryptedChars[i];
            byte[] decryptedTrueBytes;
            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            System.IO.MemoryStream inStream = new System.IO.MemoryStream(encryptedBytes);
            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(
                inStream,
                new System.Security.Cryptography.RijndaelManaged().CreateDecryptor(key, iv),
                System.Security.Cryptography.CryptoStreamMode.Read);
            cs.Read(decryptedBytes, 0, decryptedBytes.Length);
            for (int i = 0; i < decryptedBytes.Length; i++)
            {
                if (decryptedBytes[i] == '\0')
                {
                    decryptedTrueBytes = new byte[i];
                    for (int j = 0; j < decryptedTrueBytes.Length; j++)
                    {
                        decryptedTrueBytes[j] = decryptedBytes[j];
                    }

                    decryptedString = encoding.GetString(decryptedTrueBytes);
                    break;
                }
            }

            if (decryptedString == "")
                decryptedString = encoding.GetString(decryptedBytes);



            cs.Close();
            inStream.Close();
            encryptedBytes = null;
            decryptedBytes = null;
            encryptedString = null;
            inStream = null;
            cs = null;
            return decryptedString;
        }        
    }
}