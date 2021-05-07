using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class FormFields : Form
    {
        private List<PictureBox> pictureBoxes;
        private Form1 form1;
        public Shoes shoes;

        public FormFields(Form1 form)
        {
            InitializeComponent();

            this.form1 = form;
            pictureBoxes = new List<PictureBox>();
        }

        private void FormFields_Load(object sender, EventArgs e)
        {
            shoes = form1.currentShoes;

            pictureBoxes.Add(pictureBox1);
            pictureBoxes.Add(pictureBox2);
            pictureBoxes.Add(pictureBox3);
            pictureBoxes.Add(pictureBox4);
            pictureBoxes.Add(pictureBox5);
            pictureBoxes.Add(pictureBox6);
            pictureBoxes.Add(pictureBox7);
            pictureBoxes.Add(pictureBox8);

            ShoesService shoesService = form1.shoesService;

            form1.loadListToComboBox(shoesService.getDances(), comboBox_dance);
            form1.loadListToComboBox(shoesService.getManufacturers(), comboBox_manufacturer);
            form1.loadListToComboBox(shoesService.getColors(), comboBox_color);
            form1.loadListToComboBox(shoesService.getMatherials(), comboBox_matherial);

            FillForm(shoes);
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            string old_image_path = ((PictureBox)sender).ImageLocation;
            
            openFileDialog1.InitialDirectory = ((PictureBox)sender).ImageLocation;
            openFileDialog1.FileName = old_image_path;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string new_image_path = form1.shoesService.saveImage(shoes.Id, openFileDialog1.FileName);
                int index = shoes.Pictures.IndexOf(old_image_path);

                // перевірка чи оновити шлях до файлу чи додати нове зображення
                if (index != -1)
                {
                    // оновлюємо
                    shoes.Pictures.Remove(old_image_path);
                    shoes.Pictures.Insert(index, new_image_path);
                }
                else
                {
                    // додаємо
                    shoes.Pictures.Add(new_image_path);
                }
            }
            else // дію скасовано - видаляємо зображення
            {
                shoes.Pictures.Remove(((PictureBox)sender).ImageLocation);
            }

            DisplayImages();
        }

        private void FillForm(Shoes shoes)
        {
            textBox_name.Text = shoes.Name;
            comboBox_dance.Text = shoes.Dance;
            comboBox_manufacturer.Text = shoes.Manufacturer;
            comboBox_color.Text = shoes.Color;
            comboBox_matherial.Text = shoes.Matherial;

            numericUpDown_size.Value = shoes.Size;
            numericUpDown_height.Value = shoes.HeelHeight;

            SelectGender(shoes.Gender);

            DisplayImages();
        }

        private void DisplayImages()
        {
            List<string> pictures = shoes.Pictures;

            // ховаємо всі
            foreach(PictureBox box in pictureBoxes)
            {
                box.Visible = false;
                box.ImageLocation = "";
            }

            // показуємо необхідну кількість
            int i;
            for (i=0; i< pictures.Count; i++)
            {
                pictureBoxes[i].Visible = true;
                pictureBoxes[i].ImageLocation = pictures[i];
            }

            // показати якщо ще можлдиво ще одне зображення для додавання
            if(i < pictureBoxes.Count)
            {
                pictureBoxes[i].Visible = true;
            }
        }

        private void SelectGender(Shoes.GenderEnum gender)
        {
            if (Shoes.GenderEnum.BOYS == gender)
            {
                radioButton_boys.Checked = true;
            }
            else if (Shoes.GenderEnum.MAN == gender)
            {
                radioButton_man.Checked = true;
            }
            else if (Shoes.GenderEnum.WOMEN == gender)
            {
                radioButton_women.Checked = true;
            }
            else if (Shoes.GenderEnum.GIRLS == gender)
            {
                radioButton_girls.Checked = true;
            }
        }

        private Shoes.GenderEnum GetSelectedGender()
        {
            if (radioButton_boys.Checked)
            {
                return Shoes.GenderEnum.BOYS;
            }
            else if (radioButton_man.Checked)
            {
                return Shoes.GenderEnum.MAN;
            }
            else if (radioButton_women.Checked)
            {
                return Shoes.GenderEnum.WOMEN;
            }
            else if (radioButton_girls.Checked)
            {
                return Shoes.GenderEnum.GIRLS;
            }
            else
            {
                return Shoes.GenderEnum.BOYS;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            form1.shoesService.delete(shoes.Id);
            shoes = new Shoes();
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            shoes.Name = textBox_name.Text.Trim();
            shoes.Dance = comboBox_dance.Text.Trim();
            shoes.Manufacturer = comboBox_manufacturer.Text.Trim();
            shoes.Color = comboBox_color.Text.Trim();
            shoes.Matherial = comboBox_matherial.Text.Trim();

            shoes.Gender = GetSelectedGender();

            shoes.Size = (int)numericUpDown_size.Value;
            shoes.HeelHeight = (int)numericUpDown_height.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
