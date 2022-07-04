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
		public override bool IsHeadLayer => false;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "CursedHood", EquipType.Head);
		}
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.FinchNest); // this is because head layer forces a draw onto the map
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
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = MachinaBooster.changeColorBasedOnStealth(Color.White, drawInfo);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			drawInfo.DrawDataCache.Add(new DrawData(cursedArmorGlowmaskTexture.Value, position, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0 ));
		}
	}
}