using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Void
{
	internal class VoidUI : UIState
	{
		public static bool visible = false;
		public static DragableUIPanel voidUI;
		public override void OnInitialize()
		{
			voidUI = new DragableUIPanel();
			voidUI.Height.Set(30f, 0f);
			voidUI.Width.Set(200f, 0f);
			voidUI.Left.Set(VoidPlayer.voidBarOffset.X, 0f);
			voidUI.Top.Set(VoidPlayer.voidBarOffset.Y, 0f);
			voidUI.type = 0;
			voidUI.BackgroundColor = new Color(255, 255, 255, 255);
			voidUI.SetPadding(0);

			VoidBar voidAmount = new VoidBar(VoidBarMode.voidAmount, 280, 25);
			voidAmount.Height.Set(30f, 0f);
			voidAmount.Width.Set(200f, 0f);
			voidAmount.Left.Set(0f, 0f);
			voidAmount.Top.Set(0f, 0f);
			voidUI.Append(voidAmount);
			Append(voidUI);
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Recalculate();
		}
		bool runOnce = true;
		public override void Update(GameTime gameTime)
		{
			if(runOnce)
			{
				voidUI.Left.Set(VoidPlayer.voidBarOffset.X, 0f);
				voidUI.Top.Set(VoidPlayer.voidBarOffset.Y, 0f);
			}
			base.Update(gameTime);
		}
        public override void OnDeactivate()
        {
			VoidPlayer.voidBarOffset = new Vector2(voidUI.Left.Pixels, voidUI.Top.Pixels);
            base.OnDeactivate();
        }
    }
}