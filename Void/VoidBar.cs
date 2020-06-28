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
		private VoidBarBorder barBackground;
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
			
			text = new UIText("0|0"); 
			text.Width.Set(width, 0f);
			text.Height.Set(height, 0f);
			text.Top.Set(height / 2 - text.MinHeight.Pixels / 2, 0f); 

			barBackground.Append(barAmount2);
			
			barBackground.Append(barAmount);
			
			barBackground.Append(text);
			
			base.Append(barBackground);
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Main.myPlayer];
			int voidmeter = (int)VoidPlayer.ModPlayer(player).voidMeter;
			if(voidmeter < 0)
			{
				voidmeter = 0;
			}
			string voidManaText = voidmeter.ToString();
			string voidManaMaxText = VoidPlayer.ModPlayer(player).voidMeterMax2.ToString();
			
			if(text != null)
				text.SetText(voidManaText  + "/" + voidManaMaxText + " Void"); 
			float quotient = 1f;
			//Calculate quotient
			switch (stat)
			{
				case VoidBarMode.voidAmount:
					quotient = VoidPlayer.ModPlayer(player).voidMeter / VoidPlayer.ModPlayer(player).voidMeterMax2;
					break;

				default:
					break;
			}
			barAmount.Width.Set(quotient * 188, 0f);
			Recalculate();

			base.Draw(spriteBatch);
		}
    }
}