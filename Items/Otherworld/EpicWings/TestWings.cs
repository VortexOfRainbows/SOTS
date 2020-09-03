using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.EpicWings
{
	[AutoloadEquip(EquipType.Wings)]
	public class TestWings : ModItem
	{ 
		public override void SetStaticDefaults()
		{	
			DisplayName.SetDefault("Test Wings");
			Tooltip.SetDefault("Double tap space to fly like creative mode");
		}

		public override void SetDefaults()
		{
			item.width = 78;
			item.height = 56;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Red;
			item.expert = true;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			TestWingsPlayer testWingsPlayer = (TestWingsPlayer)player.GetModPlayer(mod, "TestWingsPlayer");
			testWingsPlayer.canCreativeFlight = true;
			player.wingTimeMax = 120;
			player.noFallDmg = true;
		}
		public override bool WingUpdate(Player player, bool inUse)
		{
			TestWingsPlayer testWingsPlayer = (TestWingsPlayer)player.GetModPlayer(mod, "TestWingsPlayer");
			if(testWingsPlayer.creativeFlight)
			{
				player.wingFrame = 2;
			}
			else if (player.controlJump && player.velocity.Y != 0f)
			{
				player.wingFrame = 1;
			}
			else
			{
				player.wingFrame = 0;
			}
			return true;
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			float num2 = 0.5f;
			float num5 = 0.1f;
			float num4 = 0.5f;
			float num3 = 1.5f;
			float num1 = 0.1f;
			ascentWhenFalling = num2;
			ascentWhenRising = num5;
			maxCanAscendMultiplier = num4;
			maxAscentMultiplier = num3;
			constantAscend = num1;
		}
		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 6f;
			acceleration *= 1.5f;
		}
	}
	public class TestWingsPlayer : ModPlayer
	{
		public bool canCreativeFlight = false;
		public bool creativeFlight = false;
		int timer = 0;
		bool release = false;
		public float wingSpeed = 7f;
		public int epicWingType = 1;
		public enum EpicWingType : int
		{
			Default = 0,
			Blocky = 1,
		}
		public void DustExplosion()
		{
			for (int i = 0; i < 360; i += 4)
			{
				int index = Dust.NewDust(player.Center + new Vector2(-5, -5), 0, 0, mod.DustType("CubeDust"));
				Dust dust = Main.dust[index];
				dust.noGravity = true;
				dust.fadeIn = 0.5f;
				dust.velocity *= 0.8f;
				dust.scale = 1.5f;
				dust.velocity += new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i));
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
			}
			Main.PlaySound(12, player.Center);
		}
		public void flightStart()
		{
			timer--;
			if (timer > 0 && !release && player.controlJump && player.velocity.Y != 0f)
			{
				timer = 0;
				creativeFlight = !creativeFlight;
				DustExplosion();
			}
			if(!player.controlJump)
				release = false;
			if (player.controlJump && !release)
			{
				release = true;
				timer = 15;
			}
		}
		bool movingHori = false;
		bool movingVert = false;
		public override void PostUpdate()
		{
			if(creativeFlight)
				if (player.bodyFrame.Y == player.bodyFrame.Height * 5)
					player.bodyFrame.Y = player.bodyFrame.Height * 6;
		}
		public void flight()
		{
			player.noFallDmg = true;
			player.wingFrameCounter++;
			player.maxFallSpeed *= 20f;
			player.gravity = 0;
			player.wingTime = 0;
			if (player.controlDown)
			{
				movingVert = true;
				float toBeAdded = wingSpeed / 3;
				if (player.velocity.Y + toBeAdded < wingSpeed * 1.5f)
				{
					player.velocity.Y += toBeAdded;
				}
				else if(player.velocity.Y < wingSpeed * 1.5f)
				{
					player.velocity.Y = wingSpeed * 1.5f;
				}
			}
			if (player.controlLeft && player.dashDelay >= 0)
			{
				movingHori = true;
				float toBeAdded = -wingSpeed / 3;
				if (player.velocity.X + toBeAdded > -wingSpeed * 1.5f)
				{
					player.velocity.X += toBeAdded;
				}
				else if (player.velocity.X > -wingSpeed * 1.5f)
				{
					player.velocity.X = -wingSpeed * 1.5f;
				}
			}
			if (player.controlRight && player.dashDelay >= 0)
			{
				movingHori = true;
				float toBeAdded = wingSpeed / 3;
				if (player.velocity.X + toBeAdded < wingSpeed * 1.5f)
				{
					player.velocity.X += toBeAdded;
				}
				else if (player.velocity.X < wingSpeed * 1.5f)
				{
					player.velocity.X = wingSpeed * 1.5f;
				}
			}
			if (player.controlUp || player.controlJump)
			{
				movingVert = true;
				float toBeAdded = -wingSpeed / 3;
				if (player.velocity.Y + toBeAdded > -wingSpeed * 1.5f)
				{
					player.velocity.Y += toBeAdded;
				}
				else if (player.velocity.Y > -wingSpeed * 1.5f)
				{
					player.velocity.Y = -wingSpeed * 1.5f;
				}
			}
			if (!movingVert)
			{
				player.velocity.Y *= 0.8f;
			}
			if (!movingHori && player.dashDelay >= 0)
			{
				player.velocity.X *= 0.8f;
			}
			int i = (int)player.Center.X / 16;
			int j = (int)(player.position.Y + player.height + 2) / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			if (tile.active() && Main.tileSolid[tile.type])
			{
				creativeFlight = false;
			}
			else
			{
				if (player.velocity.Y == 0f)
					player.velocity.Y = -0.04f;
			}
			movingHori = false;
			movingVert = false;
		}
		public override void ResetEffects()
		{
			if (player.gravDir != 1)
				canCreativeFlight = false;
			if (player.mount.Active || !canCreativeFlight)
			{
				canCreativeFlight = false;
				wingSpeed = 6f;
				creativeFlight = false;
				return;
			}
			if (canCreativeFlight)
				flightStart();
			if (creativeFlight)
			{
				flight();
			}
			canCreativeFlight = false;
			wingSpeed = 6f;
		}
		public static readonly PlayerLayer TestWingsVisual = new PlayerLayer("SOTS", "TestWingsVisual", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			// drawInfo.shadow != 0f || 
			if (drawInfo.drawPlayer.dead || drawInfo.drawPlayer.mount.Active)
			{
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");
			TestWingsPlayer testWingsPlayer = (TestWingsPlayer)drawPlayer.GetModPlayer(mod, "TestWingsPlayer");

			if (drawPlayer.wings != mod.GetEquipSlot("TestWings", EquipType.Wings))
			{
				return;
			}

			//Texture2D texture = mod.GetTexture("Items/Otherworld/EpicWings/TestWings_Wings_Glow");
			Texture2D smallPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingPart1");
			Texture2D bigPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingPart2");
			Texture2D mediumPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingPart3");
			Texture2D boosterPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingBooster1");
			switch (testWingsPlayer.epicWingType)
			{
				case (int)EpicWingType.Blocky:
					break;
				case (int)EpicWingType.Default:
					smallPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingPart1");
					bigPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingPart2");
					mediumPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingPart3");
					boosterPiece = mod.GetTexture("Items/Otherworld/EpicWings/WingBooster1");
					break;
			}

			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			//drawX -= 9 * drawPlayer.direction;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height / 2;
			//drawY += 2 * drawPlayer.gravDir;


			Vector2 position = new Vector2(drawX, drawY) - Main.screenPosition;
			float alpha = 1 - drawInfo.shadow;
			Color color = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX/16, (int)drawY /16)); //apply lighting to wings
			//Rectangle frame = new Rectangle(0, Main.wingsTexture[drawPlayer.wings].Height / 4 * drawPlayer.wingFrame, Main.wingsTexture[drawPlayer.wings].Width, Main.wingsTexture[drawPlayer.wings].Height / 4);
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;

			int mode = drawPlayer.wingFrame;
			List<DrawData> drawData = new List<DrawData>();
			List<DrawData> drawData2 = new List<DrawData>();
			for (int j = 0; j < 2; j++)
			{
				int direction = (j * 2) - 1;
				direction *= -drawPlayer.direction;
				float scale = 7 - j;
				scale /= 7f;
				Vector2 currentPos = position;
				currentPos.X -= direction + drawPlayer.direction;
				currentPos.Y -= 2 * drawPlayer.gravDir;
				if (mode == 0)
				{
					currentPos.X -= direction * 4;
				}
				for (int i = 0; i < 10; i++)
				{
					float rotationI = MathHelper.ToRadians((i - 2) * -(7.5f + (direction == -1 ? 0.5f : 0)) * direction * drawPlayer.gravDir);
					if(mode == 0)
					{
						rotationI = MathHelper.ToRadians((64 + (direction == -1 ? 4f : 0)) * direction * drawPlayer.gravDir);
					}
					Vector2 fromPlayer = new Vector2(-6 * direction * scale, 0).RotatedBy(rotation - rotationI);
					if (mode == 0)
					{
						fromPlayer = new Vector2((-1f -1f * scale) * direction , 0).RotatedBy(rotation - rotationI);
					}
					currentPos += fromPlayer;
					Texture2D currentTexture = smallPiece;

					if (i == 0)
						currentTexture = mediumPiece;
					else if (i % 3 == 0)
						currentTexture = bigPiece;

					Vector2 origin = new Vector2(currentTexture.Width / 2, currentTexture.Height / 2);
					if (currentTexture == bigPiece)
					{
						origin = new Vector2(currentTexture.Width / 2, 8);
						if (drawPlayer.gravDir == -1)
						{
							origin = new Vector2(currentTexture.Width / 2, currentTexture.Height - 8);
						}
					}
					if (testWingsPlayer.epicWingType == (int)EpicWingType.Blocky && i % 3 == 0 && mode == 0)
					{
						rotationI += MathHelper.ToRadians(90 * direction * drawPlayer.gravDir);
					}
					bool booster = i != 0 && i % 3 == 0 && drawPlayer.wingFrame == 2;
					if(booster)
					{
						currentTexture = boosterPiece;
						Vector2 rotationOrigin = new Vector2(-2.75f * direction, 6f) - drawPlayer.velocity;
						Vector2 currentPos2 = currentPos + Main.screenPosition;
						float overrideRotation = rotationOrigin.ToRotation();
						Vector2 dustVelo = new Vector2(8, 0).RotatedBy(overrideRotation);
						if (drawPlayer.shadow == 0f)
						{
							int index = Dust.NewDust(currentPos2 + dustVelo * 1.5f + new Vector2(-5, -5), 0, 0, mod.DustType("CubeDust"));
							Dust dust = Main.dust[index];
							dust.noGravity = true;
							dust.fadeIn = 0.5f;
							dust.velocity *= 0.8f;
							dust.velocity += dustVelo;
							dust.scale *= scale;
							dust.shader = GameShaders.Armor.GetSecondaryShader(drawInfo.wingShader, drawPlayer);
							Main.playerDrawDust.Add(index);
						}

						DrawData data = (new DrawData(currentTexture, currentPos, null, color * alpha, rotation + overrideRotation - MathHelper.ToRadians(90), origin, scale, spriteEffects, 0));
						data.shader = drawInfo.wingShader;
						drawData.Add(data);
					}
					else
					{
						DrawData data = (new DrawData(currentTexture, currentPos, null, color * alpha, rotation - rotationI, origin, scale, spriteEffects, 0));
						data.shader = drawInfo.wingShader;
						drawData.Add(data);
					}
				}
			}


			for (int i = 0; i < drawData.Count; i++)
				if ((i % 10) % 3 != 0 && mode != 0)
					Main.playerDrawData.Add(drawData[i]);
			for (int i = 0; i < drawData.Count; i++) //add the more important layers on so they don't have weired overlap
				if ((i % 10) % 3 == 0 || mode == 0)
					Main.playerDrawData.Add(drawData[i]);

			// Here we generate visual dust while our glowmask is visible
			if (Main.rand.NextBool(10))
			{
				/*
				int index = Dust.NewDust(drawPlayer.position, drawPlayer.width, drawPlayer.height, 135);
				Dust dust = Main.dust[index];
				dust.noGravity = true;
				dust.noLight = true;
				dust.fadeIn = Main.rand.NextFloat(0.5f, 0.8f);
				dust.shader = GameShaders.Armor.GetSecondaryShader(drawInfo.wingShader, drawPlayer);
				Main.playerDrawDust.Add(index);
				*/
			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int wingLayer = layers.FindIndex(l => l == PlayerLayer.Wings);

			if (wingLayer > -1)
			{
				layers.Insert(wingLayer + 1, TestWingsVisual);
			}
		}
	}
}