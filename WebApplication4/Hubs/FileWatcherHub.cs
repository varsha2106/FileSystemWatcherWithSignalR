using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRAssignmentApp.Hubs
{
    public class FileWatcherHub : Hub
    {
        private readonly string serverDirectory = @"D:\";

        public Task NotifyConnection()
        {
            return Clients.All.SendAsync("TestBrodcasting", $"Testing a Basic HUB at {DateTime.Now.ToLocalTime()}");
        }

        public Task AppendFile(string text)
        {
            var d = new DirectoryInfo(serverDirectory);
            var finfo = d.GetFiles();
            File.AppendAllText(finfo[0].FullName, text);
            return Clients.All.SendAsync("TestBrodcasting", "File appended");
        }

        public Task CreateFile(string text)
        {
            var fileName = serverDirectory + text;
            if (File.Exists(fileName)) File.Delete(fileName);
            using var sw = File.CreateText(fileName);
            sw.WriteLine("New file created: {0}", DateTime.Now.ToString(CultureInfo.CurrentCulture));
            return Clients.All.SendAsync("TestBrodcasting", "File created");
        }

        public Task DeleteFile()
        {
            var files = Directory.GetFiles(serverDirectory);
            File.Delete(files[0]);
            return Clients.All.SendAsync("TestBrodcasting", "File deleted");
        }

        public Task RenameFile(string text)
        {
            var d = new DirectoryInfo(serverDirectory);
            var finfo = d.GetFiles();
            var newFileName = finfo[0].FullName.Replace(finfo[0].Name, text);
            File.Move(finfo[0].FullName, newFileName);
            return Clients.All.SendAsync("TestBrodcasting", "File renamed");
        }
    }
}