using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using System.IO;

namespace Shutdown
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        private string path = Path.Combine(Environment.SystemDirectory, "shutdown.exe");
        private DateTime dateTime => DateTime.Parse($"{Date.Date.ToString("d")} {Time.TimeOfDay}");
        private TimeSpan diff => dateTime - DateTime.Now;
        public ICommand ShutdownButton => new Button(ShutDownButton,ShutDownButtonCanExecute);
        public ICommand CancelShutdownButton => new Button(CancelShutDown,()=>true);
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime Time { get; set; } = DateTime.Now;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void CancelShutDown(object? o)
        {
            Process.Start(@path, $"-a");
        }

        private void ShutDownButton(object? o)
        {
            Process.Start(@path, $"/s /t {Math.Round(diff.TotalSeconds)}");
        }
        private bool ShutDownButtonCanExecute()
        {
            return diff > TimeSpan.FromSeconds(0);
        }
        private void RaisePropertyChanged([CallerMemberName] string? name = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
