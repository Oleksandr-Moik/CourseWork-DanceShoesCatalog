using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class Form1 : Form
    {
        private string[] dances = { "Латина", "Контемпорарі", "Pole Dance", "Джайв", "Гімнастика", "Латина", "Сучасні", "Народні" };
        private string[] manufacturers = { "CLUBDANCE", "GRISHKO", "LEVANT", "MASTE", "MATITA" };
        private string[] names = { "Балетки, чешки", "Напівчашечки. Взуття для контемпу", "Пуанти", "Туфлі:", "Туфлі: Жіноча латина", "Туфлі: Жіночий стандарт ", "Туфлі: Взуття для народних танців ", "Туфлі: Чоловічий стандарт, Чоловіча латина", "Туфлі: Дитяче танцювальне взуття", "Кросівки для танців", "Джазовки", "Кизомба", "Танго" };
        private string[] matherials = { "Біла шкіра ", "Бежева шкіра ", "Бежевий лак", "Замша ", "Золото ", "Кірза ", "Коричнева шкіра ", "Різнокольорові ", "Червона замша ", "Червона шкіра ", "Чорний лак ", "Срібло ", "Тілесний", "Червоний сатин" };
        private string[] colors = { "Біла шкіра ", "Бежева шкіра ", "Бежевий лак", "Замша ", "Золото ", "Кірза ", "Коричнева шкіра ", "Різнокольорові ", "Червона замша ", "Червона шкіра ", "Чорний лак ", "Срібло ", "Тілесний", "Червоний сатин" };

        private enum Mode { edit, create };
        private List<PictureBox> pictureBoxes;

        public ShoesService shoesService;
        public Shoes currentShoes;

        public Form1()
        {
            InitializeComponent();

            shoesService = new ShoesService();
            currentShoes = new Shoes();
            pictureBoxes = new List<PictureBox>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBoxes.Add(pictureBox1);
            pictureBoxes.Add(pictureBox2);
            pictureBoxes.Add(pictureBox3);
            pictureBoxes.Add(pictureBox4);
            pictureBoxes.Add(pictureBox5);
            pictureBoxes.Add(pictureBox6);
            pictureBoxes.Add(pictureBox7);
            pictureBoxes.Add(pictureBox8);

            try
            {
                shoesService.LoadListsFromFiles();

                //shoesService.setList(new List<Shoes>());
                //GenerateData(4);
                //shoesService.SaveListToFile();

                DisplayListFromService();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        public void loadListToComboBox(string[] list, ComboBox combo, bool append = false)
        {
            if(!append)combo.Items.Clear();
            foreach (string item in list)
            {
                if (combo.Items.Contains(item))
                    continue;
                combo.Items.Add(item);
            }
        }
        public void loadListToComboBox(List<string> list, ComboBox combo, bool append = false)
        {
            if(!append)combo.Items.Clear();
            combo.Items.Clear();
            foreach (string item in list)
            {
                if (combo.Items.Contains(item)) 
                    continue;
                combo.Items.Add(item);
            }
        }

        private void GenerateData(int size)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            // отримати масив із перелічення
            Array genders = Enum.GetValues(typeof(Shoes.GenderEnum));

            for (int i = 0; i < size; i++)
            {
                Shoes shoes = new Shoes();

                shoes.Name = names[random.Next(0, names.Length)];
                shoes.Manufacturer = manufacturers[random.Next(0, manufacturers.Length)];
                shoes.Dance = dances[random.Next(0, dances.Length)];
                shoes.Color = colors[random.Next(0, colors.Length)]; ;
                shoes.Matherial = matherials[random.Next(0, matherials.Length)]; ;
                shoes.Size = random.Next(5, 40);
                shoes.HeelHeight = random.Next(0,9);
                shoes.Gender = (Shoes.GenderEnum)genders.GetValue(random.Next(0, genders.Length));

                shoesService.add(shoes);
            }
        }

        public void SelectGender(Shoes.GenderEnum gender)
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
        
        public void FillForm(Shoes shoes)
        {
            groupBox_form.Visible = false;
            if (shoes == null) return;

            textBox_name.Text = shoes.Name;
            comboBox_dance.Text = shoes.Dance;
            comboBox_manufacturer.Text = shoes.Manufacturer;
            comboBox_color.Text = shoes.Color;
            comboBox_matherial.Text = shoes.Matherial;

            numericUpDown_size.Value = shoes.Size;
            numericUpDown_height.Value = shoes.HeelHeight;

            SelectGender(shoes.Gender);

            DisplayImages();

            groupBox_form.Visible = true;
            groupBox_pidbir.Visible = false;
        }
        private void DisplayImages()
        {
            List<string> pictures = currentShoes.Pictures;

            foreach (PictureBox box in pictureBoxes)
            {
                box.Visible = false;
                box.ImageLocation = "";
            }

            for (int i = 0; i < pictures.Count; i++)
            {
                pictureBoxes[i].Visible = true;
                pictureBoxes[i].ImageLocation = pictures[i];
            }
        }
        private void picturebox_openImage(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(((PictureBox)sender).ImageLocation);
            }
            catch (Exception exc) //Win32Exception
            {
                MessageBox.Show("Перевірьте чи зображення не видалили на диску " +
                    "або замініть дане зображення на інше", "Зображення не можливо найти");
            }
        }

        private void DisplayListFromService()
        {
            DisplayList(shoesService.getList());
        }
        private void DisplayList(List<Shoes> list)
        {
            listBox_shoeses.Items.Clear();
            int index = 0;
            foreach (Shoes shoes in list)
            {
                listBox_shoeses.Items.Add(shoes);

                if (shoes.Id == currentShoes.Id)
                {
                    index = listBox_shoeses.Items.Count - 1;
                }
            }
            if(listBox_shoeses.Items.Count > 0)
            {
                listBox_shoeses.SelectedIndex = index;
            }

            label_shoeses_in_list.Text =
                listBox_shoeses.Items.Count.ToString()
                + " з "
                + shoesService.getList().Count();
        }
        private void listBox_shoeses_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentShoes = (Shoes)listBox_shoeses.SelectedItem;

            FillForm(currentShoes);
        }

        #region Open new edit window
        private void btn_edit_Click(object sender, EventArgs e)
        {
            ShowForm_Window(Mode.edit);
        }

        private void btn_add_item_Click(object sender, EventArgs e)
        {
            ShowForm_Window(Mode.create);
        }

        private void ShowForm_Window(Mode mode)
        {
            if (mode == Mode.create)
            {
                currentShoes = shoesService.add(new Shoes());
            }
            
            FormFields form = new FormFields(this);

            if (form.ShowDialog() == DialogResult.OK)
            {
                currentShoes = form.shoes;
                shoesService.update(currentShoes);
            }
            else if(mode == Mode.create)
            {
                shoesService.delete(currentShoes.Id);
            }

            form.Dispose();

            FillForm(currentShoes);
            DisplayListFromService();
        }
        #endregion

        private void btn_reload_Click(object sender, EventArgs e)
        {
            DisplayListFromService();
        }

        #region Pidbir methods
        private void btn_pidibrat_Click(object sender, EventArgs e)
        {
            groupBox_pidbir.Visible = false;

            Shoes shoes = new Shoes();

            if (radioButton_old.Checked)
            {
                shoes.Gender = radioButton_gender_man.Checked
                    ? Shoes.GenderEnum.MAN
                    : Shoes.GenderEnum.WOMEN;
            }
            else
            {
                shoes.Gender = radioButton_gender_man.Checked
                    ? Shoes.GenderEnum.BOYS
                    : Shoes.GenderEnum.GIRLS;
            }

            shoes.Dance = comboBox_dance_pidbir.Text.ToLower().Trim();
            shoes.Manufacturer = comboBox_manufacturer_pidbir.Text.ToLower().Trim();
            shoes.Color = comboBox_color_pidbir.Text.ToLower().Trim();
            shoes.Matherial = comboBox_matherial_pidbir.Text.ToLower().Trim();

            shoes.Size = (int)numericUpDown_size.Value;

            List<Shoes> shoeses = new List<Shoes>();
            foreach(Shoes sh in shoesService.getList())
            {
                // перевірки на вміст та відповідність
                // якщо не співпадає - пропускаємо та переходимо до наступного елементу
                if ( shoes.Gender != sh.Gender) { continue; }

                if (!(shoes.Dance.Contains(sh.Dance.ToLower())
                    || sh.Dance.ToLower().Contains(shoes.Dance))) { continue; }
                
                if (!(shoes.Manufacturer.Contains(sh.Manufacturer.ToLower())
                    || sh.Manufacturer.ToLower().Contains(shoes.Manufacturer))) { continue; }
              
                if (!(shoes.Color.Contains(sh.Color.ToLower())
                    || sh.Color.ToLower().Contains(shoes.Color))) { }
                
                if (!(shoes.Matherial.Contains(sh.Matherial.ToLower())
                    || sh.Matherial.ToLower().Contains(shoes.Matherial))) { }

                if (Math.Abs(sh.Size - shoes.Size) > 2) { continue; }

                // пройшов фільтри отже додаємо
                shoeses.Add(sh);
            }

            DisplayList(shoeses);
        }

        private void btn_pidbir_Click(object sender, EventArgs e)
        {
            loadListToComboBox(dances, comboBox_dance_pidbir);
            loadListToComboBox(colors, comboBox_color_pidbir);
            loadListToComboBox(matherials, comboBox_matherial_pidbir);
            loadListToComboBox(manufacturers, comboBox_manufacturer_pidbir);

            loadListToComboBox(shoesService.getDances(), comboBox_dance_pidbir, true);
            loadListToComboBox(shoesService.getColors(), comboBox_color_pidbir, true);
            loadListToComboBox(shoesService.getMatherials(), comboBox_matherial_pidbir, true);
            loadListToComboBox(shoesService.getManufacturers(), comboBox_manufacturer_pidbir, true);

            groupBox_pidbir.Visible = true;
            groupBox_form.Visible = false;
        }

        // btn_cancel_pidbir
        private void button2_Click(object sender, EventArgs e)
        {
            DisplayListFromService();
            groupBox_pidbir.Visible = false;
        }
        #endregion

        // btn_save
        private void button1_Click(object sender, EventArgs e)
        {
            shoesService.SaveListToFile();
            DisplayListFromService();
        }

        // btn_exit
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Filter methods and events

        private void checkBox_filter_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_filter_by.Enabled = !comboBox_filter_by.Enabled;
            comboBox_filter_field.Enabled = !comboBox_filter_field.Enabled;
            if (!checkBox_filter.Checked)
            {
                comboBox_filter_field.Text = "";
            }
            else
            {
                loadListToComboBox(ShoesService.filter_names, comboBox_filter_by);
                comboBox_filter_by.SelectedIndex = 0;
            }

            DisplayListFromService();
        }

        private void comboBox_filter_by_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox_filter_by.SelectedIndex;
            ShoesService.Filters filter = shoesService.getFilterByIndex(index);
            shoesService.setFilter(filter);

            comboBox_filter_field.Text = "";
            if (shoesService.getFilter() == ShoesService.Filters.gender)
            {
                loadListToComboBox(ShoesService.genders_name, comboBox_filter_field);
            }
            else
            {
                loadListToComboBox(shoesService.getFileredFieldList(filter), comboBox_filter_field);
            }
        }

        private void comboBox_filter_field_SelectedIndexChanged(object sender, EventArgs e)
        {
            applyFilter();
        }

        private void comboBox_filter_field_KeyUp(object sender, KeyEventArgs e)
        {
            applyFilter();
        }

        private void applyFilter()
        {
            Shoes.GenderEnum Gender = Shoes.GenderEnum.BOYS;
            bool gender_validated = true;

            string text = comboBox_filter_field.Text.ToLower().Trim();

            if(text.Length <= 0)
            {
                DisplayListFromService();
                return;
            }

            if(shoesService.getFilter() == ShoesService.Filters.gender) { 
                // { "MAN", "WOMEN", "GIRLS", "BOYS" };
                switch (comboBox_filter_field.SelectedIndex)
                {
                    case 0:
                        Gender = Shoes.GenderEnum.MAN;
                        break;
                    case 1:
                        Gender = Shoes.GenderEnum.WOMEN;
                        break;
                    case 2:
                        Gender = Shoes.GenderEnum.GIRLS;
                        break;
                    case 3:
                        Gender = Shoes.GenderEnum.BOYS;
                        break;
                    default:
                        if (text.StartsWith("ч"))
                        {
                           Gender = Shoes.GenderEnum.MAN;
                        }
                        else if (text.StartsWith("ж"))
                        {
                            Gender = Shoes.GenderEnum.WOMEN;
                        }
                        else if (text.StartsWith("д"))
                        {
                            Gender = Shoes.GenderEnum.GIRLS;
                        }
                        else if (text.StartsWith("х"))
                        {
                            Gender = Shoes.GenderEnum.BOYS;
                        }
                        else
                        {
                            gender_validated = false;
                        }
                        break;
                }
            }

            List<Shoes> shoeses = new List<Shoes>();
            foreach (Shoes shoes in shoesService.getList())
            {
                string field = "_";
                switch (shoesService.getFilter())
                {
                    case ShoesService.Filters.color:
                        field = shoes.Color;
                        break;

                    case ShoesService.Filters.names:
                        field = shoes.Name;
                        break;

                    case ShoesService.Filters.manufacturers:
                        field = shoes.Manufacturer;
                        break;

                    case ShoesService.Filters.matherial:
                        field = shoes.Matherial;
                        break;

                    case ShoesService.Filters.dances:
                        field = shoes.Dance;
                        break;
                }
                field = field.ToLower().Trim();

                if (field.Length >= 0
                    && (field.Equals(text)
                        || field.Contains(text)
                        || text.Contains(field)
                        || field.StartsWith(text)
                        || field.EndsWith(text))
                    || ((shoesService.getFilter() == ShoesService.Filters.gender) 
                        && (shoes.Gender == Gender) && gender_validated)
                    )
                {
                    shoeses.Add(shoes);
                }
            }

            DisplayList(shoeses);
        }

        #endregion

        private void numericUpDown_size_bidbir_ValueChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_old_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
