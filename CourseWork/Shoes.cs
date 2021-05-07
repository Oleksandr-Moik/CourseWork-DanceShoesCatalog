using System;
using System.Collections.Generic;
using System.Drawing;

namespace CourseWork
{
    [Serializable]
    public class Shoes
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Dance { get; set; }
        public enum GenderEnum { MAN, WOMEN, GIRLS, BOYS }; 
        public GenderEnum Gender { get; set; }
        public int HeelHeight { get; set; }
        public string Color { get; set; }
        public string Matherial { get; set; }
        public int Size { get; set; }
        public string Manufacturer { get; set; }

        public List<string> Pictures { get; set; }

        public Shoes()
        {
            Id = "not_uniq";
            Name = "";
            Dance = "";
            Gender = GenderEnum.BOYS;
            HeelHeight = 1;
            Color = "";
            Matherial = "";
            Size = 0;
            Manufacturer = "";

            Pictures = new List<string>();
        }

        public Shoes(string id, string name, string dance, GenderEnum gender, int heelHeight, string color, string matherial, int size, string manufacturer)
        {
            Id = id;
            Name = name;
            Dance = dance;
            Gender = gender;
            HeelHeight = heelHeight;
            Color = color;
            Matherial = matherial;
            Size = size;
            Manufacturer = manufacturer;

            Pictures = new List<string>();
        }


        public override string ToString()
        {
            return $"{Name},\t '{Manufacturer}'";
        }
    }

}
