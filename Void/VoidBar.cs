using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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
		private VoidBarSprite barAmount2;
		private SoulBar barAmount3;
		private BarDivider barDivider;
		private VoidBarBorder barBackground;
		private VoidBarBorder2 outline;
		private UIText text;
		public override void OnInitialize()
		{
			Height.Set(height, 0f); //Set Height of element
			Width.Set(width, 0f);   //Set Width of element

			barBackground = new VoidBarBorder(); 
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
			barAmount.Left.Set(6f, 0f);
			barAmount.Top.Set(6f, 0f);
			barAmount.Width.Set(188, 0f);
			barAmount.Height.Set(18, 0f);
			
			barAmount2 = new VoidBarSprite();
			barAmount2.SetPadding(0);
			barAmount2.Left.Set(6f, 0f);
			barAmount2.Top.Set(6f, 0f);
			barAmount2.backgroundColor = new Color(164, 55, 65); 
			barAmount2.Width.Set(188, 0f);
			barAmount2.Height.Set(18, 0f);

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

			barBackground.Append(barAmount2);
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
			string voidManaMaxText = voidPlayer.voidMeterMax2.ToString();
			string voidSoulsText = voidPlayer.lootingSouls.ToString();
			barAmount3.backgroundColor = VoidPlayer.soulLootingColor;
			if (text != null)
			{
				text.SetText("Void: " + voidManaText + " / " + voidManaMaxText);
				if(voidPlayer.lootingSouls > 0)
				{
					voidManaMaxText = (voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls).ToString();
					text.SetText("Void: " + (voidPlayer.lootingSouls < voidPlayer.voidMeterMax2 ? (voidManaText + "/" + voidManaMaxText) + " " : "0 ") + "(" + voidSoulsText + ")");
				}
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
			if(quotient > 1)
			{
				quotient = 1;
			}
			barAmount.Width.Set(quotient * 188, 0f);
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
    }
}