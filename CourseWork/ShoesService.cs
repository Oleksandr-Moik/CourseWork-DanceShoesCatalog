using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CourseWork
{
    public class ShoesService
    {
        public static string[] filter_names = { "Танець", "Виробник", "Назва", "Колір", "Матеріал", "Cтать" };
        public static string[] genders_name = { "MAN", "WOMEN", "GIRLS", "BOYS" };
        public enum Filters { dances, manufacturers, names, color, matherial, gender };

        private string FilePath = "./shoeses";
        private string PictureFolderPath = "./pictures/";

        private List<Shoes> list;
        private Filters filter;

        public ShoesService()
        {
            list = new List<Shoes>();
        }

        public Shoes add(Shoes shoes)
        {
            shoes.Id = GenerateId();
            list.Add(shoes);
            return shoes;
        }

        public Shoes get(int index)
        {
            foreach(Shoes shoes in list)
            {
                if (list.IndexOf(shoes) == index)
                {
                    return shoes;
                }
            }
            return null;
        }

        public Shoes update(Shoes item)
        {
            Shoes shoes = findById(item.Id);

            if (shoes == null)
            {
                return add(item);
            }

            int index = list.IndexOf(shoes);

            list.RemoveAt(index);
            list.Insert(index, item);

            return get(index);
        }

        public bool delete(string id)
        {
            Shoes shoes = findById(id);
            if (shoes == null) return false;

            if (Directory.Exists(Path.Combine(PictureFolderPath, id))){
                Directory.Delete(Path.Combine(PictureFolderPath, id), true);
            }

            return list.Remove(findById(id));
        }

        public Shoes findById(string id)
        {
            foreach (Shoes shoes in list)
            {
                if (shoes.Id.CompareTo(id) == 0)
                {
                    return shoes;
                }
            }
            return null;
        }

        public List<Shoes> getList()
        {
            return this.list;
        }

        public void setList(List<Shoes> shoes)
        {
            if (shoes != null)
            {
                this.list = shoes;
            }
        }

        public string saveImage(string id, string filePath)
        {
            DirectoryInfo directory = Directory.CreateDirectory(Path.Combine(PictureFolderPath, id));

            string new_path = Path.Combine(directory.FullName, GenerateId()+ ".png");

            File.Copy(filePath, new_path, false);

            return new_path;
        }

        //file save read
        public void SaveListToFile()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(stream, list);
            stream.Close();
        }
        public void LoadListsFromFiles()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = null;
            try
            {
                stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                if (stream.Length != 0)
                {
                    list = (List<Shoes>)formatter.Deserialize(stream);
                }
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                stream = new FileStream(FilePath, FileMode.Create);
                stream.Close();
            }
        }

        public void setFilter(Filters filter)
        {
            this.filter = filter;
        }

        public Filters getFilterByIndex(int index)
        {
            switch (index)
            {
                // follow by filters array and Filters enum
                case 0:
                    return Filters.dances;
                case 1:
                    return Filters.manufacturers;
                case 2:
                    return Filters.names;
                case 3:
                    return Filters.color;
                case 4:
                    return Filters.matherial;
                case 5:
                    return Filters.gender;
                default:
                    return Filters.names;
            }
        }
        
        public Filters getFilter()
        {
            return this.filter;
        }
        
        public List<string> getManufacturers()
        {
            return getFileredFieldList(Filters.manufacturers);
        }
        public List<string> getDances()
        {
            return getFileredFieldList(Filters.dances);
        }
        public List<string> getColors()
        {
            return getFileredFieldList(Filters.color);
        }
        public List<string> getMatherials()
        {
            return getFileredFieldList(Filters.matherial);
        }

        public List<string> getFileredFieldList(Filters filter)
        {
            List<string> result = new List<string>();

            string shoes_field = "";
            foreach (Shoes shoes in list)
            {
                switch (filter)
                {
                    case Filters.dances:
                        shoes_field = shoes.Dance;
                        break;
                    case Filters.manufacturers:
                        shoes_field = shoes.Manufacturer;
                        break;
                    case Filters.names:
                        shoes_field = shoes.Name;
                        break;
                    case Filters.color:
                        shoes_field = shoes.Color;
                        break;
                    case Filters.matherial:
                        shoes_field = shoes.Matherial;
                        break;
                    default:
                        return result;
                }
                shoes_field.ToLower().Trim();

                if (result.Contains(shoes_field) || shoes_field.Length <= 0)
                {
                    continue;
                }

                result.Add(shoes_field);
            }

            return result;
        }

        // генерування випадкового коду-ід
        private string GenerateId()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            DateTime dateTime = DateTime.Now;
            
            char ch;
            for (int i = 0; i < 12; i++)
            {
                // символ a-z або A-z (коди за таблицею ASCII
                ch = (i % 2 == 0) ?
                    Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))) :
                    Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 97)));
                builder.Append(ch);
            }
            builder.Append(dateTime.Millisecond.ToString());

            return builder.ToString();
        }

    }
}
