using System.Reflection;

namespace AppleCaps
{
    public class AppContext : ApplicationContext
    {
        private NotifyIcon TrayIcon = new NotifyIcon()
        {
            Text = "AppleCaps - Running",
            Visible = true,
            ContextMenuStrip = new ContextMenuStrip(),
            Icon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("AppleCaps.icon.ico"))
        };

        public AppContext()
        {
            TrayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
            Application.ApplicationExit += new EventHandler(OnExit);
        }

        private void OnExit(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            Application.Exit();
        }
    }
}