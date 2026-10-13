using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace TelessageDesktop
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TelessageWindow());
        }
    }

    public sealed class TelessageWindow : Form
    {
        private readonly WebView2 browser = new WebView2();

        public TelessageWindow()
        {
            Text = "Telessage Desktop";
            Width = 1180;
            Height = 760;
            MinimumSize = new System.Drawing.Size(800, 560);
            StartPosition = FormStartPosition.CenterScreen;

            browser.Dock = DockStyle.Fill;
            Controls.Add(browser);
            Shown += async (s, e) =>
            {
                try
                {
                    var userData = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Telessage", "WebView2");
                    Directory.CreateDirectory(userData);

                    var env = await CoreWebView2Environment.CreateAsync(null, userData);
                    await browser.EnsureCoreWebView2Async(env);

                    browser.CoreWebView2.NewWindowRequested += (sender, args) =>
                    {
                        args.Handled = true;
                        try { Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true }); }
                        catch { }
                    };

                    browser.CoreWebView2.NavigationStarting += (sender, args) =>
                    {
                        if (Uri.TryCreate(args.Uri, UriKind.Absolute, out Uri uri) &&
                            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                        {
                            args.Cancel = true;
                            try { Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true }); }
                            catch { }
                        }
                    };

                    var htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Telessage", "index.html");
                    if (!File.Exists(htmlPath))
                    {
                        MessageBox.Show("No se encontró Telessage\\index.html junto al ejecutable.",
                            "Telessage", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                        return;
                    }
                    browser.CoreWebView2.Navigate(new Uri(htmlPath).AbsoluteUri);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "No se pudo iniciar WebView2. Comprueba que el Runtime de Microsoft Edge WebView2 esté instalado.\\n\\n" + ex.Message,
                        "Telessage Desktop", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
