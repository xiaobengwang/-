using System;

using System.Collections.Generic;

using System.Text;

using System.IO;

using System.Net;
using System.Collections;

 

using System.Globalization;



namespace FtpTest1
{



    public class FtpWeb
    {

        string ftpServerIP;

        string ftpRemotePath;

        string ftpUserID;

        string ftpPassword;

        string ftpURI;

    

        /// <summary>

        /// 连接FTP

        /// </summary>

        /// <param name="FtpServerIP">FTP连接地址</param>

        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>

        /// <param name="FtpUserID">用户名</param>

        /// <param name="FtpPassword">密码</param>

        public FtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {

            ftpServerIP = FtpServerIP;

            ftpRemotePath = FtpRemotePath;

            ftpUserID = FtpUserID;

            ftpPassword = FtpPassword;

            ftpURI = "ftp://" + ftpServerIP + "/";

        }





        static void Main()
        {

            //string file = "c:\\aq3.gifa";

            //FileInfo fileInf = new FileInfo(file);

            //if (!fileInf.Exists)

            //{

            //    Console.WriteLine(file + " no exists");

            //}

            //else {

            //    Console.WriteLine("yes");

            //}

            //Console.ReadLine();

            FtpWeb fw = new FtpWeb("10.8.0.17", "", "xiaobengwang", "123456");

            fw.DeleteDir("1");
            string []b=fw.GetFileList("2");
          
           
            Console.ReadLine();
        }


        public void Delete(string filename)//删除文件
        {

            try
            {


                
                FtpWebRequest reqFTP;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(filename));



                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP.KeepAlive = false;

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;



                string result = String.Empty;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                long size = response.ContentLength;

                Stream datastream = response.GetResponseStream();

                StreamReader sr = new StreamReader(datastream);

                result = sr.ReadToEnd();

                sr.Close();

                datastream.Close();

                response.Close();

            }

            catch (Exception ex)
            {


            }

        }
        private void DeleteFolder(string path)   //path为所要删除的文件夹的全路径
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = false;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void DeleteDir(string path)
        {
            try
            {
                string[] folderArray =this.GetDirectoryList(path);
                string[] fileArray = this.GetFileList(path);

              
                ArrayList folderArrayList = new ArrayList();
                ArrayList fileArrayList = new ArrayList();

                //重新构造存放文件夹的数组(用动态数组实现)
                for (int i = 0; i < folderArray.Length; i++)
                {


                    folderArrayList.Add(folderArray[i]);

                      
                }

                //重新构造存放文件的数组(用动态数组实现)
                for (int i = 0; i < fileArray.Length; i++)
                {
                   
                        fileArrayList.Add(fileArray[i]);
                    
                }

                if (folderArrayList.Count == 1 && fileArrayList.Count == 0)
                {
                   this. DeleteFolder(ftpURI+path);
                }
                else if (folderArrayList.Count == 0 && fileArrayList.Count != 0)
                {
                    for (int i = 0; i < fileArrayList.Count; i++)
                    {
                        string fileUri = ftpURI+path + "/" + fileArrayList[i];
                        this.Delete(fileUri);
                    }
                  this.  DeleteFolder(ftpURI+path);
                }
                else if (folderArrayList.Count != 0 && fileArrayList.Count != 0)
                {
                    for (int i = 0; i < fileArrayList.Count; i++)
                    {
                        string fileUri = ftpURI+ path + "/" + fileArrayList[i];
                        this.Delete(fileUri);
                    }
                    for (int i = 0; i < folderArrayList.Count; i++)
                    {
                        string dirUri = ftpURI+path + "/" + folderArrayList[i];
                       this. DeleteDir(dirUri);
                    }
                   this. DeleteFolder(ftpURI+path);
                }
                else if (folderArrayList.Count != 0 && fileArrayList.Count == 0)
                {
                    for (int i = 0; i < folderArrayList.Count; i++)
                    {
                        string dirUri = path + "/" + folderArrayList[i];
                        this.DeleteDir(dirUri);
                    }
                   this. DeleteFolder(ftpURI+path);
                }
            }
            catch (Exception ex)
            {
               
            }
        }
       
       
        public  void   shanchusuoyou (string filepath)
        {

            string[] a = this.GetFileList(filepath);
            if (a != null && a.Length > 0)
            {
                foreach (string file in a)
                {
                    this.Delete(ftpURI + filepath + "/" + file);
                }
            }
            string[] c= this.GetDirectoryList(filepath);
        
                foreach (string file2 in c)
                {

                    this.DeleteFolder(ftpURI+filepath + "/" + file2);
                }


           

           
                    
                                                     
                
              
              
               
            
            }
           

        
        
    


        public void shanchu(string[] filename)
        {

     

            if (filename != null && filename.Length > 0)
            {

                foreach (var file in filename)
                {
                   
                    DeleteFolder(ftpURI+file);



                }

            }
        }
       

        public string UploadFile(string[] filePaths)
        {

            StringBuilder sb = new StringBuilder();

            if (filePaths != null && filePaths.Length > 0)
            {

                foreach (var file in filePaths)
                {

                    sb.Append(Upload(file));



                }

            }

            return sb.ToString();

        }



        /// <summary>

        /// 上传文件

        /// </summary>

        /// <param name="filename"></param>

        private string Upload(string filename)
        {

            FileInfo fileInf = new FileInfo(filename);

            if (!fileInf.Exists)
            {

                return filename + " 不存在!\n";

            }



            string uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));



            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

            reqFTP.KeepAlive = false;

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.UsePassive = false;  //选择主动还是被动模式

            //Entering Passive Mode

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            FileStream fs = fileInf.OpenRead();

            try
            {

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            return "";

        }





        /// <summary>

        /// 下载

        /// </summary>

        /// <param name="filePath"></param>

        /// <param name="fileName"></param>

        public void Download(string filePath, string fileName)
        {

            FtpWebRequest reqFTP;

            try
            {

                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);



                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;

                int bufferSize = 2048;

                int readCount;

                byte[] buffer = new byte[bufferSize];



                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {

                    outputStream.Write(buffer, 0, readCount);

                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                }



                ftpStream.Close();

                outputStream.Close();

                response.Close();

            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }


      




        /// <summary>

        /// 获取当前目录下明细(包含文件和文件夹)

        /// </summary>

        /// <returns></returns>

        public string[] GetFilesDetailList(string filepath)
        {

            string[] downloadFiles;
            string URI = ftpURI + filepath;
            try
            {

                StringBuilder result = new StringBuilder();

                FtpWebRequest ftp;


                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(URI));

                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = ftp.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                string line = reader.ReadLine();

                

                

                while (line != null)
                {

                    result.Append(line);

                    result.Append("\n");

                    line = reader.ReadLine();

                }

                result.Remove(result.ToString().LastIndexOf("\n"), 1);

                reader.Close();

                response.Close();

                return result.ToString().Split('\n');

            }

            catch (Exception ex)
            {

                downloadFiles = null;

               

                return downloadFiles;

            }

        }



        /// <summary>

        /// 获取当前目录下文件列表(仅文件)

        /// </summary>

        /// <returns></returns>

        public  string[] GetFileList(string filepath)
        {

            string URI = ftpURI + filepath;
            string[] downloadFiles;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(URI));
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
              
            }
            catch (Exception )
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }


       

        /// <summary>

        /// 获取当前目录下所有的文件夹列表(仅文件夹)

        /// </summary>

        /// <returns></returns>

        public string[] GetDirectoryList(string filepath)
        {
            
            string[] drectory = GetFilesDetailList(filepath);

            string m = string.Empty;

           
                foreach (string str in drectory)
                {

                    if (str.Trim().Substring(0, 1).ToUpper() == "D" && str.Trim().Substring(55, 1) != "." )
                    {

                        m += str.Substring(54).Trim() + "\n";

                    }

                }

            
         

            char[] n = new char[] { '\n' };

            return m.Split(n);

        }



        /// <summary>

        /// 判断当前目录下指定的子目录是否存在

        /// </summary>

        /// <param name="RemoteDirectoryName">指定的目录名</param>

  


        /// <summary>

        /// 判断当前目录下指定的文件是否存在

        /// </summary>

        /// <param name="RemoteFileName">远程文件名</param>

       


        /// <summary>

        /// 创建文件夹

        /// </summary>

        /// <param name="dirName"></param>

        public void MakeDir(string dirName)
        {

            FtpWebRequest reqFTP;

            try
            {

                // dirName = name of the directory to create.

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + dirName));

                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();



                ftpStream.Close();

                response.Close();

            }

            catch (Exception ex)
            {

                Console.WriteLine("FtpWeb", "MakeDir Error --> " + ex.Message);

            }

        }



        /// <summary>

        /// 获取指定文件大小

        /// </summary>

        /// <param name="filename"></param>

        /// <returns></returns>

        public long GetFileSize(string filename)
        {

            FtpWebRequest reqFTP;

            long fileSize = 0;

            try
            {

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + filename));

                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();

                fileSize = response.ContentLength;



                ftpStream.Close();

                response.Close();

            }

            catch (Exception ex)
            {

                Console.WriteLine("FtpWeb", "GetFileSize Error --> " + ex.Message);

            }

            return fileSize;

        }



        /// <summary>

        /// 改名

        /// </summary>

        /// <param name="currentFilename"></param>

        /// <param name="newFilename"></param>

        public void ReName(string currentFilename, string newFilename)
        {

            FtpWebRequest reqFTP;

            try
            {

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + currentFilename));

                reqFTP.Method = WebRequestMethods.Ftp.Rename;

                reqFTP.RenameTo = newFilename;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();



                ftpStream.Close();

                response.Close();

            }

            catch (Exception ex)
            {

                Console.WriteLine("FtpWeb", "ReName Error --> " + ex.Message);

            }

        }



        /// <summary>

        /// 移动文件

        /// </summary>

        /// <param name="currentFilename"></param>

        /// <param name="newFilename"></param>

        public void MovieFile(string currentFilename, string newDirectory)
        {

            ReName(currentFilename, newDirectory);

        }
    }
}
