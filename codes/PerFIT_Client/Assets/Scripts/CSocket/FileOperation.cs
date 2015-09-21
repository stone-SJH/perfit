using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Client.FileOp
{
    class FileOperation
    {       
        private string _fileName;
        private string _folder;
        private string _path;
        private FileStream _fileStream;
        //初始化参数
        public FileOperation(string path)
        {
            _fileName = path.Substring(path.LastIndexOf('\\') + 1);
            _folder = path.Substring(0, path.Length - _fileName.Length);
            _path = path;
        }

       
        //得到路径
        public string GetPath()
        {
            return _path;
        }


        //清空文件内容
        public void ClearFile()
        {
            FileStream fileStream = File.Create(_path);
            fileStream.Close();
        }

        //以读取模式打开文件
        public void OpenFile()
        {          
            _fileStream = new FileStream(_path, FileMode.Open);
            _fileStream.Seek(0, SeekOrigin.Begin);
        }

        //关闭文件
        public void CloseFile()
        {
            _fileStream.Close();
        }

        //读取文件
        //从头读取整个文件
        public void ReadContent(out List<string> content)
        {
            content = new List<string>();
            StreamReader streamReader = new StreamReader(_path, Encoding.Default);
            string line = "";
            while ((line = streamReader.ReadLine()) != null)
            {
                content.Add(line);
            }
            streamReader.Close();
        }

        //从上次读取位置读取文件
        public void ReadContent(out string content, int size)
        {
            content = "";
            byte[] buf = new byte[size];
            int newSize = _fileStream.Read(buf, 0, size);
            content = Encoding.Default.GetString(buf);
            content = content.Substring(0, newSize);
        }

        //二进制读取整个文件
        public void ReadContentBinary(out List <byte[]> content)
        {
            int maxLength = 1000;
            content = new List<byte[]>();
            FileStream fileStream = new FileStream(_path, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            byte[] buf = binaryReader.ReadBytes(maxLength);
            content.Add(buf);
            while (buf.Length == maxLength)
            {
                buf = binaryReader.ReadBytes(maxLength);
                content.Add(buf);
            }
            binaryReader.Close();
            fileStream.Close();
        }

        //写入文件
        public void WriteContent(List<string> content)
        {
            FileStream fileStream = new FileStream(_path, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            foreach (string line in content)
            {
                streamWriter.WriteLine(line);
            }
            streamWriter.Close();
            fileStream.Close();
        }

        public void WriteContent(string content)
        {
            FileStream fileStream = new FileStream(_path, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(content);
            streamWriter.Close();
            fileStream.Close();
        }


        //二进制写入文件
        public void WriteContentBinary(List<byte[]> content)
        {
            FileStream fileStream = new FileStream(_path, FileMode.Append);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            binaryWriter.Seek(0, SeekOrigin.Begin);
            foreach (byte[] buf in content)
            {
                binaryWriter.Write(buf);
            }
            binaryWriter.Close();
            fileStream.Close();
        }
    }
}
