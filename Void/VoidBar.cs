using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace SOTS.Void
{
    internal enum VoidBarMode
    {
        voidAmount
    }
    class VoidBar : UIElement
    {
        private readonly float width = 200f;
        private readonly float height = 30f;
        public VoidBar()
        {

        }
		private VoidBarSprite barAmount;
		private VoidBarBorder2 barBackground;
		private UIText text;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); //do not remove
			string activeSet = Main.ResourceSetsManager.ActiveSetKeyName;
			//Main.NewText(activeSet);
			if(activeSet.Contains("Bar"))
			{
				//Main.NewText("Active");
				if (SOTSConfig.PreviousBarMode < 4)
				{
					SOTSConfig.PreviousBarMode++;
					if(!SOTS.Config.lockVoidBar)
					{
						SOTS.Config.voidBarPointX = Main.screenWidth - 254;
						SOTS.Config.voidBarPointY = 64;
						SOTS.Config.alternateVoidBarDirection = true;
						SOTS.Config.alternateVoidBarStyle = true;
						SOTS.Config.voidBarTextOn = false;
						SOTS.Config.voidBarHoverTextOn = true;
						VoidPlayer.ModPlayer(Main.LocalPlayer).voidBarOffset = new Vector2(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY);
					}
				}
			}
			else
			{
				SOTSConfig.PreviousBarMode = 0;
			}
        }
        public override void OnInitialize()
		{
			Height.Set(height, 0f); //Set Height of element
			Width.Set(width, 0f);   //Set Width of element

			barBackground = new VoidBarBorder2(); 
			barBackground.Left.Set(0f, 0f);
			barBackground.Top.Set(0f, 0f);
			barBackground.Width.Set(width, 0f);
			barBackground.Height.Set(height, 0f);

			barAmount = new VoidBarSprite(ModContent.Request<Texture2D>("SOTS/Void/VoidBarSprite").Value); 
			barAmount.SetPadding(0);
			barAmount.Left.Set(5.01f, 0f);
			barAmount.Top.Set(6f, 0f);
			barAmount.Width.Set(188, 0f);
			barAmount.Height.Set(18, 0f);

			text = new UIText("0|0"); 
			text.Width.Set(width, 0f);
			text.Height.Set(height, 0f);
			text.Top.Set(height - 42 - text.MinHeight.Pixels / 2, 0f);

			barBackground.Append(barAmount);
			barBackground.Append(text);
			base.Append(barBackground);
		}
		public Vector2 flipOrigin => new Vector2(SOTS.Config.voidBarPointX + width / 2, SOTS.Config.voidBarPointY + height / 2);
		public override void Draw(SpriteBatch spriteBatch)
		{
			Player player = Main.player[Main.myPlayer];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			DrawIce(spriteBatch, true);
			int voidmeter = (int)voidPlayer.voidMeter;
			if(voidmeter < 0)
			{
				voidmeter = 0;
			}
			if (voidPlayer.frozenVoid)
			{
				voidmeter = (int)voidPlayer.frozenVoidCount;
			}
			string voidManaText = voidmeter.ToString();
			int voidMax = voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption;
			string voidManaMaxText = voidMax.ToString();
			string voidSoulsText = voidPlayer.lootingSouls.ToString();
			Color soulColor = VoidPlayer.soulLootingColor;
			if (text != null)
			{
				if (player.dead)
					voidManaText = "0 ";
				string textStart = "Void: ";
				string textThing = "";
				if (voidMax - voidPlayer.lootingSouls <= 0)
				{
					textThing += "0 ";
				}
				else
				{
					voidManaMaxText = (voidMax - voidPlayer.lootingSouls).ToString();
					textThing += voidManaText + "/" + voidManaMaxText;
				}
				if (!player.dead && (voidPlayer.lootingSouls > 0 || (voidPlayer.VoidMinionConsumption > 0 && !SOTS.Config.simpleVoidText)))
				{
					textThing += " (";
					if (voidPlayer.VoidMinionConsumption > 0 && !SOTS.Config.simpleVoidText)
						textThing += voidPlayer.VoidMinionConsumption.ToString();
					if (voidPlayer.lootingSouls > 0 && voidPlayer.VoidMinionConsumption > 0 && !SOTS.Config.simpleVoidText)
						textThing += " + ";
					if (voidPlayer.lootingSouls > 0)
						textThing += voidSoulsText;
					textThing += ")";
				}
				if (SOTS.Config.voidBarTextOn)
                {
					text.SetText(textStart + textThing);
				}
				else
				{
					text.SetText("");
				}
				if (SOTS.Config.voidBarHoverTextOn && this.ContainsPoint(Main.MouseScreen))
                {
					if(!Main.LocalPlayer.cursorItemIconEnabled)
					{
						Main.mouseText = true;

						/*string[] twoString = textThing.Split(" / ");
						textThing = twoString[0] + "/" + twoString[1]; //remove spaces for consistency with vanilla bars*/

						Main.hoverItemName = textThing;
					}
				}
			}
			//Calculate quotient
			float quotient = voidPlayer.voidMeter / voidPlayer.voidMeterMax2;
			float quotient2 = (float)voidPlayer.lootingSouls / voidPlayer.voidMeterMax2;
			Texture2D fill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/SoulBar");
			Texture2D divider = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarDivider");
			if (quotient > 1)
			{
				quotient = 1;
			}
			if (quotient < 0)
				quotient = 0;
			Vector2 padding = new Vector2(6, 6);
			float prevRight = 0;
			float length = 40;
			int height = 18;
			Texture2D fill2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarBorder");
			if (SOTS.Config.alternateVoidBarStyle)
				fill2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarBorderAlt");
			if (player.dead)
			{
				Recalculate();
				base.Draw(spriteBatch);
				fill2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidDead");
				if(SOTS.Config.alternateVoidBarStyle)
					fill2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidDeadAlt");
			}
			Texture2D textureBot = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarAltBottom");
			spriteBatch.Draw(fill2, new Rectangle((int)SOTS.Config.voidBarPointX, (int)SOTS.Config.voidBarPointY + 2, 200, 30).FlipHorizontal(flipOrigin), Color.White);
			if (SOTS.Config.alternateVoidBarStyle)
			{
				if (SOTS.Config.alternateVoidBarDirection)
					textureBot = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarAltBottomFlipped");
				spriteBatch.Draw(textureBot, new Rectangle(SOTS.Config.voidBarPointX - 6,  SOTS.Config.voidBarPointY, 206, 32).FlipHorizontal(flipOrigin), Color.White);
			}
			if (!player.dead)
            {
				List<Rectangle> rectangles = new List<Rectangle>();
				//List<Color> colors = new List<Color>();
				int minionCount = voidPlayer.VoidMinions.Count;
				for (int i = 0; i < minionCount; i++)
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
					spriteBatch.Draw(fill, new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y), (int)(length + 1), height).FlipHorizontal(flipOrigin), color);
					color.A = 0;
					spriteBatch.Draw(fill, new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y), (int)(length + 1), height).FlipHorizontal(flipOrigin), color);
					/*if(i == 0)
					{
						rectangles.Add(new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y - 2), 6, 22));
						colors.Add(color);
					}*/
					prevRight += length;
					if (!SOTS.Config.simpleVoidFill)
						if (nextType == -1 || nextType != type)
						{
							rectangles.Add(new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight - 1), (int)(SOTS.Config.voidBarPointY + padding.Y - 2), 2, 20));
							//colors.Add(color);
						}
				}
				barAmount.Left.Set(6f + prevRight, 0f);
				length = (int)(quotient * 188);
				if (length + prevRight > 188)
					length = 188 - prevRight;
				barAmount.Width.Set(length, 0f);
				Vector2 soulBarOffset = Vector2.Zero;
				int soulBarWidth = 0;
				if(voidPlayer.lootingSouls > 0)
				{
					soulBarWidth = (int)(quotient2 * 188f);
					soulBarOffset.X = 200f - quotient2 * 188 - 6;
				}
				Recalculate();
				if (voidPlayer.lerpingVoidMeter > voidPlayer.voidMeter && voidPlayer.lerpingVoidMeter < voidPlayer.voidMeterMax2) //this draws the red part of the sprite
				{
					float quotientLerp = voidPlayer.lerpingVoidMeter / voidPlayer.voidMeterMax2;
					fill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarSprite");
					float lerpLength = (int)(quotientLerp * 188);
					spriteBatch.Draw(fill, new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y), (int)lerpLength, height).FlipHorizontal(flipOrigin), new Color(255, 10, 10, 0) * 0.6f);
				}
				//spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarDark"), new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y), (int)length, height), new Color(255, 255, 255));
				base.Draw(spriteBatch);
				fill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarSprite");
				spriteBatch.Draw(fill, new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y), (int)length, height).FlipHorizontal(flipOrigin), Color.White);
				if (!SOTS.Config.simpleVoidFill)
				{
					for (int i = 0; i < rectangles.Count; i++)
					{
						spriteBatch.Draw(divider, rectangles[i].FlipHorizontal(flipOrigin), Color.White);
					}
				}
				if(soulBarWidth > 0)
				{
					Texture2D soulFill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/SoulBar");
					spriteBatch.Draw(soulFill, new Rectangle((int)(SOTS.Config.voidBarPointX + soulBarOffset.X), (int)(SOTS.Config.voidBarPointY + soulBarOffset.Y + padding.Y), soulBarWidth + 2, height).FlipHorizontal(flipOrigin), soulColor * 1.5f);
					spriteBatch.Draw(divider, new Rectangle((int)(SOTS.Config.voidBarPointX + soulBarOffset.X - 2), (int)(SOTS.Config.voidBarPointY + soulBarOffset.Y + padding.Y), 2, 20).FlipHorizontal(flipOrigin), Color.White);
				}

				fill2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarBorder2");
				spriteBatch.Draw(fill2, new Rectangle((int)SOTS.Config.voidBarPointX, (int)SOTS.Config.voidBarPointY, 200, 30).FlipHorizontal(flipOrigin), Color.White);
				DrawGreenBar(spriteBatch);
				if (voidPlayer.resolveVoidCounter > 0) //resolve visuals
				{
					float resolveAmount = 188f * voidPlayer.resolveVoidAmount / voidPlayer.voidMeterMax2;
					if (voidPlayer.voidRecovery)
						resolveAmount = length;
					else
					{
						if (resolveAmount > length)
						{
							resolveAmount = length;
						}
					}
					prevRight += length - resolveAmount;
					float mult = voidPlayer.resolveVoidCounter / 15f;
					for (int i = -1; i <= 1; i += 2)
					{
						float sinY = i * 4 * (0.5f * (float)Math.Sin(mult * MathHelper.Pi) + mult * 0.5f);
						spriteBatch.Draw(fill, new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X + (int)prevRight), (int)(SOTS.Config.voidBarPointY + padding.Y + sinY), (int)(resolveAmount + 1), height).FlipHorizontal(flipOrigin), new Color(110, 80, 110, 0) * mult);
					}
					voidPlayer.resolveVoidCounter--;
					if (voidPlayer.resolveVoidCounter <= 0)
						voidPlayer.resolveVoidAmount = 0;
				}
				if (voidPlayer.safetySwitchVisual)
					DrawLock(spriteBatch);
				DrawIce(spriteBatch, false);
				DrawShock(spriteBatch);
			}
			if (SOTS.Config.alternateVoidBarStyle)
				DrawCrescent(spriteBatch);
		}
		public void DrawGreenBar(SpriteBatch spriteBatch)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Main.LocalPlayer);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarGreen");
			int timerDelay = 90;
			float regenBarPercent = (voidPlayer.voidRegenTimer - timerDelay) / (VoidPlayer.voidRegenTimerMax - timerDelay);
			int Xoffset = 16;
			if (SOTS.Config.alternateVoidBarStyle)
				Xoffset = 0;
			if(regenBarPercent > 0)
			{
				Rectangle frame1 = new Rectangle((int)(SOTS.Config.voidBarPointX + Xoffset), (int)(SOTS.Config.voidBarPointY + 2), (int)(168 * regenBarPercent), 30);
				Rectangle frame = new Rectangle(0, 0, (int)(168 * regenBarPercent), 30);
				spriteBatch.Draw(texture, frame1.FlipHorizontal(flipOrigin), frame, Color.White, 0f, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			if (SOTS.Config.alternateVoidBarStyle && SOTS.Config.alternateVoidBarDirection)
				Xoffset = 32;
			if (voidPlayer.GreenBarCounter > 12 || voidPlayer.voidRecovery)
			{
				float mult = (voidPlayer.GreenBarCounter - 12) / 8f;
				if (voidPlayer.voidRecovery)
					mult = 1f;
				Rectangle frame1 = new Rectangle((int)(SOTS.Config.voidBarPointX + Xoffset), (int)(SOTS.Config.voidBarPointY + 2), 168, 30);
				spriteBatch.Draw(texture, frame1, Color.White * mult);
			}
			if (voidPlayer.voidRecovery && voidPlayer.GreenBarCounter < 20 && voidPlayer.GreenBarCounter != 0)
			{
				float mult = voidPlayer.GreenBarCounter / 20f;
				float reverseMult = 1 - mult;
				float sinusoid = (float)Math.Sin(Math.PI * reverseMult);
				Rectangle frame1 = new Rectangle(0, 0, 168, 30);
				Color glow = new Color(60, 60, 60, 0);
				for (int i = 0; i < 6; i++)
				{
					Vector2 circular = new Vector2(sinusoid * 1.5f, 0).RotatedBy(MathHelper.ToRadians(i * 60));
					spriteBatch.Draw(texture, new Vector2((int)(SOTS.Config.voidBarPointX + Xoffset) + circular.X, (int)(SOTS.Config.voidBarPointY + 2) + circular.Y), frame1, glow * (0.5f * sinusoid + 0.5f * mult));
				}
			}
			else if (voidPlayer.GreenBarCounter < 20 && voidPlayer.GreenBarCounter != 0)
			{
				float mult = voidPlayer.GreenBarCounter / 20f;
				float reverseMult = 1 - mult;
				float sinusoid = (float)Math.Sin(Math.PI * reverseMult);
				Color glow = new Color(120, 120, 120, 0);
				Rectangle frame1 = new Rectangle(0, 0, 168, 30);
				for (int i = 0; i < 6; i++)
				{
					Vector2 circular = new Vector2(sinusoid * 3, 0).RotatedBy(MathHelper.ToRadians(i * 60));
					spriteBatch.Draw(texture, new Vector2((int)(SOTS.Config.voidBarPointX + Xoffset) + circular.X, (int)(SOTS.Config.voidBarPointY + 2) + circular.Y), frame1, glow * (0.5f * sinusoid + 0.5f * mult));
				}
			}
			if (voidPlayer.GreenBarCounter > 0)
				voidPlayer.GreenBarCounter--;
		}
		public void DrawShock(SpriteBatch spriteBatch)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(Main.LocalPlayer);
			if (voidPlayer.voidShockAnimationCounter > 0)
			{
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidShockEffect");
				int frameNumber = (int)(voidPlayer.voidShockAnimationCounter / 3.6f);
				if (frameNumber >= 11)
				{
					voidPlayer.voidShockAnimationCounter = 0;
					return;
				}
				Rectangle frame1 = new Rectangle((int)(SOTS.Config.voidBarPointX - 10), (int)(SOTS.Config.voidBarPointY - 4), 216, 40);
				Rectangle frame = new Rectangle(0, 40 * frameNumber, 216, 40);
				spriteBatch.Draw(texture, frame1.FlipHorizontal(flipOrigin), frame, Color.White, 0, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				voidPlayer.voidShockAnimationCounter++;
			}
		}
		public void DrawLock(SpriteBatch spriteBatch)
		{
			Texture2D Lock = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarLock");
			if(SOTS.Config.alternateVoidBarDirection)
				Lock = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarLockFlipped");
			spriteBatch.Draw(Lock, new Rectangle(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY, Lock.Width, Lock.Height).FlipHorizontal(flipOrigin), new Color(255, 255, 255));
		}
		public void DrawCrescent(SpriteBatch spriteBatch)
		{
			Texture2D Emblem = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/VoidBarEmblem");
			spriteBatch.Draw(Emblem, new Rectangle(SOTS.Config.voidBarPointX - 24, SOTS.Config.voidBarPointY, Emblem.Width, Emblem.Height).FlipHorizontal(flipOrigin), new Color(255, 255, 255));
		}
		public void DrawIce(SpriteBatch spriteBatch, bool shadow = false)
		{
			Player player = Main.player[Main.myPlayer];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			Texture2D frozenBar = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/FrozenVoidBar");
			Texture2D fill = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/FrozenVoidBarFill");
			Texture2D frozenBarBorder = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/FrozenVoidBarBorder");
			Texture2D LockFrozen = (Texture2D)ModContent.Request<Texture2D>("SOTS/Void/FrozenVoidBarLock");
			Vector2 padding = new Vector2(-2, 0);
			Rectangle frame = new Rectangle((int)(SOTS.Config.voidBarPointX + padding.X), (int)(SOTS.Config.voidBarPointY + padding.Y), frozenBar.Width, frozenBar.Height);
			if (voidPlayer.frozenVoid)
			{
				if (!shadow && voidPlayer.frozenDuration >= 30)
				{
					float frozenPercent2 = (float)voidPlayer.frozenDuration / (float)voidPlayer.frozenMaxDuration;
					spriteBatch.Draw(fill, frame, new Color(255, 255, 255) * 0.1f);
					spriteBatch.Draw(frozenBarBorder, new Rectangle(SOTS.Config.voidBarPointX - 2, SOTS.Config.voidBarPointY, frozenBarBorder.Width, frozenBarBorder.Height), new Rectangle(0, 0, frozenBarBorder.Width, frozenBarBorder.Height), new Color(255, 255, 255), 0, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					Rectangle destRect = new Rectangle(SOTS.Config.voidBarPointX - 2, SOTS.Config.voidBarPointY, (int)(frozenBar.Width * frozenPercent2), frozenBar.Height);
					spriteBatch.Draw(frozenBar, destRect.FlipHorizontal(flipOrigin), new Rectangle(0, 0, (int)(frozenBar.Width * frozenPercent2), frozenBar.Height), new Color(255, 255, 255), 0f, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					if (voidPlayer.safetySwitchVisual)
					{
						spriteBatch.Draw(LockFrozen, new Rectangle(SOTS.Config.voidBarPointX - 2, SOTS.Config.voidBarPointY, LockFrozen.Width, LockFrozen.Height).FlipHorizontal(flipOrigin), new Rectangle(0, 0, LockFrozen.Width, LockFrozen.Height), new Color(255, 255, 255), 0, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
				else if (shadow && voidPlayer.frozenDuration < 30)
					for (int i = 0; i < 6; i++)
					{
						float distance = 30 - voidPlayer.frozenDuration;
						float alphaMult = (40 - distance) / 40f;
						Vector2 circular = new Vector2(1.0f * distance, 0).RotatedBy(MathHelper.ToRadians(2 * voidPlayer.frozenDuration + 60 * i));
						spriteBatch.Draw(fill, new Vector2(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY) + circular + padding, new Rectangle(0, 0, fill.Width, fill.Height), new Color(100, 100, 100, 0) * 0.05f * alphaMult);
						spriteBatch.Draw(frozenBarBorder, new Vector2(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY) + circular + padding, new Rectangle(0, 0, frozenBarBorder.Width, frozenBarBorder.Height), new Color(100, 100, 100, 0) * 0.5f * alphaMult);
					}
			}
			else
			{
				if (!shadow)
				{
					float frozenPercent = (float)voidPlayer.frozenCounter / (float)voidPlayer.frozenMinTimer;
					frame = new Rectangle(0, 0, (int)(frozenBarBorder.Width * frozenPercent), frozenBarBorder.Height);
					Rectangle destRect = new Rectangle(SOTS.Config.voidBarPointX - 2, SOTS.Config.voidBarPointY, (int)(frozenBarBorder.Width * frozenPercent), frozenBarBorder.Height);
					spriteBatch.Draw(frozenBarBorder, destRect.FlipHorizontal(flipOrigin), frame, new Color(255, 255, 255), 0, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					if (voidPlayer.safetySwitchVisual)
						spriteBatch.Draw(LockFrozen, destRect.FlipHorizontal(flipOrigin), frame, new Color(255, 255, 255), 0, Vector2.Zero, SOTS.Config.alternateVoidBarDirection ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				else if (voidPlayer.frozenCounter > voidPlayer.frozenMinTimer - 30)
					for (int i = 0; i < 6; i++)
					{
						float distance = voidPlayer.frozenMinTimer - voidPlayer.frozenCounter;
						float alphaMult = (40 - distance) / 40f;
						Vector2 circular = new Vector2(1.0f * distance, 0).RotatedBy(MathHelper.ToRadians(2 * voidPlayer.frozenCounter + 60 * i));
						spriteBatch.Draw(fill, new Vector2(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY) + circular + padding, new Rectangle(0, 0, fill.Width, fill.Height), new Color(100, 100, 100, 0) * 0.05f * alphaMult);
						spriteBatch.Draw(frozenBar, new Vector2(SOTS.Config.voidBarPointX, SOTS.Config.voidBarPointY) + circular + padding, new Rectangle(0, 0, frozenBar.Width, frozenBar.Height), new Color(100, 100, 100, 0) * 0.5f * alphaMult);
					}
			}
		}
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}