using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.ComponentModel;

namespace DragonUI
{
    public abstract class DragonUIComponent//: INotifyPropertyChanged
    {
/*        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }*/

        public Rectangle rect;
        /*public Rectangle rect { get { return _rect; } set {
            _rect = value;
            NotifyPropertyChanged("X");
            NotifyPropertyChanged("Y");
            NotifyPropertyChanged("Width");
            NotifyPropertyChanged("Height");
        }}*/

        [CategoryAttribute("Bounds")]
        public int X { get { return rect.X; } set {
            rect.X = value;
            //NotifyPropertyChanged("X");
        } }
        [CategoryAttribute("Bounds")]
        public int Y { get { return rect.Y; } set {
            rect.Y = value;
            //NotifyPropertyChanged("Y");
        } }
        [CategoryAttribute("Bounds")]
        public int Width { get { return rect.Width; } set {
            rect.Width = value;
            //NotifyPropertyChanged("Width");
        } }
        [CategoryAttribute("Bounds")]
        public int Height { get { return rect.Height; } set {
            rect.Height = value;
            //NotifyPropertyChanged("Height");
        } }

        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual void RegisterSystem(DragonUISystem system) { }
        public virtual void SaveMembersTo(StreamWriter writer)
        {
            writer.WriteLine("\"rect\":[" + rect.X + "," + rect.Y + "," + rect.Width + "," + rect.Height + "]");
        }

        public static DragonUIComponent newFromTemplate(JSONTable template)
        {
            switch (template.getString("type"))
            {
                case "image": return new DragonUIImage(template);
            }

            return null;
        }
    }
}
