using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace DragonUI
{
    public class DragonUISystem
    {
        public List<DragonUIComponent> druiComponents = new List<DragonUIComponent>();

        GraphicsDevice graphics;
        public Texture2D placeholderTexture { get; private set; }

        public DragonUISystem(GraphicsDevice aGraphics, Texture2D aPlaceholderTexture)
        {
            graphics = aGraphics;
            placeholderTexture = aPlaceholderTexture;
        }

        public DragonUISystem(GraphicsDevice aGraphics)
        {
            graphics = aGraphics;
        }

        public void Add(DragonUIComponent component)
        {
            druiComponents.Add(component);
            component.RegisterSystem(this);
        }

        public void Remove(DragonUIComponent component)
        {
            druiComponents.Remove(component);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (DragonUIComponent component in druiComponents)
            {
                component.Draw(spriteBatch);
            }
        }

        public Texture2D LoadTexture(string filename)
        {
            Stream imageFile = File.Open(filename, FileMode.Open);
            Texture2D texture = Texture2D.FromStream(graphics, imageFile);
            imageFile.Close();

            return texture;
        }
    }
}
