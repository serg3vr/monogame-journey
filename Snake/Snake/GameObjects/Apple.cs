using System;
using System.Collections.Generic;
using Snake.Graphics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake.GameObjects;

public class Apple : Sprite
{
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; }

    private readonly Texture2D _texture;

    public Apple(Texture2D texture, Vector2 position, Vector2 size)
        : base(texture, position, size)
    {
        var w = (int)size.X;
        var h = (int)size.Y;
        _texture = new Texture2D(texture.GraphicsDevice, w, h);
        var data = new Color[w * h];

        const int segments = 128;
        var rawVerts = new Vector2[segments];
        for (var i = 0; i < segments; i++)
        {
            var t = i * MathHelper.TwoPi / segments;
            var cosT = Math.Cos(t);
            var sinT = Math.Sin(t);
            double r = 1.0;
            r += 0.3 * cosT * cosT;
            r -= 0.15 * sinT;
            r -= 0.2 * Math.Pow(Math.Max(0.0, sinT), 6);
            rawVerts[i] = new Vector2(
                (float)(r * 15 * cosT),
                (float)(-(r * 17 * sinT)));
        }

        var minX = float.MaxValue;
        var maxX = float.MinValue;
        var minY = float.MaxValue;
        var maxY = float.MinValue;
        foreach (var v in rawVerts)
        {
            if (v.X < minX) minX = v.X;
            if (v.X > maxX) maxX = v.X;
            if (v.Y < minY) minY = v.Y;
            if (v.Y > maxY) maxY = v.Y;
        }

        var scale = Math.Min((w - 2) / (maxX - minX), (h - 2) / (maxY - minY));
        var verts = new Vector2[segments];
        for (var i = 0; i < segments; i++)
        {
            verts[i] = new Vector2(
                (rawVerts[i].X - minX) * scale + 1,
                (rawVerts[i].Y - minY) * scale + 1);
        }

        for (var sy = 0; sy < h; sy++)
        {
            var intersections = new List<float>();
            for (var i = 0; i < segments; i++)
            {
                var j = (i + 1) % segments;
                var v1 = verts[i];
                var v2 = verts[j];

                if ((v1.Y <= sy && v2.Y > sy) || (v2.Y <= sy && v1.Y > sy))
                {
                    var t = (sy - v1.Y) / (v2.Y - v1.Y);
                    intersections.Add(v1.X + t * (v2.X - v1.X));
                }
            }

            intersections.Sort();
            for (var k = 0; k + 1 < intersections.Count; k += 2)
            {
                var x1 = (int)Math.Ceiling(intersections[k]);
                var x2 = (int)Math.Floor(intersections[k + 1]);
                for (var sx = Math.Max(0, x1); sx <= Math.Min(w - 1, x2); sx++)
                    data[sy * w + sx] = Color.White;
            }
        }

        _texture.SetData(data);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        var b = 1;
        spriteBatch.Draw(_texture, new Rectangle((int)Position.X - b, (int)Position.Y - b, (int)Size.X + b * 2, (int)Size.Y + b * 2), Color.White);
        spriteBatch.Draw(_texture, Bounds, SpriteColor);
    }
}
