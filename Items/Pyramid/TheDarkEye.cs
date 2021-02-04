using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS;
using SOTS.Void;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using static SOTS.SOTS;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Pyramid
{
	public class TheDarkEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows the ability to perform an instant dash to the left or right at the cost of 4 void\nAlso allows the ability to dash through walls\nDashing into enemies will strike them, but consumes more void");
		}
		Vector2 toPos = new Vector2(3.75f, 0);
		Vector2 CurrentPos = new Vector2(0, 0);
		int waitTime = 0;
		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 40;
			item.accessory = true;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 4, 0, 0);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Pyramid/TheDarkEyeEmpty");
			Vector2 origin2 = new Vector2(5, 5);
			if (CurrentPos.X != toPos.X || CurrentPos.Y != toPos.Y)
			{
				Vector2 to = toPos - CurrentPos;
				to.Normalize();
				to *= 0.15f;
				CurrentPos += to;
				if ((toPos - CurrentPos).Length() < 0.2f)
				{
					CurrentPos = new Vector2(toPos.X, toPos.Y);
				}
			}
			else if (waitTime < 0)
			{
				waitTime = 30;
				toPos = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
				toPos.Y /= 2.2f;
			}
			else
				waitTime--;
			Texture2D texture2 = mod.GetTexture("Items/Pyramid/TheDarkEyePupil");
			spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture2, position + new Vector2(19 * scale, 20 * scale) + CurrentPos * scale, null, drawColor, 0, origin2, scale, SpriteEffects.None, 1f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Pyramid/TheDarkEyeEmpty");
			Vector2 origin = new Vector2(19, 20);
			Vector2 origin2 = new Vector2(5, 5);
			if (CurrentPos.X != toPos.X || CurrentPos.Y != toPos.Y)
			{
				Vector2 to = toPos - CurrentPos;
				to.Normalize();
				to *= 0.15f;
				CurrentPos += to;
				if ((toPos - CurrentPos).Length() < 0.2f)
				{
					CurrentPos = new Vector2(toPos.X, toPos.Y);
				}
			}
			else if (waitTime < 0)
			{
				waitTime = 30;
				toPos = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
				toPos.Y /= 2.2f;
			}
			else
				waitTime--;
			Texture2D texture2 = mod.GetTexture("Items/Pyramid/TheDarkEyePupil");
			spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, lightColor, 0, origin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture2, item.position + new Vector2(19 * scale, 20 * scale) - Main.screenPosition + CurrentPos * scale, null, lightColor, 0, origin2, scale, SpriteEffects.None, 1f);
			return false;
		}
		int rotation = 0;
		int alpha1 = 0;
		int alpha2 = 0;
		Vector2 npcPosStore1;
		Vector2 npcPosStore2;
		public static Vector2 StaticDrawDetect(Mod mod, Vector2 player, int i, int alpha1, int alpha2, int rotation, ref Vector2 toVelo, bool final = false, Player player2 = null)
		{
			Vector2 npcPosStore1 = player;
			Vector2 npcPosStore2 = player;
			for (int u = 0; u < 360; u += 10)
			{
				Vector2 circularRotation = new Vector2(-16, 0).RotatedBy(MathHelper.ToRadians(u));
				int num = Dust.NewDust(new Vector2(player.X - 5 + circularRotation.X, player.Y - 6 + circularRotation.Y), 0, 0, 21);
				Main.dust[num].velocity *= 1.5f;
				Main.dust[num].scale = 2.1f;
				Main.dust[num].noGravity = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player2).darkEyeShader, player2);
			}
			Vector2 finalPos = player;
			float dX;
			float dY;
			float distance;
			float minDist = 600;
			toVelo = new Vector2(i * 16, 0);
			Vector2 npcPos = player;
			Vector2 npcCheckPos = new Vector2(i * 180, 0);
			npcCheckPos += player;
			for (int k = 0; k < Main.npc.Length; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy() && npc.active)
				{
					dX = npc.Center.X - player.X;
					float dX1 = dX;
					dY = npc.Center.Y - player.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					dX = npc.Center.X - npcCheckPos.X;
					dY = npc.Center.Y - npcCheckPos.Y;
					float distance2 = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance < minDist && distance2 < 160 && Math.Abs(dX1) > 64)
					{
						minDist = distance;
						npcPos = npc.Center;
						if (npcPos.Y > player.Y && npcPos.Y - 24 < player.Y)
							npcPos.Y = player.Y;
					}
				}
			}
			float steepening = 0.1f;
			for (int j = 0; j < 160; j++)
			{
				if (npcPos != player)
				{
					dX = npcPos.X - finalPos.X;
					dY = npcPos.Y - finalPos.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance <= 8 || j == 158)
					{
						if (i == -1)
							npcPosStore1 = npcPos;
						if (i == 1)
							npcPosStore2 = npcPos;
						npcPos = player;
					}
					float radians = new Vector2(dX, dY).ToRotation();
					if (distance <= 120)
					{
						toVelo += new Vector2(1f + steepening, 0).RotatedBy(radians);
						steepening += 0.1f;
					}
				}
				toVelo.Normalize();
				finalPos += toVelo * 2;
				Vector2 fluctuation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(rotation));
				Vector2 rotlength = new Vector2(fluctuation.X, 0).RotatedBy(MathHelper.ToRadians(rotation + j * 9));
				Vector2 circularLocation = new Vector2(0, rotlength.X).RotatedBy(toVelo.ToRotation());

				if (i == -1)
				{
					if (j > 12)
						dust(mod, finalPos + circularLocation, i, alpha1, alpha2, final, player2.whoAmI);
					if (j == 159)
					{
						finalPos += toVelo * 12;
						for (int u = 0; u < 360; u += 10)
						{
							Vector2 circularRotation = new Vector2(-16, 0).RotatedBy(MathHelper.ToRadians(u));
							if (final)
							{
								int num = Dust.NewDust(new Vector2(finalPos.X - 5 + circularRotation.X, finalPos.Y - 6 + circularRotation.Y), 0, 0, 21);
								Main.dust[num].velocity *= 1.5f;
								Main.dust[num].scale = 2.1f;
								Main.dust[num].noGravity = true;
								Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player2).darkEyeShader, player2);
							}
						}
					}
				}
				if (i == 1)
				{
					if (j > 12)
						dust(mod, finalPos + circularLocation, i, alpha1, alpha2, final, player2.whoAmI);
					if (j == 159)
					{
						finalPos += toVelo * 12;
						for (int u = 0; u < 360; u += 10)
						{
							Vector2 circularRotation = new Vector2(-16, 0).RotatedBy(MathHelper.ToRadians(u));
							if (final)
							{
								int num = Dust.NewDust(new Vector2(finalPos.X - 5 + circularRotation.X, finalPos.Y - 6 + circularRotation.Y), 0, 0, 21);
								Main.dust[num].velocity *= 1.5f;
								Main.dust[num].scale = 2.1f;
								Main.dust[num].noGravity = true;
								Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player2).darkEyeShader, player2);
							}
						}
					}
				}
				toVelo *= 16;
			}
			return finalPos;
		}
		public Vector2 DrawDetect(Player player, int i, bool final = false)
		{
			Vector2 finalPos = player.Center;
			float dX;
			float dY;
			float distance;
			float minDist = 600;
			Vector2 toVelo = new Vector2(i * 16, 0);
			Vector2 npcPos = player.Center;
			Vector2 npcCheckPos = new Vector2(i * 180, 0);
			npcCheckPos += player.Center;
			for (int k = 0; k < Main.npc.Length; k++)
			{
				NPC npc = Main.npc[k];
				if (npc.CanBeChasedBy() && npc.active)
				{
					dX = npc.Center.X - player.Center.X;
					float dX1 = dX;
					dY = npc.Center.Y - player.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					dX = npc.Center.X - npcCheckPos.X;
					dY = npc.Center.Y - npcCheckPos.Y;
					float distance2 = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance < minDist && distance2 < 160 && Math.Abs(dX1) > 64)
					{
						minDist = distance;
						npcPos = npc.Center;
						if (npcPos.Y > player.Center.Y && npcPos.Y - 24 < player.Center.Y)
							npcPos.Y = player.Center.Y;
					}
				}
			}
			float steepening = 0.1f;
			for (int j = 0; j < 160; j++)
			{
				if (npcPos != player.Center)
				{
					dX = npcPos.X - finalPos.X;
					dY = npcPos.Y - finalPos.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance <= 8 + j/8f)
					{
						if(i == -1)
							npcPosStore1 = npcPos;
						if(i == 1)
							npcPosStore2 = npcPos;
						npcPos = player.Center;
					}
					float radians = new Vector2(dX, dY).ToRotation();
					if (distance <= 120)
					{
						toVelo += new Vector2(1f + steepening, 0).RotatedBy(radians);
						steepening += 0.1f;
					}
				}
				toVelo.Normalize();
				finalPos += toVelo * 2;
				Vector2 fluctuation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(rotation));
				Vector2 rotlength = new Vector2(fluctuation.X, 0).RotatedBy(MathHelper.ToRadians(rotation + j * 9));
				Vector2 circularLocation = new Vector2(0, rotlength.X).RotatedBy(toVelo.ToRotation());

				if (i == -1)
				{
					if (j > 12)
						dust(mod, finalPos + circularLocation, i, alpha1, alpha2, final, player.whoAmI);
					if(j == 159)
					{
						finalPos += toVelo * 12;
						for (int u = 0; u < 360; u += 10)
						{
							Vector2 circularRotation = new Vector2(-16, 0).RotatedBy(MathHelper.ToRadians(u));
							if (Main.myPlayer == player.whoAmI)
							{
								int num = Dust.NewDust(new Vector2(finalPos.X - 5 + circularRotation.X, finalPos.Y - 6 + circularRotation.Y), 0, 0, mod.DustType("ShortlivedCurseDust"));
								Main.dust[num].velocity *= 0.1f;
								Main.dust[num].alpha = 255 - (alpha1 < 45 ? alpha1 : 45);
								Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
							}
							if (final)
							{
								int num = Dust.NewDust(new Vector2(finalPos.X - 5 + circularRotation.X, finalPos.Y - 6 + circularRotation.Y), 0, 0, 21);
								Main.dust[num].velocity *= 1.5f;
								Main.dust[num].scale = 2.1f;
								Main.dust[num].noGravity = true;
								Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
							}
						}
					}
				}
				if (i == 1)
				{
					if(j > 12)
						dust(mod, finalPos + circularLocation, i, alpha1, alpha2, final, player.whoAmI);
					if (j == 159)
					{
						finalPos += toVelo * 12;
						for (int u = 0; u < 360; u += 10)
						{
							Vector2 circularRotation = new Vector2(-16, 0).RotatedBy(MathHelper.ToRadians(u));
							if (Main.myPlayer == player.whoAmI)
							{	
								int num = Dust.NewDust(new Vector2(finalPos.X - 5 + circularRotation.X, finalPos.Y - 6 + circularRotation.Y), 0, 0, mod.DustType("ShortlivedCurseDust"));
								Main.dust[num].velocity *= 0.1f;
								Main.dust[num].alpha = 255 - (alpha2 < 45 ? alpha2 : 45);
								Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
							}
							if(final)
							{
								int num = Dust.NewDust(new Vector2(finalPos.X - 5 + circularRotation.X, finalPos.Y - 6 + circularRotation.Y), 0, 0, 21);
								Main.dust[num].velocity *= 1.5f;
								Main.dust[num].scale = 2.1f;
								Main.dust[num].noGravity = true;
								Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
							}
						}
					}
				}
				toVelo *= 16;
			}
			return finalPos;
		}
		public static void dust(Mod mod, Vector2 pos, int type, int alpha1, int alpha2, bool final = false, int whoAmI = 255)
		{
			int i = (int)pos.X / 16;
			int j = (int)pos.Y / 16;
			Tile tile = Framing.GetTileSafely(i, j);
			Player player = Main.player[whoAmI];
			if (final)
			{
				int num = Dust.NewDust(pos - new Vector2(5, 6), 0, 0, 21);
				Main.dust[num].velocity *= 0.1f;
				Main.dust[num].scale = 2.1f;
				Main.dust[num].noGravity = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
			}
			else if (type == -1 && alpha1 > 0 && Main.myPlayer == whoAmI)
			{
				int num = Dust.NewDust(pos - new Vector2(5, 6), 0, 0, mod.DustType("ShortlivedCurseDust"));
				Main.dust[num].velocity *= 0.1f;
				Main.dust[num].alpha = 255 - (alpha1 < 45 ? alpha1 : 45);
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
			}
			else if (type == 1 && alpha2 > 0 && Main.myPlayer == whoAmI)
			{
				int num = Dust.NewDust(pos - new Vector2(5, 6), 0, 0, mod.DustType("ShortlivedCurseDust"));
				Main.dust[num].velocity *= 0.1f;
				Main.dust[num].alpha = 255 - (alpha2 < 45 ? alpha2 : 45);
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(SOTSPlayer.ModPlayer(player).darkEyeShader, player);
			}
		}
		public void calculateDamage(Player player, NPC npc, float damageMult)
		{
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			int life = npc.life;
			float value = life + npc.defense;
			int capableVoid = (int)(voidPlayer.voidMeter);
			if (capableVoid < 0)
			{
				capableVoid = 0;
			}
			capableVoid += 4;
			if (value > capableVoid * damageMult)
			{
				value = capableVoid * damageMult;
			}
			player.immune = true;
			player.immuneTime = 30;
			voidPlayer.voidMeter -= value / damageMult - 4;
			if (Main.myPlayer == player.whoAmI)
				Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("DarkEyeDamage"), (int)(1.25f * value * voidPlayer.voidDamage), 0, Main.myPlayer);
		}
		public void teleportEffect(Player player)
		{
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.voidMeter -= 4 * voidPlayer.voidCost;
			alpha1 = 0;
			alpha2 = 0;
			player.immune = true;
			player.immuneTime = 6;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSDashPlayer modPlayer = player.GetModPlayer<SOTSDashPlayer>();
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			rotation++;
			npcPosStore1 = player.Center;
			npcPosStore2 = player.Center;
			Vector2 finalPos = DrawDetect(player, 1);
			Vector2 finalPos2 = DrawDetect(player, -1);
			if(rotation % 2 == 0)
			{
				if (npcPosStore1 != player.Center || (player.velocity.Length() <= 0.5f && !hideVisual) || (alpha1 < 18 && !hideVisual))
					alpha1++;
				else if (npcPosStore1 == player.Center && (alpha2 > 10 || hideVisual))
					alpha1--;
				if (npcPosStore2 != player.Center || (player.velocity.Length() <= 0.5f && !hideVisual) || (alpha2 < 18 && !hideVisual))
					alpha2++;
				else if (npcPosStore2 == player.Center && (alpha2 > 10 || hideVisual))
					alpha2--;
			}

			if (alpha1 < 0)
				alpha1 = 0;
			if (alpha2 < 0)
				alpha2 = 0;
			if (alpha1 > 45)
				alpha1 = 45;
			if (alpha2 > 45)
				alpha2 = 45;

			if(hideVisual)
			{
				if(alpha1 > 24)
				{
					alpha1 = 24;
				}
				if (alpha2 > 24)
				{
					alpha2 = 24;
				}
			}


			if (!modPlayer.DashActive)
				return;

			//Main.NewText("Passed dash active" + Main.myPlayer);
			if (modPlayer.DashTimer == SOTSDashPlayer.MAX_DASH_TIMER)
			{
				Vector2 newVelocity = player.velocity;
				if (modPlayer.DashDir == SOTSDashPlayer.DashLeft || modPlayer.DashDir == SOTSDashPlayer.DashRight)
				{
					int dashDirection = modPlayer.DashDir == SOTSDashPlayer.DashRight ? 1 : -1;
					if(dashDirection == 1)
					{
						bool flag = true;
						int i = (int)finalPos.X / 16;
						int j = (int)finalPos.Y / 16;
						for(int k = -1; k < 2; k++)
						{
							for (int w = -1; w < 2; w++)
							{
								Tile tile = Framing.GetTileSafely(i + k, j + w);
								if(Main.tileSolid[tile.type] == true && !tile.inActive() && Main.tileSolidTop[tile.type] == false && tile.active())
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							teleportEffect(player);
							if (Main.myPlayer == player.whoAmI)
								Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("multiplayerDarkEye"), alpha2, rotation, Main.myPlayer, 1, alpha1);
							if (npcPosStore2 != player.Center)
							{
								for (int k = 0; k < Main.npc.Length; k++)
								{
									NPC npc = Main.npc[k];
									if (npc.CanBeChasedBy() && npc.active && npc.Center.X == npcPosStore2.X && (npc.Center.Y == npcPosStore2.Y || npcPosStore2.Y == player.Center.Y))
									{
										if(Main.hardMode)
										{
											calculateDamage(player, npc, 9f);
										}
										else
										{
											calculateDamage(player, npc, 4.5f);
										}
									}
								}
							}
						}
					}
					else
					{
						bool flag = true;
						int i = (int)finalPos2.X / 16;
						int j = (int)finalPos2.Y / 16;
						for (int k = -1; k < 2; k++)
						{
							for (int w = -1; w < 2; w++)
							{
								Tile tile = Framing.GetTileSafely(i + k, j + w);
								if (Main.tileSolid[tile.type] == true && !tile.inActive() && Main.tileSolidTop[tile.type] == false && tile.active())
								{
									flag = false;
								}
							}
						}
						if (flag)
						{
							teleportEffect(player);
							if (Main.myPlayer == player.whoAmI)
								Projectile.NewProjectile(player.Center, Vector2.Zero, mod.ProjectileType("multiplayerDarkEye"), alpha2, rotation, Main.myPlayer, -1, alpha1);
							if (npcPosStore1 != player.Center)
							{
								for (int k = 0; k < Main.npc.Length; k++)
								{
									NPC npc = Main.npc[k];
									if (npc.CanBeChasedBy() && npc.active && npc.Center.X == npcPosStore1.X && (npc.Center.Y == npcPosStore1.Y || npcPosStore1.Y == player.Center.Y))
									{
										if (Main.hardMode)
										{
											calculateDamage(player, npc, 9f);
										}
										else
										{
											calculateDamage(player, npc, 4.5f);
										}
									}
								}
							}
						}
					}
				}
				if(Main.myPlayer != player.whoAmI)
					modPlayer.DashDir = -1;
			}

			modPlayer.DashTimer--;
			modPlayer.DashDelay--;
			if (modPlayer.DashDelay <= 0)
			{
				modPlayer.DashDelay = SOTSDashPlayer.MAX_DASH_DELAY;
				modPlayer.DashTimer = SOTSDashPlayer.MAX_DASH_TIMER;
				modPlayer.DashActive = false;
			}
		}
	}
	public class multiplayerDarkEye : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 50;
			projectile.timeLeft = 10;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;
			projectile.alpha = 255;
		}
		bool effect = false;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (!effect)
			{
				Vector2 velo = Vector2.Zero;
				effect = true;
				Vector2 finalPos = TheDarkEye.StaticDrawDetect(mod, projectile.Center, (int)projectile.ai[0], (int)projectile.ai[1], projectile.damage, (int)projectile.knockBack, ref velo, true, player);
				Main.PlaySound(SoundID.Item8, Main.player[projectile.owner].Center);
				player.position = finalPos - new Vector2(player.width / 2, player.height / 2);
				if(player.velocity.Y > 1)
				{
					player.velocity.Y = 1;
				}
				player.velocity = new Vector2(velo.X * 0.4f, velo.Y * 0.8f) + new Vector2(player.velocity.X, player.velocity.Y);
			}
		}
	}
	public class SOTSDashPlayer : ModPlayer
	{
		public static readonly int DashRight = 2;
		public static readonly int DashLeft = 3;

		public int DashDir = -1;

		public bool DashActive = false;
		public int DashDelay = MAX_DASH_DELAY;
		public int DashTimer = MAX_DASH_TIMER;
		public static readonly int MAX_DASH_DELAY = 50;
		public static readonly int MAX_DASH_TIMER = 35;
		public override void ResetEffects()
		{
			bool dashAccessoryEquipped = false;

			for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
			{
				Item item = player.armor[i];

				if (item.type == ItemType<TheDarkEye>())
				{ 
					dashAccessoryEquipped = true;
				}
			}

			if (!dashAccessoryEquipped || player.mount.Active || DashActive || player.grappling[0] >= 0)
				return;

			if (player.controlRight && player.releaseRight && player.doubleTapCardinalTimer[DashRight] < 15)
				DashDir = DashRight;
			else if (player.controlLeft && player.releaseLeft && player.doubleTapCardinalTimer[DashLeft] < 15)
				DashDir = DashLeft;
			else
				return; 

			if(Main.myPlayer == player.whoAmI)
				DashActive = true;
			//Here you'd be able to set an effect that happens when the dash first activates
			//Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
		}
	}
}