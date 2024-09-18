using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Linq;
using System.Text;
using Blish_HUD.Controls;
using System.Collections.Generic;

namespace Blish_HUD {
    public static class DrawUtil {

        public static void DrawAlignedText(SpriteBatch sb, SpriteFont sf, string text, Rectangle bounds, Color clr, HorizontalAlignment ha, VerticalAlignment va) {
            // Filter out any characters our font doesn't support
            text = string.Join("", text.ToCharArray().Where(c => sf.Characters.Contains(c)));

            var textSize = sf.MeasureString(text);

            int xPos = bounds.X;
            int yPos = bounds.Y;

            if (ha == HorizontalAlignment.Center) xPos += bounds.Width / 2 - (int)textSize.X / 2;
            if (ha == HorizontalAlignment.Right) xPos += bounds.Width - (int)textSize.X;

            if (va == VerticalAlignment.Middle) yPos += bounds.Height / 2 - (int)textSize.Y / 2;
            if (va == VerticalAlignment.Bottom) yPos += bounds.Height - (int)textSize.Y;

            sb.DrawString(sf, text, new Vector2(xPos, yPos), clr);
        }

        public static void DrawAlignedText(SpriteBatch sb, BitmapFont sf, string text, Rectangle bounds, Color clr, HorizontalAlignment ha = HorizontalAlignment.Left, VerticalAlignment va = VerticalAlignment.Middle) {
            Vector2 textSize = sf.MeasureString(text);

            int xPos = bounds.X;
            int yPos = bounds.Y;

            if (ha == HorizontalAlignment.Center) xPos += bounds.Width / 2 - (int)textSize.X / 2;
            if (ha == HorizontalAlignment.Right) xPos += bounds.Width - (int)textSize.X;

            if (va == VerticalAlignment.Middle) yPos += bounds.Height / 2 - (int)textSize.Y / 2;
            if (va == VerticalAlignment.Bottom) yPos += bounds.Height - (int)textSize.Y;

            sb.DrawString(sf, text, new Vector2(xPos, yPos), clr);
        }

        private static string WrapTextSegment(BitmapFont spriteFont, string text, float maxLineWidth) {
            return WrapTextSegment(spriteFont, text, maxLineWidth, out _);
        }

        /// <remarks>
        /// Source: https://stackoverflow.com/a/15987581/595437
        /// (slightly modified)
        /// </remarks>
        private static string WrapTextSegment(BitmapFont spriteFont, string text, float maxLineWidth, out int[] newLineIndices) {
            newLineIndices = Array.Empty<int>();
            if (string.IsNullOrEmpty(text)) return string.Empty;

            string[] words = text.Split(' ');
            var sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").Width;

            List<int> indices = new List<int>();
            int characterIndex = 0;

            for (int i = 0; i < words.Length; i++) {
                string word = words[i];
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth) {
                    sb.Append(word);
                    lineWidth += size.X;
                    if (i < words.Length - 1) {
                        sb.Append(" ");
                        lineWidth += spaceWidth;
                    }
                } else {
                    sb.Append("\n" + word);
                    lineWidth = size.X;
                    if (i < words.Length - 1) {
                        sb.Append(" ");
                        lineWidth += spaceWidth;
                    }
                    indices.Add(characterIndex);
                    characterIndex++;
                }
                characterIndex += word.Length + 1;
            }

            newLineIndices = indices.ToArray();
            return sb.ToString();
        }

        public static string WrapText(BitmapFont spriteFont, string text, float maxLineWidth) {
            return WrapText(spriteFont, text, maxLineWidth, out _);
        }

        public static string WrapText(BitmapFont spriteFont, string text, float maxLineWidth, out int[] newLineIndices) {
            newLineIndices = Array.Empty<int>();
            if (string.IsNullOrEmpty(text)) return "";

            var sb = new StringBuilder();
            List<int> indices = new List<int>();
            int lineStart = 0;

            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++) {
                sb.Append(WrapTextSegment(spriteFont, lines[i], maxLineWidth, out int[] localNewLineIndices));
                foreach (int localIndex in localNewLineIndices) {
                    indices.Add(localIndex + lineStart);
                }

                lineStart += lines[i].Length;

                if (i < lines.Length - 1) {
                    sb.Append('\n');
                    lineStart++;
                }
            }

            newLineIndices = indices.ToArray();
            return sb.ToString();
        }

    }
}
