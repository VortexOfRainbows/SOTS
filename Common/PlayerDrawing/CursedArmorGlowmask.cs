using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class CursedArmorGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> cursedArmorGlowmaskTexture;
		public override bool IsHeadLayer => true;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "CursedHood", EquipType.Head);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (cursedArmorGlowmaskTexture == null)
			{
				cursedArmorGlowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Pyramid/CursedHood_HeadGlow");
			}
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawPlayer.position.X + drawPlayer.width / 2;
			float drawY = (int)drawPlayer.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect; //assumed replacement for drawInfo.bodyOrigin
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = MachinaBooster.changeColorBasedOnStealth(Color.White, drawInfo);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			//position = new Vector2((int)position.X, (int)position.Y); // You'll sometimes want to do this, to avoid quivering.

			drawInfo.DrawDataCache.Add(new DrawData(cursedArmorGlowmaskTexture.Value, position, frame, color * alpha, rotation, origin * 0.5f, 1f, SpriteEffects.None, 0 ));
		}
	}
}