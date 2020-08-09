using System;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using SignalRAssignmentApp.Hubs;

namespace SignalRAssignmentApp
{
    public class MyFileWatcher : IMyFileWatcher
    {
        private readonly IHubContext<FileWatcherHub> _hubContext;
        private readonly string serverDirectory = @"D:\";

        public MyFileWatcher(IHubContext<FileWatcherHub> hubContext)
        {
            _hubContext = hubContext;
            var watcher = new FileSystemWatcher();

            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;
            watcher.Changed += OnChanged;

            // tell the watcher where to look
            watcher.Path = serverDirectory;

            // You must add this line - this allows events to fire.
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var file = new FileDetails
                {Name = e.Name, Time = DateTime.Now.ToLocalTime(), ChangeType = e.ChangeType.ToString()};
            _hubContext.Clients.All.SendAsync("onFileChange", file);
        }
    }
}