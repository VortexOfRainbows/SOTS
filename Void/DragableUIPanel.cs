using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace SOTS.Void
{
    internal class DragableUIPanel : UIPanel
    {
		public Vector2 offset;
		public bool dragging;
		public int aliveFor = 0;
		public int type = -1;
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);
			if (!SOTS.Config.lockVoidBar)
			{
				DragStart(evt);
			}
		}

		public override void MouseUp(UIMouseEvent evt) {
			base.MouseUp(evt);
			if (!SOTS.Config.lockVoidBar)
			{
				DragEnd(evt);
			}
		}

		private void DragStart(UIMouseEvent evt) {
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt) {
			Vector2 end = evt.MousePosition;
			dragging = false;

			Left.Set(end.X - offset.X, 0f);
			Top.Set(end.Y - offset.Y, 0f);

			Recalculate();
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime); // don't remove.

			// Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
			if (ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
			if (dragging)
			{
				float xPos = Main.mouseX - offset.X;
				float yPos = Main.mouseY - offset.Y;
				if (type == 0)
				{
					xPos = (int)MathHelper.Clamp(xPos, 0, Main.screenWidth - 200);
					yPos = (int)MathHelper.Clamp(yPos, 0, Main.screenHeight - 30);
				}
				Left.Set(xPos, 0f); // Main.MouseScreen.X and Main.mouseX are the same.
				Top.Set(yPos, 0f);
				if(type == 0)
				{
					//Main.NewText("Set new offset: " + SOTSConfig.voidBarNeedsLoading);
					//VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset = new Vector2((int)Left.Pixels, (int)Top.Pixels);
					VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset.X = (int)Left.Pixels;
					VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset.Y = (int)Top.Pixels;
				}
				Recalculate();
			}
			else if(type == 0)
			{
				float scale = Main.UIScale;
				Left.Set(SOTS.Config.voidBarPointX, 0f);
				Top.Set(SOTS.Config.voidBarPointY, 0f);
				//VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset = new Point(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY).ToVector2();
			}
			if(type == 0)
			{
				if (SOTSConfig.voidBarNeedsLoading >= 2)
				{
					Main.NewText("Alive For: " + SOTSConfig.voidBarNeedsLoading);
					Main.NewText("voidBarPointX: " + SOTS.Config.voidBarPointX);
					Main.NewText("voidBarOffsetX: " + VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset.X);
					SOTSConfig.voidBarNeedsLoading--;
				}
				else
				{
					SOTS.Config.voidBarPointX = (int)MathHelper.Clamp(VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset.X, 0, Main.screenWidth - 200);
					SOTS.Config.voidBarPointY = (int)MathHelper.Clamp(VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset.Y, 0, Main.screenHeight - 30);
				}
				VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset = new Vector2(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY);
			}
		   // Here we check if the DragableUIPanel is outside the Parent UIElement rectangle. 
		   // (In our example, the parent would be ExampleUI, a UIState. This means that we are checking that the DragableUIPanel is outside the whole screen)
		   // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution.
		   var parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace)) {
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				// Recalculate forces the UI system to do the positioning math again.
				Recalculate();
			}
		}
    }
}