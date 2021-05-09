using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SOTS.Void
{
    internal enum VoidBarMode
    {
        voidAmount
    }
    class VoidBar : UIElement
    {
        private VoidBarMode stat;
        private float width;
        private float height;
        public VoidBar(VoidBarMode stat, int height, int width)
        {
            this.stat = stat;
            this.width = 200f;
            this.height = 30f;
        }
		private VoidBarSprite barAmount;
		private SoulBar barAmount3;
		private BarDivider barDivider;
		private VoidBarBorder2 barBackground;
		private VoidBarBorder2 outline;
		private UIText text;

        public override void OnInitialize()
		{
			Height.Set(height, 0f); //Set Height of element
			Width.Set(width, 0f);   //Set Width of element

			barBackground = new VoidBarBorder2(); 
			barBackground.Left.Set(0f, 0f);
			barBackground.Top.Set(0f, 0f);
			barBackground.Width.Set(width, 0f);
			barBackground.Height.Set(height, 0f);

			outline = new VoidBarBorder2();
			outline.Left.Set(0f, 0f);
			outline.Top.Set(0f, 0f);
			outline.Width.Set(width, 0f);
			outline.Height.Set(height, 0f);

			barDivider = new BarDivider();
			barDivider.Left.Set(2f, 0f);
			barDivider.Top.Set(4f, 0f);
			barDivider.Width.Set(10, 0f);
			barDivider.Height.Set(22, 0f);

			barAmount = new VoidBarSprite(); 
			barAmount.SetPadding(0);
			barAmount.Left.Set(5.01f, 0f);
			barAmount.Top.Set(6f, 0f);
			barAmount.Width.Set(188, 0f);
			barAmount.Height.Set(18, 0f);

			barAmount3 = new SoulBar();
			barAmount3.SetPadding(0);
			barAmount3.Left.Set(6f, 0f);
			barAmount3.Top.Set(6f, 0f);
			barAmount3.Width.Set(188, 0f);
			barAmount3.Height.Set(18, 0f);

			text = new UIText("0|0"); 
			text.Width.Set(width, 0f);
			text.Height.Set(height, 0f);
			text.Top.Set(height - 42 - text.MinHeight.Pixels / 2, 0f); 

			barBackground.Append(barAmount);
			barBackground.Append(barAmount3);
			barBackground.Append(barDivider);
			barBackground.Append(outline);

			barBackground.Append(text);
			
			base.Append(barBackground);
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Main.myPlayer];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			int voidmeter = (int)voidPlayer.voidMeter;
			if(voidmeter < 0)
			{
				voidmeter = 0;
			}
			string voidManaText = voidmeter.ToString();
			int voidMax = voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption;
			string voidManaMaxText = voidMax.ToString();
			string voidSoulsText = voidPlayer.lootingSouls.ToString();
			barAmount3.backgroundColor = VoidPlayer.soulLootingColor;
			if (text != null)
			{
				string textThing = "Void: ";
				if (voidMax - voidPlayer.lootingSouls <= 0)
                {
					textThing += "0 ";
                }
				else
				{
					voidManaMaxText = (voidMax - voidPlayer.lootingSouls).ToString();
					textThing += voidManaText + " / " + voidManaMaxText;
                }
				if(voidPlayer.lootingSouls > 0 || voidPlayer.VoidMinionConsumption > 0)
				{
					textThing += " (";
					if (voidPlayer.VoidMinionConsumption > 0)
						textThing += voidPlayer.VoidMinionConsumption.ToString();
					if (voidPlayer.lootingSouls > 0 && voidPlayer.VoidMinionConsumption > 0)
						textThing += " + ";
					if (voidPlayer.lootingSouls > 0)
						textThing += voidSoulsText;
					textThing += ")";
				}
				text.SetText(textThing);
			}
			float quotient = 1f;
			float quotient2 = 1f;
			//Calculate quotient
			switch (stat)
			{
				case VoidBarMode.voidAmount:
					quotient = voidPlayer.voidMeter / voidPlayer.voidMeterMax2;
					quotient2 = (float)voidPlayer.lootingSouls / voidPlayer.voidMeterMax2;
					break;

				default:
					break;
			}
			Texture2D fill = ModContent.GetTexture("SOTS/Void/SoulBar");
			Texture2D divider = ModContent.GetTexture("SOTS/Void/SoulBarDivider");
			if (quotient > 1)
			{
				quotient = 1;
			}
			if (quotient < 0)
				quotient = 0;
			Vector2 padding = new Vector2(6, 6);
			float prevRight = quotient * 188;
			float length = 40;
			int height = 18;
			Texture2D fill2 = ModContent.GetTexture("SOTS/Void/VoidBarSprite");
			Texture2D divider2 = ModContent.GetTexture("SOTS/Void/VoidBarDivider");
			spriteBatch.Draw(fill2, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), 188, 18), new Color(164, 55, 65));
			List<Rectangle> rectangles = new List<Rectangle>();
			List<Color> colors = new List<Color>();
			for (int i = 0; i < voidPlayer.VoidMinions.Count; i++)
            {
				int type = voidPlayer.VoidMinions[i];
				int nextType = -1;
				if (i < voidPlayer.VoidMinions.Count - 1)
					nextType = voidPlayer.VoidMinions[i + 1];
				int cost = VoidPlayer.minionVoidCost(type);
				length = cost / (float)voidPlayer.voidMeterMax2 * 188f;
				Color color = Color.White;
				color = VoidPlayer.minionVoidColor(type);
				if (length + prevRight >= 188)
					length = 188 - prevRight;
				spriteBatch.Draw(fill, new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y), (int)(length + 1), height), color);
				if(i == 0)
				{
					rectangles.Add(new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y - 2), 6, 22));
					colors.Add(color);
				}					
				prevRight += length;
				if (nextType == -1 || nextType != type)
				{
					rectangles.Add(new Rectangle((int)(VoidPlayer.voidBarOffset.X + padding.X + (int)prevRight), (int)(VoidPlayer.voidBarOffset.Y + padding.Y - 2), 6, 22));
					colors.Add(color);
				}
			}
			for(int i = 0; i < rectangles.Count; i++)
			{
				if(i == 0)
				{
					spriteBatch.Draw(divider2, rectangles[i], Color.Gray);
				}
				else
				{
					spriteBatch.Draw(divider, rectangles[i], colors[i]);
				}
			}
			barAmount.Width.Set((int)(quotient * 188 + 1f), 0f);
			if(voidPlayer.lootingSouls > 0)
			{
				barAmount3.Width.Set(quotient2 * 188, 0f);
				barAmount3.Left.Set(200f - quotient2 * 188 - 6, 0f);
				barDivider.Width.Set(10, 0);
				barDivider.Left.Set(200f - quotient2 * 188 - 10, 0f);
			}
			else
            {
				barAmount3.Width.Set(0, 0);
				barDivider.Width.Set(0, 0);
			}
			Recalculate();
			base.Draw(spriteBatch);
		}
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}