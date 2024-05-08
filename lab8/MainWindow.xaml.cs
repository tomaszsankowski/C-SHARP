using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Controls.MenuItem;
using System.Text.RegularExpressions;
using Application = System.Windows.Application;

namespace lab8
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            treeView.Items.Clear();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog() { Description = "Select directory to open" };
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = dlg.SelectedPath;

                string rootDirectoryName = new System.IO.DirectoryInfo(selectedPath).Name;

                var root = new TreeViewItem
                {
                    Header = rootDirectoryName,
                    Tag = selectedPath
                };

                treeView.Items.Add(root);
                AddContextMenu(root, true);

                AddChilds(selectedPath, root);
            }
        }

        private void AddChilds(string directoryPath, TreeViewItem parentNode)
        {
            try
            {
                if (!System.IO.Directory.Exists(directoryPath))
                    return;
                
                string[] directories = System.IO.Directory.GetDirectories(directoryPath);

                foreach (string directory in directories)
                {
                    var directoryItem = new TreeViewItem
                    {
                        Header = new System.IO.DirectoryInfo(directory).Name,
                        Tag = directory
                    };

                    parentNode.Items.Add(directoryItem);
                    AddContextMenu(directoryItem, true);

                    AddChilds(directory, directoryItem);
                }

                string[] files = System.IO.Directory.GetFiles(directoryPath);

                foreach (string file in files)
                {
                    var fileItem = new TreeViewItem
                    {
                        Header = new System.IO.FileInfo(file).Name,
                        Tag = file
                    };

                    parentNode.Items.Add(fileItem);
                    AddContextMenu(fileItem, false);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Error: " + e.Message);
            }
        }

        private void AddContextMenu(TreeViewItem treeViewItem, bool isDirectory)
        {
            var contextMenu = new ContextMenu();

            if(isDirectory)
            {
                var createMenuItem = new MenuItem() { Header = "Create" };
                createMenuItem.Click += (sender, e) => CreateItem(treeViewItem);
                contextMenu.Items.Add(createMenuItem);
            }
            else
            {
                var openFileMenuItem = new MenuItem() { Header = "Open" };
                openFileMenuItem.Click += (sender, e) => OpenItem(treeViewItem);
                contextMenu.Items.Add(openFileMenuItem);
            }
            
            var deleteMenuItem = new MenuItem() { Header = "Delete" };
            deleteMenuItem.Click += (sender, e) => DeleteItem(treeViewItem);
            contextMenu.Items.Add(deleteMenuItem);

            treeViewItem.ContextMenu = contextMenu;
        }

        private void DeleteItem(TreeViewItem selectedTreeViewItem)
        {
            try
            {
                if (selectedTreeViewItem.Tag is string directoryPath && System.IO.Directory.Exists(directoryPath))
                {
                    try
                    {
                        var directoryInfo = new System.IO.DirectoryInfo(directoryPath);

                        if ((directoryInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                        {
                            directoryInfo.Attributes &= ~System.IO.FileAttributes.ReadOnly;
                        }

                        directoryInfo.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }

                if (selectedTreeViewItem.Parent is TreeViewItem parentItem)
                {
                    parentItem.Items.Remove(selectedTreeViewItem);
                }
                else
                {
                    treeView.Items.Remove(selectedTreeViewItem);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while deleting the item: {ex.Message}");
            }
        }

        private void CreateItem(TreeViewItem parentItem)
        {
            var dlg = new CreateItemDialog();
            if (dlg.ShowDialog() == true)
            {
                string itemName = dlg.ItemName;
                bool isFile = dlg.IsFile;
                bool isFolder = dlg.IsFolder;
                bool isArchive = dlg.IsArchive;
                bool isHidden = dlg.IsHidden;
                bool isReadOnly = dlg.IsReadOnly;
                bool isSystem = dlg.IsSystem;

                string pattern = @"^[a-zA-Z0-9-_~]{1,8}\.(txt|php|html)$";

                if (isFile && Regex.IsMatch(itemName, pattern))
                {
                    try
                    {
                        string fullPath = System.IO.Path.Combine((string)parentItem.Tag, itemName);
                        if (!System.IO.File.Exists(fullPath))
                        {
                            using (System.IO.FileStream fs = System.IO.File.Create(fullPath))
                            {
                                System.IO.FileAttributes attributes = System.IO.FileAttributes.Normal;

                                if (isReadOnly)
                                {
                                    attributes |= System.IO.FileAttributes.ReadOnly;
                                }

                                if (isHidden)
                                {
                                    attributes |= System.IO.FileAttributes.Hidden;
                                }

                                if (isSystem)
                                {
                                    attributes |= System.IO.FileAttributes.System;
                                }

                                if (isArchive)
                                {
                                    attributes |= System.IO.FileAttributes.Archive;
                                }

                                System.IO.File.SetAttributes(fullPath, attributes);

                                TreeViewItem newItem = new TreeViewItem
                                {
                                    Header = itemName,
                                    Tag = fullPath
                                };
                                parentItem.Items.Add(newItem);
                                AddContextMenu(newItem, false);
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Plik o tej nazwie już istnieje!");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Błąd podczas tworzenia pliku: {ex.Message}");
                    }
                }
                else if (isFolder)
                {
                    try
                    {
                        string fullPath = System.IO.Path.Combine((string)parentItem.Tag, itemName);
                        if (!System.IO.Directory.Exists(fullPath))
                        {
                            System.IO.Directory.CreateDirectory(fullPath);

                            System.IO.FileAttributes attributes = System.IO.FileAttributes.Normal;

                            if (isReadOnly)
                            {
                                attributes |= System.IO.FileAttributes.ReadOnly;
                            }

                            if (isHidden)
                            {
                                attributes |= System.IO.FileAttributes.Hidden;
                            }

                            if (isSystem)
                            {
                                attributes |= System.IO.FileAttributes.System;
                            }

                            if (isArchive)
                            {
                                attributes |= System.IO.FileAttributes.Archive;
                            }

                            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(fullPath)
                            {
                                Attributes = attributes
                            };

                            TreeViewItem newItem = new TreeViewItem
                            {
                                Header = itemName,
                                Tag = fullPath
                            };
                            parentItem.Items.Add(newItem);
                            AddContextMenu(newItem, true);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Folder o tej nazwie już istnieje!");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Błąd podczas tworzenia folderu: {ex.Message}");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Nie można utworzyć pliku o takim rozszerzeniu lub niepoprawnej nazwie!");
                }
            }
        }



        private void OpenItem(TreeViewItem selectedItem)
        {
            string filePath = (string)selectedItem.Tag;
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    string fileContent = System.IO.File.ReadAllText(filePath);
                    fileContentTextBlock.Text = fileContent;

                    System.IO.FileAttributes attributes = System.IO.File.GetAttributes(filePath);
                    openedStatusBarTextBlock.Text = GetCompactAttributes(attributes);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while opening the item: {ex.Message}");
            }
        }

        private string GetCompactAttributes(System.IO.FileAttributes attributes)
        {
            StringBuilder sb = new StringBuilder();

            if ((attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
            {
                sb.Append("r");
            }
            else
            {
                sb.Append("-");
            }

            if ((attributes & System.IO.FileAttributes.Archive) == System.IO.FileAttributes.Archive)
            {
                sb.Append("a");
            }
            else
            {
                sb.Append("-");
            }

            if ((attributes & System.IO.FileAttributes.System) == System.IO.FileAttributes.System)
            {
                sb.Append("s");
            }
            else
            {
                sb.Append("-");
            }

            if ((attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
            {
                sb.Append("h");
            }
            else
            {
                sb.Append("-");
            }

            return sb.ToString();
        }


        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem selectedItem)
            {
                string itemPath = selectedItem.Tag as string;
                if (!string.IsNullOrEmpty(itemPath))
                {
                    try
                    {
                        System.IO.FileAttributes attributes = System.IO.File.GetAttributes(itemPath);
                        statusBarTextBlock.Text = GetCompactAttributes(attributes);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"An error occurred while reading attributes: {ex.Message}");
                    }
                }
            }
        }
    }
}
