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
		public override bool IsHeadLayer => false;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "TwilightAssassinsCirclet", EquipType.Head);
		}
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.FinchNest); // this is because head layer forces a draw onto the map
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
				Vector2 addition = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(i));
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
			Vector2 bobbingOffset = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
			bobbingOffset.Y -= 2f;
			bobbingOffset *= -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f + drawInfo.torsoOffset + bobbingOffset.Y;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawInfo.compTorsoFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(glowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class TwilightAssassinBackArmGlowmask : PlayerDrawLayer
	{
		public static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawInfo)
		{
			return new Vector2(6 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
		}

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
			float alpha = 1 - drawInfo.shadow;
			Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
			Vector2 bobbingOffset = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
			bobbingOffset.Y -= 2f;
			vector += bobbingOffset * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			vector.Y += drawInfo.torsoOffset;
			float bodyRotation = drawInfo.drawPlayer.bodyRotation;
			Vector2 vector3 = vector;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawInfo);
			vector3 += compositeOffset_BackArm;
			float rotation = bodyRotation + drawInfo.compositeBackArmRotation;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(i));
				PlayerDrawLayers.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.BackArm, new DrawData(glowmaskTexture.Value, vector3 + addition, drawInfo.compBackArmFrame, color * alpha, rotation, drawInfo.bodyVect + compositeOffset_BackArm, 1f, drawInfo.playerEffect, 0)
				{
					shader = drawInfo.cBody
				});
			}
		}
	}
	public class TwilightAssassinFrontArmGlowmask : PlayerDrawLayer
	{
		public static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawInfo)
		{
			return new Vector2(-5 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
		}
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
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
			Vector2 bobbingOffset = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
			bobbingOffset.Y -= 2f;
			vector += bobbingOffset * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			vector.Y += drawInfo.torsoOffset;
			if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
			{
				vector += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
			}
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = vector + GetCompositeOffset_FrontArm(ref drawInfo); 
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawInfo.compFrontArmFrame;
			float rotation = drawPlayer.bodyRotation + drawInfo.compositeFrontArmRotation;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(glowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin + GetCompositeOffset_FrontArm(ref drawInfo), 1f, spriteEffects, 0);
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
				Vector2 addition = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(leggingsGlowmaskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cLegs;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}