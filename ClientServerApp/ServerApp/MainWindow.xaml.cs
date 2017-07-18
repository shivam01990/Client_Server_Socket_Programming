using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonLib.Controller;
namespace ServerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// commad table
        private Table commandtable = new Table();

        /// Current Row
        private int currentRow = 0;

        public MainWindow()
        {
            InitializeComponent();
            Paragraph p = RichTextBoxCommand.Document.Blocks.FirstBlock as Paragraph;
            p.LineHeight = 0.1;

            GridLengthConverter gridLenghtConvertor = new GridLengthConverter();
            this.commandtable = new Table();
            this.commandtable.Columns.Add(new TableColumn() { Width = (GridLength)gridLenghtConvertor.ConvertFromString("200") });
            this.commandtable.Columns.Add(new TableColumn { Width = (GridLength)gridLenghtConvertor.ConvertFromString("300") });
            RichTextBoxCommand.Document.Blocks.Add(this.commandtable);
        }

        /// Gets or sets communication
        private CommunicationController Communication { get; set; }

        /// Connect button
        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            int portNo = 12345;
            string result = string.Empty;
            try
            {
                this.Communication = new CommunicationController(portNo);

                this.Communication.OnRequestReceived += (request) =>
                {
                    result = request;
                };

                this.Communication.OnLookUpComplete += (response) =>
                {
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            if (this.commandtable.RowGroups.Count == 0)
                            {
                                this.commandtable.RowGroups.Add(new TableRowGroup());
                            }

                            this.commandtable.RowGroups[0].Rows.Add(new TableRow());
                            TableRow tabRow = commandtable.RowGroups[0].Rows[currentRow];
                            tabRow.Cells.Add(new TableCell(new Paragraph(new Run(result))));
                            tabRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow++;
                            RichTextBoxCommand.Document.Blocks.Add(this.commandtable);
                        });
                    }
                };
                this.Communication.StartServer();
                RichTextBoxCommand.AppendText(string.Format(Environment.NewLine));
                this.EnableDisable(false);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// Enable disable
        private void EnableDisable(bool enableFlag)
        {
            buttonDisConnect.IsEnabled = !enableFlag;
            buttonConnect.IsEnabled = enableFlag;
        }

        /// Rich Text Box Mouse Up Command
        private void RichTextBoxCommand_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == Mouse.RightButton)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem selectAllmenu = new MenuItem();
                selectAllmenu.Header = "Select All";

                selectAllmenu.Click += (o, a) =>
                {
                    RichTextBoxCommand.SelectAll();
                };

                MenuItem copymenu = new MenuItem();
                copymenu.Header = "Copy";
                if (RichTextBoxCommand.Selection.Text.Length == 0)
                {
                    copymenu.IsEnabled = false;
                }

                copymenu.Click += (o, a) =>
                {
                    RichTextBoxCommand.Copy();
                };

                contextMenu.Items.Add(selectAllmenu);
                contextMenu.Items.Add(copymenu);
                RichTextBoxCommand.ContextMenu = contextMenu;
            }
        }

        private void ButtonDisConnect_Click(object sender, RoutedEventArgs e)
        {
            if (this.Communication != null)
            {
                this.Communication.StopServer();
                RichTextBoxCommand.AppendText(Environment.NewLine);
                this.EnableDisable(true);
            }
        }
    }
}
