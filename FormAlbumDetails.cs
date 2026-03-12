using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MusicDirectoryApp.Data;

namespace MusicDirectoryApp.Forms
{
    public partial class FormAlbumDetails : Form
    {
        private int albumId = -1;
        private bool isEditMode = false;
        private bool isReadOnly = false;
        private string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MusicDirectory;Integrated Security=True";
        private DataTable tracksTable;

        // Конструктор для добавления нового альбома
        public FormAlbumDetails()
        {
            InitializeComponent();
            isEditMode = false;
            InitializeTracksTable();
            LoadAutoCompleteData();
            this.Text = "Добавление альбома";
            SetupTrackGrid();
        }

        // Конструктор для редактирования существующего альбома
        public FormAlbumDetails(int albumId)
        {
            InitializeComponent();
            this.albumId = albumId;
            isEditMode = true;
            isReadOnly = false;
            InitializeTracksTable();
            LoadAutoCompleteData();
            LoadAlbumData();
            this.Text = "Редактирование альбома";
            SetupTrackGrid();
        }

        // Конструктор для просмотра (только чтение)
        public FormAlbumDetails(int albumId, bool readOnly)
        {
            InitializeComponent();
            this.albumId = albumId;
            isEditMode = true;
            isReadOnly = readOnly;
            InitializeTracksTable();
            LoadAutoCompleteData();
            LoadAlbumData();
            SetReadOnlyMode();
            this.Text = "Просмотр альбома";
            SetupTrackGrid();
        }

        private void LoadAutoCompleteData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    // Загрузка исполнителей для автодополнения
                    string artistsQuery = "SELECT artist_name FROM Artists ORDER BY artist_name";
                    SqlDataAdapter artistsAdapter = new SqlDataAdapter(artistsQuery, conn);
                    DataTable artistsTable = new DataTable();
                    artistsAdapter.Fill(artistsTable);

                    // Настройка автодополнения для поля исполнителя
                    var artistSource = new AutoCompleteStringCollection();
                    foreach (DataRow row in artistsTable.Rows)
                    {
                        artistSource.Add(row["artist_name"].ToString());
                    }
                    txtArtist.AutoCompleteCustomSource = artistSource;
                    txtArtist.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txtArtist.AutoCompleteSource = AutoCompleteSource.CustomSource;

                    // Загрузка жанров для автодополнения
                    string genresQuery = "SELECT genre_name FROM Genres ORDER BY genre_name";
                    SqlDataAdapter genresAdapter = new SqlDataAdapter(genresQuery, conn);
                    DataTable genresTable = new DataTable();
                    genresAdapter.Fill(genresTable);

                    // Настройка автодополнения для поля жанра
                    var genreSource = new AutoCompleteStringCollection();
                    foreach (DataRow row in genresTable.Rows)
                    {
                        genreSource.Add(row["genre_name"].ToString());
                    }
                    txtGenre.AutoCompleteCustomSource = genreSource;
                    txtGenre.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txtGenre.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeTracksTable()
        {
            tracksTable = new DataTable();
            tracksTable.Columns.Add("track_id", typeof(int));
            tracksTable.Columns.Add("track_number", typeof(int));
            tracksTable.Columns.Add("track_title", typeof(string));
            tracksTable.Columns.Add("duration", typeof(string)); // В формате "mm:ss"

            dataGridViewTracks.DataSource = tracksTable;
        }

        private void SetupTrackGrid()
        {
            if (dataGridViewTracks.Columns.Count > 0)
            {
                // Скрываем техническую колонку
                dataGridViewTracks.Columns["track_id"].Visible = false;

                // Настраиваем отображаемые колонки
                dataGridViewTracks.Columns["track_number"].HeaderText = "№";
                dataGridViewTracks.Columns["track_number"].Width = 40;
                dataGridViewTracks.Columns["track_number"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dataGridViewTracks.Columns["track_number"].ReadOnly = true; // Номер трека нельзя редактировать

                dataGridViewTracks.Columns["track_title"].HeaderText = "Название трека";
                dataGridViewTracks.Columns["track_title"].Width = 250;

                dataGridViewTracks.Columns["duration"].HeaderText = "Длительность (мм:сс)";
                dataGridViewTracks.Columns["duration"].Width = 100;
                dataGridViewTracks.Columns["duration"].DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;

                // Настраиваем DataGridView для редактирования
                if (!isReadOnly)
                {
                    dataGridViewTracks.ReadOnly = false;
                    dataGridViewTracks.AllowUserToAddRows = false; // Запрещаем добавление строк через DataGridView
                    dataGridViewTracks.EditMode = DataGridViewEditMode.EditOnEnter; // Редактирование при входе в ячейку
                }
                else
                {
                    dataGridViewTracks.ReadOnly = true;
                    btnAddTrack.Enabled = false;
                    btnEditTrack.Enabled = false;
                    btnDeleteTrack.Enabled = false;
                }
            }
        }

        private void LoadAlbumData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string query = @"
                        SELECT 
                            a.*,
                            ar.artist_name,
                            g.genre_name
                        FROM Albums a
                        LEFT JOIN Artists ar ON a.artist_id = ar.artist_id
                        LEFT JOIN Genres g ON a.genre_id = g.genre_id
                        WHERE a.album_id = @albumId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@albumId", albumId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        txtTitle.Text = row["album_title"].ToString();
                        txtArtist.Text = row["artist_name"].ToString();
                        numYear.Value = row["release_year"] != DBNull.Value ? Convert.ToInt32(row["release_year"]) : DateTime.Now.Year;
                        txtGenre.Text = row["genre_name"].ToString();
                        txtLabel.Text = row["label"].ToString();
                        txtDescription.Text = row["description"].ToString();
                        numRating.Value = row["rating"] != DBNull.Value ? Convert.ToDecimal(row["rating"]) : 0;
                        numTracks.Value = row["total_tracks"] != DBNull.Value ? Convert.ToInt32(row["total_tracks"]) : 0;

                        // Загрузка изображения
                        string imagePath = row["image_path"]?.ToString();
                        if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                        {
                            pictureBoxAlbum.Image = Image.FromFile(imagePath);
                            lblImagePath.Text = imagePath;
                        }

                        // Загрузка треков
                        LoadTracks();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных альбома: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTracks()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT track_id, track_number, track_title, duration FROM Tracks WHERE album_id = @albumId ORDER BY track_number";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@albumId", albumId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Очищаем таблицу треков
                    tracksTable.Clear();

                    // Добавляем треки в таблицу
                    foreach (DataRow row in dt.Rows)
                    {
                        string duration = "00:00";
                        if (row["duration"] != DBNull.Value)
                        {
                            TimeSpan ts = (TimeSpan)row["duration"];
                            duration = ts.ToString(@"mm\:ss");
                        }

                        tracksTable.Rows.Add(
                            row["track_id"],
                            row["track_number"],
                            row["track_title"].ToString(),
                            duration
                        );
                    }

                    // Обновляем количество треков в форме
                    numTracks.Value = tracksTable.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки треков: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetReadOnlyMode()
        {
            txtTitle.ReadOnly = true;
            txtArtist.ReadOnly = true;
            numYear.Enabled = false;
            txtGenre.ReadOnly = true;
            txtLabel.ReadOnly = true;
            txtDescription.ReadOnly = true;
            numRating.Enabled = false;
            numTracks.Enabled = false;
            btnSelectImage.Enabled = false;
            btnSave.Visible = false;
            btnCancel.Text = "Закрыть";

            // Делаем DataGridView только для чтения
            dataGridViewTracks.ReadOnly = true;
            btnAddTrack.Enabled = false;
            btnEditTrack.Enabled = false;
            btnDeleteTrack.Enabled = false;

            // Меняем цвет фона
            txtTitle.BackColor = SystemColors.Control;
            txtArtist.BackColor = SystemColors.Control;
            txtGenre.BackColor = SystemColors.Control;
            txtLabel.BackColor = SystemColors.Control;
            txtDescription.BackColor = SystemColors.Control;
        }

        // Методы для работы с исполнителями и жанрами
        private int GetOrCreateArtist(string artistName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Сначала ищем существующего исполнителя
                    string findQuery = "SELECT artist_id FROM Artists WHERE artist_name = @name";
                    SqlCommand findCmd = new SqlCommand(findQuery, conn);
                    findCmd.Parameters.AddWithValue("@name", artistName);
                    object result = findCmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }

                    // Создаем нового исполнителя
                    string insertQuery = "INSERT INTO Artists (artist_name) VALUES (@name); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@name", artistName);

                    return Convert.ToInt32(insertCmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с исполнителем: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private int GetOrCreateGenre(string genreName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Сначала ищем существующий жанр
                    string findQuery = "SELECT genre_id FROM Genres WHERE genre_name = @name";
                    SqlCommand findCmd = new SqlCommand(findQuery, conn);
                    findCmd.Parameters.AddWithValue("@name", genreName);
                    object result = findCmd.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }

                    // Создаем новый жанр
                    string insertQuery = "INSERT INTO Genres (genre_name) VALUES (@name); SELECT SCOPE_IDENTITY();";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@name", genreName);

                    return Convert.ToInt32(insertCmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с жанром: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Выберите обложку альбома";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBoxAlbum.Image = Image.FromFile(openFileDialog.FileName);
                    lblImagePath.Text = openFileDialog.FileName;
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Кнопки для работы с треками
        private void btnAddTrack_Click(object sender, EventArgs e)
        {
            // Добавляем новый трек
            int nextTrackNumber = 1;
            if (tracksTable.Rows.Count > 0)
            {
                // Находим максимальный номер трека
                int maxNumber = 0;
                foreach (DataRow row in tracksTable.Rows)
                {
                    int currentNumber = Convert.ToInt32(row["track_number"]);
                    if (currentNumber > maxNumber)
                    {
                        maxNumber = currentNumber;
                    }
                }
                nextTrackNumber = maxNumber + 1;
            }

            tracksTable.Rows.Add(
                -1, // ID для новых треков
                nextTrackNumber,
                "Новый трек",
                "00:00"
            );

            // Обновляем количество треков
            numTracks.Value = tracksTable.Rows.Count;

            // Переходим к редактированию новой строки
            if (dataGridViewTracks.Rows.Count > 0)
            {
                dataGridViewTracks.CurrentCell = dataGridViewTracks.Rows[dataGridViewTracks.Rows.Count - 1].Cells["track_title"];
                dataGridViewTracks.BeginEdit(true);
            }
        }

        private void btnEditTrack_Click(object sender, EventArgs e)
        {
            if (dataGridViewTracks.SelectedRows.Count > 0)
            {
                // Начинаем редактирование выбранной строки
                dataGridViewTracks.CurrentCell = dataGridViewTracks.SelectedRows[0].Cells["track_title"];
                dataGridViewTracks.BeginEdit(true);
            }
            else
            {
                MessageBox.Show("Выберите трек для редактирования", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDeleteTrack_Click(object sender, EventArgs e)
        {
            if (dataGridViewTracks.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Удалить выбранный трек?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Удаляем выбранные строки
                    foreach (DataGridViewRow row in dataGridViewTracks.SelectedRows)
                    {
                        dataGridViewTracks.Rows.Remove(row);
                    }

                    // Перенумеровываем треки
                    RenumberTracks();

                    // Обновляем количество треков
                    numTracks.Value = tracksTable.Rows.Count;
                }
            }
            else
            {
                MessageBox.Show("Выберите трек для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RenumberTracks()
        {
            int trackNumber = 1;
            foreach (DataRow row in tracksTable.Rows)
            {
                row["track_number"] = trackNumber++;
            }
            dataGridViewTracks.Refresh();
        }

        private void dataGridViewTracks_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Валидация данных в DataGridView
            if (e.ColumnIndex == dataGridViewTracks.Columns["duration"].Index)
            {
                string value = e.FormattedValue.ToString();
                if (!IsValidDuration(value))
                {
                    dataGridViewTracks.Rows[e.RowIndex].ErrorText = "Формат длительности должен быть мм:сс";
                    e.Cancel = true;
                }
                else
                {
                    dataGridViewTracks.Rows[e.RowIndex].ErrorText = null;
                }
            }
        }

        private bool IsValidDuration(string duration)
        {
            // Проверка формата мм:сс
            if (string.IsNullOrEmpty(duration))
                return false;

            string[] parts = duration.Split(':');
            if (parts.Length != 2)
                return false;

            if (!int.TryParse(parts[0], out int minutes) || !int.TryParse(parts[1], out int seconds))
                return false;

            return minutes >= 0 && seconds >= 0 && seconds < 60;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Валидация основных полей
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название альбома", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArtist.Text))
            {
                MessageBox.Show("Введите исполнителя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtArtist.Focus();
                return;
            }

            // Валидация треков
            foreach (DataRow row in tracksTable.Rows)
            {
                if (string.IsNullOrWhiteSpace(row["track_title"].ToString()))
                {
                    MessageBox.Show("Все треки должны иметь название", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!IsValidDuration(row["duration"].ToString()))
                {
                    MessageBox.Show($"Неправильный формат длительности в треке {row["track_number"]}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                // Получаем или создаем исполнителя и жанр
                int artistId = GetOrCreateArtist(txtArtist.Text.Trim());
                int genreId = GetOrCreateGenre(txtGenre.Text.Trim());

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    if (isEditMode)
                    {
                        // Обновление существующего альбома
                        UpdateAlbum(conn, artistId, genreId);
                    }
                    else
                    {
                        // Добавление нового альбома
                        albumId = InsertAlbum(conn, artistId, genreId);
                    }

                    // Сохраняем треки
                    SaveTracks(conn);

                    MessageBox.Show("Данные успешно сохранены", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int InsertAlbum(SqlConnection conn, int artistId, int genreId)
        {
            string query = @"
                INSERT INTO Albums (
                    album_title, artist_id, release_year, genre_id, 
                    label, description, image_path, rating, total_tracks
                ) VALUES (
                    @title, @artistId, @year, @genreId, 
                    @label, @description, @imagePath, @rating, @tracks
                );
                SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            cmd.Parameters.AddWithValue("@artistId", artistId);
            cmd.Parameters.AddWithValue("@year", (int)numYear.Value);
            cmd.Parameters.AddWithValue("@genreId", genreId);
            cmd.Parameters.AddWithValue("@label", string.IsNullOrWhiteSpace(txtLabel.Text) ? DBNull.Value : (object)txtLabel.Text.Trim());
            cmd.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(txtDescription.Text) ? DBNull.Value : (object)txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@imagePath", string.IsNullOrWhiteSpace(lblImagePath.Text) ? DBNull.Value : (object)lblImagePath.Text);
            cmd.Parameters.AddWithValue("@rating", numRating.Value);
            cmd.Parameters.AddWithValue("@tracks", (int)numTracks.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void UpdateAlbum(SqlConnection conn, int artistId, int genreId)
        {
            string query = @"
                UPDATE Albums SET
                    album_title = @title,
                    artist_id = @artistId,
                    release_year = @year,
                    genre_id = @genreId,
                    label = @label,
                    description = @description,
                    image_path = @imagePath,
                    rating = @rating,
                    total_tracks = @tracks
                WHERE album_id = @albumId";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
            cmd.Parameters.AddWithValue("@artistId", artistId);
            cmd.Parameters.AddWithValue("@year", (int)numYear.Value);
            cmd.Parameters.AddWithValue("@genreId", genreId);
            cmd.Parameters.AddWithValue("@label", string.IsNullOrWhiteSpace(txtLabel.Text) ? DBNull.Value : (object)txtLabel.Text.Trim());
            cmd.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(txtDescription.Text) ? DBNull.Value : (object)txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@imagePath", string.IsNullOrWhiteSpace(lblImagePath.Text) ? DBNull.Value : (object)lblImagePath.Text);
            cmd.Parameters.AddWithValue("@rating", numRating.Value);
            cmd.Parameters.AddWithValue("@tracks", (int)numTracks.Value);
            cmd.Parameters.AddWithValue("@albumId", albumId);

            cmd.ExecuteNonQuery();
        }

        private void SaveTracks(SqlConnection conn)
        {
            // Удаляем старые треки (при редактировании)
            if (isEditMode)
            {
                string deleteQuery = "DELETE FROM Tracks WHERE album_id = @albumId";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@albumId", albumId);
                deleteCmd.ExecuteNonQuery();
            }

            // Сохраняем новые треки
            foreach (DataRow row in tracksTable.Rows)
            {
                // Преобразуем строку длительности в TimeSpan
                string durationStr = row["duration"].ToString();
                TimeSpan duration = TimeSpan.Zero;
                if (!string.IsNullOrEmpty(durationStr))
                {
                    string[] parts = durationStr.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int minutes) && int.TryParse(parts[1], out int seconds))
                    {
                        duration = new TimeSpan(0, minutes, seconds);
                    }
                }

                string insertQuery = @"
                    INSERT INTO Tracks (album_id, track_number, track_title, duration)
                    VALUES (@albumId, @trackNumber, @title, @duration)";

                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@albumId", albumId);
                insertCmd.Parameters.AddWithValue("@trackNumber", Convert.ToInt32(row["track_number"]));
                insertCmd.Parameters.AddWithValue("@title", row["track_title"].ToString());
                insertCmd.Parameters.AddWithValue("@duration", duration);

                insertCmd.ExecuteNonQuery();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewTracks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // При двойном клике начинаем редактирование
            if (e.RowIndex >= 0 && !isReadOnly)
            {
                dataGridViewTracks.BeginEdit(true);
            }
        }

        private void dataGridViewTracks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // После редактирования проверяем валидность
            if (e.ColumnIndex == dataGridViewTracks.Columns["track_title"].Index)
            {
                string value = dataGridViewTracks.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    dataGridViewTracks.Rows[e.RowIndex].ErrorText = "Название трека не может быть пустым";
                }
                else
                {
                    dataGridViewTracks.Rows[e.RowIndex].ErrorText = null;
                }
            }
        }

        private void dataGridViewTracks_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Добавляем обработчик события для валидации ввода при редактировании
            if (dataGridViewTracks.CurrentCell.ColumnIndex == dataGridViewTracks.Columns["duration"].Index)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress -= DurationTextBox_KeyPress;
                    textBox.KeyPress += DurationTextBox_KeyPress;
                }
            }
        }

        private void DurationTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Валидация ввода для поля длительности
            if (dataGridViewTracks.CurrentCell.ColumnIndex == dataGridViewTracks.Columns["duration"].Index)
            {
                TextBox textBox = sender as TextBox;
                string currentText = textBox.Text;

                // Разрешаем цифры и двоеточие
                if (char.IsDigit(e.KeyChar) || e.KeyChar == ':')
                {
                    // Проверяем, что двоеточие может быть только одно
                    if (e.KeyChar == ':' && currentText.Contains(":"))
                    {
                        e.Handled = true;
                    }
                    // Ограничиваем длину ввода (максимум 5 символов: mm:ss)
                    else if (currentText.Length >= 5 && e.KeyChar != '\b')
                    {
                        e.Handled = true;
                    }
                }
                else if (!char.IsControl(e.KeyChar)) // Разрешаем управляющие символы (backspace, delete)
                {
                    e.Handled = true;
                }
            }
        }
    }
}