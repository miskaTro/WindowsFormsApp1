using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Database database = new Database();
        private int selectedTaskId = -1; // Для хранения ID выбранной задачи

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            // Проверка на заполненность полей
            if (string.IsNullOrWhiteSpace(txtTaskNumber.Text) ||
                string.IsNullOrWhiteSpace(txtTaskName.Text) ||
                string.IsNullOrWhiteSpace(txtProjectName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                cmbPriority.SelectedIndex == -1 ||
                cmbStatus.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtAssignee.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Добавляем задачу в базу данных
                database.AddTask(
                    txtTaskNumber.Text,
                    txtTaskName.Text,
                    txtProjectName.Text,
                    txtDescription.Text,
                    cmbPriority.SelectedItem.ToString(),
                    cmbStatus.SelectedItem.ToString(),
                    txtAssignee.Text,
                    dtpCreationDate.Value
                );

                // Уведомление о успешном добавлении
                MessageBox.Show("Задача успешно добавлена в базу данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей после добавления задачи
                ClearFormFields();

                // Обновляем DataGridView с задачами
                RefreshTasks();
            }
            catch (Exception ex)
            {
                // Ошибка при добавлении задачи
                MessageBox.Show("Ошибка при добавлении задачи: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditTask_Click(object sender, EventArgs e)
        {
            // Проверка, если задача не выбрана
            if (selectedTaskId == -1)
            {
                MessageBox.Show("Пожалуйста, выберите задачу для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на заполненность полей
            if (string.IsNullOrWhiteSpace(txtTaskNumber.Text) ||
                string.IsNullOrWhiteSpace(txtTaskName.Text) ||
                string.IsNullOrWhiteSpace(txtProjectName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                cmbPriority.SelectedIndex == -1 ||
                cmbStatus.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtAssignee.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Обновляем задачу в базе данных
                database.UpdateTask(
                    selectedTaskId,
                    txtTaskNumber.Text,
                    txtTaskName.Text,
                    txtProjectName.Text,
                    txtDescription.Text,
                    cmbPriority.SelectedItem.ToString(),
                    cmbStatus.SelectedItem.ToString(),
                    txtAssignee.Text,
                    dtpCreationDate.Value
                );

                // Уведомление о успешном обновлении
                MessageBox.Show("Задача успешно обновлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей после редактирования
                ClearFormFields();

                // Обновляем DataGridView с задачами
                RefreshTasks();
            }
            catch (Exception ex)
            {
                // Ошибка при обновлении задачи
                MessageBox.Show("Ошибка при обновлении задачи: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            // Проверка, если задача не выбрана
            if (selectedTaskId == -1)
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Удаляем задачу из базы данных
                database.DeleteTask(selectedTaskId);

                // Уведомление о успешном удалении
                MessageBox.Show("Задача успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Очистка полей после удаления
                ClearFormFields();

                // Обновляем DataGridView с задачами
                RefreshTasks();
            }
            catch (Exception ex)
            {
                // Ошибка при удалении задачи
                MessageBox.Show("Ошибка при удалении задачи: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTasks_SelectionChanged(object sender, EventArgs e)
        {
            // Если выбрана строка в DataGridView
            if (dgvTasks.SelectedRows.Count > 0)
            {
                var row = dgvTasks.SelectedRows[0];
                selectedTaskId = Convert.ToInt32(row.Cells["TaskId"].Value); // Предполагаем, что столбец с TaskId есть в DataGridView

                // Заполняем форму данными выбранной задачи
                txtTaskNumber.Text = row.Cells["TaskNumber"].Value.ToString();
                txtTaskName.Text = row.Cells["TaskName"].Value.ToString();
                txtProjectName.Text = row.Cells["ProjectName"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value.ToString();
                cmbPriority.SelectedItem = row.Cells["Priority"].Value.ToString();
                cmbStatus.SelectedItem = row.Cells["Status"].Value.ToString();
                txtAssignee.Text = row.Cells["Assignee"].Value.ToString();
                dtpCreationDate.Value = Convert.ToDateTime(row.Cells["CreationDate"].Value);
            }
        }

        private void RefreshTasks()
        {
            try
            {
                // Получаем все задачи из базы данных
                DataTable tasks = database.GetAllTasks();

                // Отображаем задачи в DataGridView
                dgvTasks.DataSource = tasks;
            }
            catch (Exception ex)
            {
                // Ошибка при получении задач
                MessageBox.Show("Ошибка при получении задач: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormFields()
        {
            txtTaskNumber.Clear();
            txtTaskName.Clear();
            txtProjectName.Clear();
            txtDescription.Clear();
            cmbPriority.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            txtAssignee.Clear();
            dtpCreationDate.Value = DateTime.Now;

            selectedTaskId = -1; // Сбрасываем ID выбранной задачи
        }
    }
}
