using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class TwilightAssassinHeadGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> circletGlowmaskTexture;
		public override bool IsHeadLayer => true;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "TwilightAssassinsCirclet", EquipType.Head);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (circletGlowmaskTexture == null)
			{
				circletGlowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Otherworld/FromChests/TwilightAssassinsCirclet_HeadGlow");
			}
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(circletGlowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cHead;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class TwilightAssassinBodyGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> glowmaskTexture;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, "TwilightAssassinsChestplate", EquipType.Body);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (glowmaskTexture == null)
			{
				glowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Otherworld/FromChests/TwilightAssassinsChestplate_BodyGlow");
			}
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(glowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class TwilightAssassinBackArmGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> glowmaskTexture;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, "TwilightAssassinsChestplate", EquipType.Body);
		}
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Torso); //hopefully this covers backarm location
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (glowmaskTexture == null)
			{
				glowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Otherworld/FromChests/TwilightAssassinsChestplate_BodyGlow");
			}
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawInfo.compBackArmFrame;
			float rotation = drawInfo.compositeBackArmRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(glowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class TwilightAssassinFrontArmGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> glowmaskTexture;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(Mod, "TwilightAssassinsChestplate", EquipType.Body);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (glowmaskTexture == null)
			{
				glowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Otherworld/FromChests/TwilightAssassinsChestplate_BodyGlow");
			}
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawInfo.compFrontArmFrame;
			float rotation = drawInfo.compositeFrontArmRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(glowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class TwilightAssassinLegsGlowmask : PlayerDrawLayer
	{
		private Asset<Texture2D> leggingsGlowmaskTexture;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.legs == EquipLoader.GetEquipSlot(Mod, "TwilightAssassinsLeggings", EquipType.Legs);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Leggings);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (leggingsGlowmaskTexture == null)
			{
				leggingsGlowmaskTexture = ModContent.Request<Texture2D>("SOTS/Items/Otherworld/FromChests/TwilightAssassinsLeggings_LegsGlow");
			}
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.legFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawPlayer.legFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(leggingsGlowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cLegs;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}