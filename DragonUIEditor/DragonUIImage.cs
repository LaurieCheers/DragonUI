using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace DragonUI
{
    class DragonUIImage:DragonUIComponent
    {
        string m_filename;

        [CategoryAttribute("Image")]
        public string filename { get { return m_filename; } set { m_filename = value; UpdateCache(); } }

        Texture2D cachedImage;
        DragonUISystem druiSystem;

        public DragonUIImage(string aImagePath)
        {
            filename = aImagePath;
        }

        public DragonUIImage(JSONTable template)
        {
            filename = template.getString("filename");
            rect = template.getArray("rect").toRectangle();
        }

        public override void RegisterSystem(DragonUISystem system)
        {
            druiSystem = system;
            UpdateCache();
            if (rect.Width == 0 && rect.Height == 0)
            {
                rect = new Rectangle(rect.X, rect.Y, cachedImage.Width, cachedImage.Height);
            }
        }

        public void UpdateCache()
        {
            if (druiSystem != null)
            {
                try
                {
                    cachedImage = druiSystem.LoadTexture(filename);
                }
                catch (Exception ex)            
                {
                    if (ex is FileNotFoundException || ex is DirectoryNotFoundException)
                    {
                        cachedImage = druiSystem.placeholderTexture;
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (cachedImage != null)
            {
                spriteBatch.Draw(cachedImage, rect, Color.White);
            }
        }

        public override void SaveMembersTo(StreamWriter writer)
        {
            writer.WriteLine("\"type\":\"image\",");
            writer.WriteLine("\"filename\":\"" + JSONTable.escapeString(filename) + "\",");
            base.SaveMembersTo(writer);
        }
    }
}
