using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Windows.Forms;
using MusicDirectoryApp.Data;
using System.Diagnostics;

namespace MusicDirectoryApp.Forms
{
    public partial class FormMain : Form
    {
        // Прямая строка подключения
        private string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MusicDirectory;Integrated Security=True";
        private DataTable albumsTable;
        private bool isDataLoaded = false;

        public FormMain()
        {
            InitializeComponent();
            SetupUI();

            // Подписываемся на события
            dataGridViewAlbums.DataBindingComplete += DataGridViewAlbums_DataBindingComplete;

            // Загружаем данные после полной инициализации формы
            this.Load += (s, e) => LoadAlbums();
        }

        private void SetupUI()
        {
            if (AuthService.CurrentUser != null)
            {
                this.Text = $"Справочник Меломана - {AuthService.CurrentUser.Username}";
            }
            else
            {
                this.Text = "Справочник Меломана (демо-режим)";
            }

            this.StartPosition = FormStartPosition.CenterScreen;

            // Настройка видимости кнопок в зависимости от прав
            bool isAdmin = AuthService.CurrentUser?.IsAdmin ?? false;
            btnAddAlbum.Visible = isAdmin;
            btnEditAlbum.Visible = isAdmin;
            btnDeleteAlbum.Visible = isAdmin;

            // Показываем роль пользователя
            if (AuthService.CurrentUser != null)
            {
                lblUserInfo.Text = $"Пользователь: {AuthService.CurrentUser.Username} ({AuthService.CurrentUser.Role})";
                lblUserInfo.ForeColor = isAdmin ? Color.Green : Color.Blue;
            }
            else
            {
                lblUserInfo.Text = "Режим: Демонстрационный";
                lblUserInfo.ForeColor = Color.Gray;
            }

            // Инициализация PictureBox
            pictureBoxAlbum.Image = CreateDefaultAlbumCover();
        }

        private void DataGridViewAlbums_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Этот метод вызывается после полной привязки данных
            if (e.ListChangedType == System.ComponentModel.ListChangedType.Reset)
            {
                Debug.WriteLine("DataBindingComplete вызван, настраиваем DataGridView...");
                SafeConfigureDataGridView();
            }
        }

        private void LoadAlbums()
        {
            if (isDataLoaded) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    /*string query = @"
                        SELECT 
                            a.album_id,
                            a.album_title AS [Название альбома],
                            ar.artist_name AS [Исполнитель],
                            a.release_year AS [Год выпуска],
                            g.genre_name AS [Жанр],
                            a.label AS [Лейбл],
                            FORMAT(a.rating, '0.0') AS [Рейтинг],
                            a.total_tracks AS [Треков],
                            a.image_path AS [image_path]
                        FROM Albums a
                        LEFT JOIN Artists ar ON a.artist_id = ar.artist_id
                        LEFT JOIN Genres g ON a.genre_id = g.genre_id
                        ORDER BY a.release_year DESC";*/

                    string query = @"
                        SELECT DISTINCT
                            a.album_id,
                            a.album_title AS [Название альбома],
                            ar.artist_name AS [Исполнитель],
                            a.release_year AS [Год выпуска],
                            g.genre_name AS [Жанр],
                            a.label AS [Лейбл],
                            FORMAT(a.rating, '0.0') AS [Рейтинг],
                            a.total_tracks AS [Треков],
                            a.image_path AS [image_path],
                            STUFF((SELECT ', ' + track_title 
                                FROM Tracks t 
                                WHERE t.album_id = a.album_id 
                                ORDER BY t.track_number
                                FOR XML PATH('')), 1, 2, '') AS [Список треков]
                            FROM Albums a
                            LEFT JOIN Artists ar ON a.artist_id = ar.artist_id
                            LEFT JOIN Genres g ON a.genre_id = g.genre_id
                            ORDER BY a.release_year DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    albumsTable = new DataTable();

                    // Заполняем таблицу
                    adapter.Fill(albumsTable);

                    Debug.WriteLine($"Загружено {albumsTable.Rows.Count} записей");
                    Debug.WriteLine("Колонки в DataTable:");
                    foreach (DataColumn col in albumsTable.Columns)
                    {
                        Debug.WriteLine($"  - {col.ColumnName}");
                    }

                    // Привязываем данные к DataGridView
                    // Важно: делаем это в основном потоке UI
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            dataGridViewAlbums.DataSource = albumsTable;
                            UpdateStats();
                        }));
                    }
                    else
                    {
                        dataGridViewAlbums.DataSource = albumsTable;
                        UpdateStats();
                    }

                    isDataLoaded = true;
                }
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine($"SQL Ошибка: {sqlEx.Message}");
                ShowDatabaseErrorAndFallback();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Общая ошибка: {ex.Message}");
                ShowDatabaseErrorAndFallback();
            }
        }

        private void ShowDatabaseErrorAndFallback()
        {
            MessageBox.Show("Не удалось загрузить данные из базы данных. Приложение перейдет в демо-режим.",
                "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            CreateTestData();
        }

        private void CreateTestData()
        {
            try
            {
                // Создаем тестовые данные
                albumsTable = new DataTable();

                // Добавляем колонки
                albumsTable.Columns.Add("album_id", typeof(int));
                albumsTable.Columns.Add("Название альбома", typeof(string));
                albumsTable.Columns.Add("Исполнитель", typeof(string));
                albumsTable.Columns.Add("Год выпуска", typeof(int));
                albumsTable.Columns.Add("Жанр", typeof(string));
                albumsTable.Columns.Add("Лейбл", typeof(string));
                albumsTable.Columns.Add("Рейтинг", typeof(decimal));
                albumsTable.Columns.Add("Треков", typeof(int));
                albumsTable.Columns.Add("image_path", typeof(string));

                // Добавляем тестовые строки
                albumsTable.Rows.Add(1, "Группа крови", "Кино", 1988, "Рок", "Мелодия", 4.8, 10, null);
                albumsTable.Rows.Add(2, "Nevermind", "Nirvana", 1991, "Гранж", "Geffen", 4.9, 12, null);
                albumsTable.Rows.Add(3, "Back in Black", "AC/DC", 1980, "Хард-рок", "Atlantic", 4.7, 10, null);
                albumsTable.Rows.Add(4, "Герой асфальта", "Ария", 1987, "Метал", "Мелодия", 4.6, 8, null);
                albumsTable.Rows.Add(5, "Abbey Road", "The Beatles", 1969, "Рок", "Apple", 5.0, 17, null);
                albumsTable.Rows.Add(6, "The Dark Side of the Moon", "Pink Floyd", 1973, "Прогрессивный рок", "Harvest", 4.9, 10, null);
                albumsTable.Rows.Add(7, "Thriller", "Michael Jackson", 1982, "Поп", "Epic", 4.8, 9, null);
                albumsTable.Rows.Add(8, "Rumours", "Fleetwood Mac", 1977, "Рок", "Warner Bros.", 4.7, 11, null);

                // Привязываем данные
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        dataGridViewAlbums.DataSource = albumsTable;
                        SafeConfigureDataGridView();
                        UpdateStats();
                    }));
                }
                else
                {
                    dataGridViewAlbums.DataSource = albumsTable;
                    SafeConfigureDataGridView();
                    UpdateStats();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания тестовых данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SafeConfigureDataGridView()
        {
            try
            {
                // Ждем, пока DataGridView полностью инициализируется
                if (dataGridViewAlbums == null || dataGridViewAlbums.IsDisposed)
                    return;

                // Приостанавливаем обновление для избежания мерцания
                dataGridViewAlbums.SuspendLayout();

                // Проверяем наличие колонок
                int columnCount = dataGridViewAlbums.Columns?.Count ?? 0;
                Debug.WriteLine($"Настройка DataGridView, колонок: {columnCount}");

                if (columnCount == 0)
                {
                    Debug.WriteLine("Нет колонок для настройки");
                    return;
                }

                // Настройка видимости колонок с проверкой
                SafeSetColumnVisible("album_id", false);
                SafeSetColumnVisible("image_path", false);
                SafeSetColumnVisible("Список треков", false);

                // Настройка ширины колонок
                SafeSetColumnWidth("Название альбома", 200);
                SafeSetColumnWidth("Исполнитель", 150);
                SafeSetColumnWidth("Год выпуска", 80);
                SafeSetColumnWidth("Жанр", 100);
                SafeSetColumnWidth("Лейбл", 120);
                SafeSetColumnWidth("Рейтинг", 70);
                SafeSetColumnWidth("Треков", 60);

                // Центрирование
                SafeSetColumnAlignment("Год выпуска", DataGridViewContentAlignment.MiddleCenter);
                SafeSetColumnAlignment("Рейтинг", DataGridViewContentAlignment.MiddleCenter);
                SafeSetColumnAlignment("Треков", DataGridViewContentAlignment.MiddleCenter);

                // Форматирование
                SafeSetColumnFormat("Рейтинг", "0.0");

                // Включаем сортировку для всех колонок
                EnableSortingForAllColumns();

                Debug.WriteLine("DataGridView успешно настроен");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка настройки DataGridView: {ex.Message}");
            }
            finally
            {
                // Возобновляем обновление
                if (dataGridViewAlbums != null && !dataGridViewAlbums.IsDisposed)
                {
                    dataGridViewAlbums.ResumeLayout();
                }
            }
        }

        private void SafeSetColumnVisible(string columnName, bool visible)
        {
            try
            {
                if (dataGridViewAlbums.Columns.Contains(columnName))
                {
                    var column = dataGridViewAlbums.Columns[columnName];
                    if (column != null)
                    {
                        column.Visible = visible;
                        Debug.WriteLine($"Колонка '{columnName}' видимость: {visible}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Колонка '{columnName}' не найдена для установки видимости");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при установке видимости колонки '{columnName}': {ex.Message}");
            }
        }

        private void SafeSetColumnWidth(string columnName, int width)
        {
            try
            {
                if (dataGridViewAlbums.Columns.Contains(columnName))
                {
                    var column = dataGridViewAlbums.Columns[columnName];
                    if (column != null)
                    {
                        column.Width = width;
                        Debug.WriteLine($"Колонка '{columnName}' ширина: {width}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Колонка '{columnName}' не найдена для установки ширины");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при установке ширины колонки '{columnName}': {ex.Message}");
            }
        }

        private void SafeSetColumnAlignment(string columnName, DataGridViewContentAlignment alignment)
        {
            try
            {
                if (dataGridViewAlbums.Columns.Contains(columnName))
                {
                    var column = dataGridViewAlbums.Columns[columnName];
                    if (column != null)
                    {
                        column.DefaultCellStyle.Alignment = alignment;
                        Debug.WriteLine($"Колонка '{columnName}' выравнивание: {alignment}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при установке выравнивания колонки '{columnName}': {ex.Message}");
            }
        }

        private void SafeSetColumnFormat(string columnName, string format)
        {
            try
            {
                if (dataGridViewAlbums.Columns.Contains(columnName))
                {
                    var column = dataGridViewAlbums.Columns[columnName];
                    if (column != null)
                    {
                        column.DefaultCellStyle.Format = format;
                        Debug.WriteLine($"Колонка '{columnName}' формат: {format}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при установке формата колонки '{columnName}': {ex.Message}");
            }
        }

        private void EnableSortingForAllColumns()
        {
            try
            {
                if (dataGridViewAlbums.Columns == null) return;

                foreach (DataGridViewColumn column in dataGridViewAlbums.Columns)
                {
                    try
                    {
                        if (column != null)
                        {
                            column.SortMode = DataGridViewColumnSortMode.Automatic;
                        }
                    }
                    catch (Exception innerEx)
                    {
                        Debug.WriteLine($"Ошибка при установке сортировки для колонки '{column?.Name}': {innerEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при включении сортировки: {ex.Message}");
            }
        }

        private void UpdateStats()
        {
            try
            {
                int totalAlbums = albumsTable?.Rows.Count ?? 0;
                lblStats.Text = $"Всего альбомов: {totalAlbums}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка обновления статистики: {ex.Message}");
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                AuthService.Logout();
                this.Close(); 
            }
        }

        private void btnAddAlbum_Click(object sender, EventArgs e)
        {
            var addForm = new FormAlbumDetails();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                isDataLoaded = false;
                LoadAlbums();
            }
        }

        private void btnEditAlbum_Click(object sender, EventArgs e)
        {
            if (dataGridViewAlbums.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите альбом для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int albumId = GetSelectedAlbumId();
            if (albumId > 0)
            {
                var editForm = new FormAlbumDetails(albumId);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    isDataLoaded = false;
                    LoadAlbums();
                }
            }
        }

        private void btnDeleteAlbum_Click(object sender, EventArgs e)
        {
            if (dataGridViewAlbums.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите альбом для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int albumId = GetSelectedAlbumId();
            if (albumId <= 0) return;

            string albumTitle = "";
            try
            {
                if (dataGridViewAlbums.Columns.Contains("Название альбома"))
                {
                    var cellValue = dataGridViewAlbums.SelectedRows[0].Cells["Название альбома"].Value;
                    albumTitle = cellValue?.ToString() ?? "Неизвестный альбом";
                }
            }
            catch
            {
                albumTitle = "Неизвестный альбом";
            }

            var result = MessageBox.Show($"Удалить альбом '{albumTitle}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DeleteAlbum(albumId);
            }
        }

        private int GetSelectedAlbumId()
        {
            if (dataGridViewAlbums.SelectedRows.Count == 0)
                return -1;

            try
            {
                if (dataGridViewAlbums.Columns.Contains("album_id"))
                {
                    var cellValue = dataGridViewAlbums.SelectedRows[0].Cells["album_id"].Value;
                    if (cellValue != null && cellValue != DBNull.Value)
                    {
                        return Convert.ToInt32(cellValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка получения ID альбома: {ex.Message}");
            }
            return -1;
        }

        private void DeleteAlbum(int albumId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Albums WHERE album_id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", albumId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Альбом удален", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isDataLoaded = false;
                        LoadAlbums();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (albumsTable != null)
                {
                    string searchText = txtSearch.Text.Trim();

                    if (string.IsNullOrEmpty(searchText))
                    {
                        albumsTable.DefaultView.RowFilter = "";
                    }
                    else
                    {
                        // Создаем фильтр для поиска по всем полям
                        StringBuilder filter = new StringBuilder();

                        // Убираем одинарные кавычки для безопасности
                        string safeSearchText = searchText.Replace("'", "''");

                        // Ищем по всем текстовым полям
                        string[] textColumns = {
                    "Название альбома",
                    "Исполнитель",
                    "Жанр",
                    "Лейбл",
                    "Список треков"
                };

                        foreach (string column in textColumns)
                        {
                            if (filter.Length > 0)
                                filter.Append(" OR ");

                            filter.Append($"[{column}] LIKE '%{safeSearchText}%'");
                        }

                        // Ищем по году выпуска (числовое поле)
                        if (int.TryParse(searchText, out int year))
                        {
                            if (filter.Length > 0)
                                filter.Append(" OR ");

                            filter.Append($"[Год выпуска] = {year}");
                        }

                        // Ищем по рейтингу (десятичное поле)
                        if (decimal.TryParse(searchText, out decimal rating))
                        {
                            if (filter.Length > 0)
                                filter.Append(" OR ");

                            filter.Append($"[Рейтинг] = {rating}");
                        }

                        // Ищем по количеству треков (целочисленное поле)
                        if (int.TryParse(searchText, out int tracks))
                        {
                            if (filter.Length > 0)
                                filter.Append(" OR ");

                            filter.Append($"[Треков] = {tracks}");
                        }

                        // Применяем фильтр
                        if (filter.Length > 0)
                        {
                            albumsTable.DefaultView.RowFilter = filter.ToString();
                        }
                        else
                        {
                            albumsTable.DefaultView.RowFilter = "";
                        }
                    }

                    UpdateStats();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка поиска: {ex.Message}");
                // В случае ошибки сбрасываем фильтр
                if (albumsTable != null)
                    albumsTable.DefaultView.RowFilter = "";
            }
        }
        


        private void dataGridViewAlbums_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewAlbums.SelectedRows.Count > 0)
                {
                    DisplayAlbumImage();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при изменении выделения: {ex.Message}");
            }
        }

        private void DisplayAlbumImage()
        {
            try
            {
                if (dataGridViewAlbums.SelectedRows.Count > 0 &&
                    dataGridViewAlbums.Columns.Contains("image_path"))
                {
                    var cellValue = dataGridViewAlbums.SelectedRows[0].Cells["image_path"].Value;
                    string imagePath = cellValue?.ToString();

                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        pictureBoxAlbum.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        pictureBoxAlbum.Image = CreateDefaultAlbumCover();
                    }
                }
                else
                {
                    pictureBoxAlbum.Image = CreateDefaultAlbumCover();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка отображения изображения: {ex.Message}");
                pictureBoxAlbum.Image = CreateDefaultAlbumCover();
            }
        }

        private Image CreateDefaultAlbumCover()
        {
            try
            {
                Bitmap bmp = new Bitmap(pictureBoxAlbum.Width, pictureBoxAlbum.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.LightGray);
                    g.DrawRectangle(Pens.DarkGray, 0, 0, bmp.Width - 1, bmp.Height - 1);

                    using (Font font = new Font("Arial", 12))
                    {
                        string text = "Нет обложки";
                        SizeF textSize = g.MeasureString(text, font);
                        float x = (bmp.Width - textSize.Width) / 2;
                        float y = (bmp.Height - textSize.Height) / 2;

                        g.DrawString(text, font, Brushes.Black, x, y);
                    }

                    using (Font iconFont = new Font("Arial", 24, FontStyle.Bold))
                    {
                        string note = "♪";
                        SizeF noteSize = g.MeasureString(note, iconFont);
                        float noteX = (bmp.Width - noteSize.Width) / 2;
                        float noteY = (bmp.Height - noteSize.Height) / 3;

                        g.DrawString(note, iconFont, Brushes.DarkBlue, noteX, noteY);
                    }
                }
                return bmp;
            }
            catch
            {
                // Возвращаем пустое изображение в случае ошибки
                return new Bitmap(pictureBoxAlbum.Width, pictureBoxAlbum.Height);
            }
        }

        private void pictureBoxAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewAlbums.SelectedRows.Count > 0)
                {
                    int albumId = GetSelectedAlbumId();
                    if (albumId > 0)
                    {
                        var detailsForm = new FormAlbumDetails(albumId, true);
                        detailsForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия деталей альбома: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            isDataLoaded = false;
            LoadAlbums();
            txtSearch.Clear();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Отписываемся от событий
            if (dataGridViewAlbums != null)
            {
                dataGridViewAlbums.DataBindingComplete -= DataGridViewAlbums_DataBindingComplete;
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Экспорт данных в Excel";
                saveFileDialog.FileName = $"MusicDirectory_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        // Устанавливаем лицензионный контекст для EPPlus (некоммерческое использование)
                        ExcelPackage.License.SetNonCommercialPersonal("FridayMIT");

                        using (var package = new ExcelPackage())
                        {
                            // Лист с альбомами
                            ExportAlbumsToExcel(package);

                            // Лист с треками
                            ExportTracksToExcel(package);

                            // Лист со статистикой
                            ExportStatisticsToExcel(package);

                            // Сохраняем файл
                            var fileInfo = new FileInfo(saveFileDialog.FileName);
                            package.SaveAs(fileInfo);
                        }

                        Cursor.Current = Cursors.Default;

                        var result = MessageBox.Show($"Данные успешно экспортированы в файл:\n{saveFileDialog.FileName}\n\nОткрыть файл?",
                            "Экспорт завершен", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(saveFileDialog.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show($"Ошибка при экспорте в Excel:\n{ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportAlbumsToExcel(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Альбомы");

            // Заголовки
            string[] headers = {
        "ID", "Название альбома", "Исполнитель", "Год выпуска",
        "Жанр", "Лейбл", "Рейтинг", "Количество треков",
        "Описание", "Путь к обложке"
    };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            // Стиль для заголовков
            using (var range = worksheet.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Получаем данные из БД
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = @"
            SELECT 
                a.album_id,
                a.album_title,
                ar.artist_name,
                a.release_year,
                g.genre_name,
                a.label,
                a.rating,
                a.total_tracks,
                a.description,
                a.image_path
            FROM Albums a
            LEFT JOIN Artists ar ON a.artist_id = ar.artist_id
            LEFT JOIN Genres g ON a.genre_id = g.genre_id
            ORDER BY a.album_title";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Заполняем данные
                int row = 2;
                foreach (DataRow dataRow in dt.Rows)
                {
                    worksheet.Cells[row, 1].Value = dataRow["album_id"];
                    worksheet.Cells[row, 2].Value = dataRow["album_title"];
                    worksheet.Cells[row, 3].Value = dataRow["artist_name"];
                    worksheet.Cells[row, 4].Value = dataRow["release_year"];
                    worksheet.Cells[row, 5].Value = dataRow["genre_name"];
                    worksheet.Cells[row, 6].Value = dataRow["label"];
                    worksheet.Cells[row, 7].Value = dataRow["rating"];
                    worksheet.Cells[row, 8].Value = dataRow["total_tracks"];
                    worksheet.Cells[row, 9].Value = dataRow["description"];
                    worksheet.Cells[row, 10].Value = dataRow["image_path"];
                    row++;
                }

                // Автоподбор ширины колонок
                for (int i = 1; i <= headers.Length; i++)
                {
                    worksheet.Column(i).AutoFit();
                }

                // Форматирование
                worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(7).Style.Numberformat.Format = "0.0";
                worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Добавляем фильтры
                worksheet.Cells[1, 1, 1, headers.Length].AutoFilter = true;
            }
        }

        private void ExportTracksToExcel(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Треки");

            // Заголовки
            string[] headers = {
        "ID альбома", "Альбом", "Исполнитель", "Год выпуска",
        "№ трека", "Название трека", "Длительность (сек)",
        "Длительность (мм:сс)"
    };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            // Стиль для заголовков
            using (var range = worksheet.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Получаем данные из БД
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = @"
            SELECT 
                a.album_id,
                a.album_title,
                ar.artist_name,
                a.release_year,
                t.track_number,
                t.track_title,
                t.duration
            FROM Tracks t
            INNER JOIN Albums a ON t.album_id = a.album_id
            LEFT JOIN Artists ar ON a.artist_id = ar.artist_id
            ORDER BY a.album_title, t.track_number";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Заполняем данные
                int row = 2;
                foreach (DataRow dataRow in dt.Rows)
                {
                    worksheet.Cells[row, 1].Value = dataRow["album_id"];
                    worksheet.Cells[row, 2].Value = dataRow["album_title"];
                    worksheet.Cells[row, 3].Value = dataRow["artist_name"];
                    worksheet.Cells[row, 4].Value = dataRow["release_year"];
                    worksheet.Cells[row, 5].Value = dataRow["track_number"];
                    worksheet.Cells[row, 6].Value = dataRow["track_title"];

                    // Длительность
                    if (dataRow["duration"] != DBNull.Value)
                    {
                        TimeSpan duration = (TimeSpan)dataRow["duration"];
                        worksheet.Cells[row, 7].Value = duration.TotalSeconds;
                        worksheet.Cells[row, 8].Value = duration.ToString(@"mm\:ss");
                    }

                    row++;
                }

                // Автоподбор ширины колонок
                for (int i = 1; i <= headers.Length; i++)
                {
                    worksheet.Column(i).AutoFit();
                }

                // Форматирование
                worksheet.Column(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(7).Style.Numberformat.Format = "0";
                worksheet.Column(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Column(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Добавляем фильтры
                worksheet.Cells[1, 1, 1, headers.Length].AutoFilter = true;
            }
        }

        private void ExportStatisticsToExcel(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Статистика");

            // Заголовок
            worksheet.Cells[1, 1].Value = "СТАТИСТИКА МУЗЫКАЛЬНОГО КАТАЛОГА";
            worksheet.Cells[1, 1, 1, 2].Merge = true;
            using (var titleCell = worksheet.Cells[1, 1])
            {
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.Size = 14;
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
            }

            int row = 3;

            // Основная статистика
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // Общее количество альбомов
                worksheet.Cells[row, 1].Value = "Всего альбомов:";
                worksheet.Cells[row, 2].Value = GetScalarValue(conn, "SELECT COUNT(*) FROM Albums");
                row++;

                // Общее количество треков
                worksheet.Cells[row, 1].Value = "Всего треков:";
                worksheet.Cells[row, 2].Value = GetScalarValue(conn, "SELECT COUNT(*) FROM Tracks");
                row++;

                // Количество исполнителей
                worksheet.Cells[row, 1].Value = "Уникальных исполнителей:";
                worksheet.Cells[row, 2].Value = GetScalarValue(conn, "SELECT COUNT(*) FROM Artists");
                row++;

                // Количество жанров
                worksheet.Cells[row, 1].Value = "Уникальных жанров:";
                worksheet.Cells[row, 2].Value = GetScalarValue(conn, "SELECT COUNT(*) FROM Genres");
                row++;

                row++; // Пустая строка

                // Средний рейтинг
                worksheet.Cells[row, 1].Value = "Средний рейтинг альбомов:";
                object avgRatingObj = GetScalarValue(conn, "SELECT AVG(rating) FROM Albums WHERE rating IS NOT NULL");
                if (avgRatingObj != DBNull.Value)
                {
                    decimal avgRating = Convert.ToDecimal(avgRatingObj);
                    worksheet.Cells[row, 2].Value = Math.Round(avgRating, 2);
                }
                row++;

                // Среднее количество треков в альбоме
                worksheet.Cells[row, 1].Value = "Среднее количество треков в альбоме:";
                object avgTracksObj = GetScalarValue(conn, "SELECT AVG(total_tracks) FROM Albums WHERE total_tracks IS NOT NULL");
                if (avgTracksObj != DBNull.Value)
                {
                    decimal avgTracks = Convert.ToDecimal(avgTracksObj);
                    worksheet.Cells[row, 2].Value = Math.Round(avgTracks, 1);
                }
                row++;

                row++; // Пустая строка

                // Альбом с самым высоким рейтингом
                worksheet.Cells[row, 1].Value = "Альбом с самым высоким рейтингом:";
                using (var cmd = new SqlCommand(@"
            SELECT TOP 1 a.album_title, ar.artist_name, a.rating 
            FROM Albums a 
            LEFT JOIN Artists ar ON a.artist_id = ar.artist_id 
            WHERE a.rating IS NOT NULL
            ORDER BY a.rating DESC", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string album = reader["album_title"].ToString();
                            string artist = reader["artist_name"].ToString();
                            decimal rating = Convert.ToDecimal(reader["rating"]);
                            worksheet.Cells[row, 2].Value = $"{album} ({artist}) - {rating:0.0}";
                        }
                    }
                }
                row++;

                // Самый старый альбом
                worksheet.Cells[row, 1].Value = "Самый старый альбом:";
                using (var cmd = new SqlCommand(@"
            SELECT TOP 1 a.album_title, ar.artist_name, a.release_year 
            FROM Albums a 
            LEFT JOIN Artists ar ON a.artist_id = ar.artist_id 
            WHERE a.release_year IS NOT NULL
            ORDER BY a.release_year ASC", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string album = reader["album_title"].ToString();
                            string artist = reader["artist_name"].ToString();
                            int year = Convert.ToInt32(reader["release_year"]);
                            worksheet.Cells[row, 2].Value = $"{album} ({artist}) - {year} г.";
                        }
                    }
                }

                // Автоподбор ширины колонок
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).AutoFit();

                // Добавляем дату экспорта
                worksheet.Cells[row + 2, 1].Value = $"Дата экспорта: {DateTime.Now:dd.MM.yyyy HH:mm}";
                worksheet.Cells[row + 2, 1].Style.Font.Italic = true;
                worksheet.Cells[row + 2, 1].Style.Font.Size = 10;
            }
        }

        private object GetScalarValue(SqlConnection conn, string query)
        {
            using (var cmd = new SqlCommand(query, conn))
            {
                return cmd.ExecuteScalar();
            }
        }
    }
}