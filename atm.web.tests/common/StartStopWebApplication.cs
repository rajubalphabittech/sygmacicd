namespace atm.web.tests.common
{
    public class StartStopWebApplication
    {
        //private static Process _iisExpressProcess;

        //public static void StartIISExpress()
        //{
        //    var applicationPath = GetApplicationPath(Hooks.ConstantsUtils.WebProjectName);
        //    var arguments = string.Format(CultureInfo.InvariantCulture, "/path:\"{0}\" /port:{1}", applicationPath, Hooks.ConstantsUtils.PortNumber);
        //    var startInfo = new ProcessStartInfo("dotnet.exe ")
        //    {
        //        WorkingDirectory = applicationPath,
        //        WindowStyle = ProcessWindowStyle.Hidden,
        //        ErrorDialog = true,
        //        LoadUserProfile = true,
        //        CreateNoWindow = true,
        //        UseShellExecute = false,
        //        Arguments = " run"
        //    };

        //    _iisExpressProcess = Process.Start(startInfo);
        //}

        //private static string GetApplicationPath(string applicationName)
        //{
        //    var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
        //    return Path.Combine(solutionFolder, "src", applicationName);
        //}

        //public static void StopIISExpress()
        //{
        //    IEnumerable<Process> processes = Process.GetProcesses().Where(p => p.ProcessName == "dotnet" && p.HasExited == false).AsEnumerable();

        //    foreach (Process process in processes) process.Kill();

        //    if (_iisExpressProcess != null)
        //    {
        //        if (!_iisExpressProcess.HasExited)
        //        {
        //            _iisExpressProcess.Kill();
        //            _iisExpressProcess.Dispose();
        //        }
        //        _iisExpressProcess = null;
        //    }
        //}
       
    }
}
