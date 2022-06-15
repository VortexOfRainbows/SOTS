using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class StarbeltGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> starbeltGlowmaskTexture;
		//public override bool IsHeadLayer => true;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.waist == EquipLoader.GetEquipSlot(Mod, "Starbelt", EquipType.Waist);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (starbeltGlowmaskTexture == null)
				starbeltGlowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Otherworld/FromChests/Starbelt_WaistGlow");
			float alpha = 1 - drawInfo.shadow;
			Player drawPlayer = drawInfo.drawPlayer;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = Color.White;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			DrawData drawData = new DrawData(starbeltGlowmaskTexture.Value, position, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.cWaist;
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}