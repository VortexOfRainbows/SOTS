using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SOTS.Items.Celestial;
using System.Collections.Generic;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Earth;
using SOTS.Items.Otherworld;
using Terraria.Graphics.Shaders;
using System.Linq;

namespace SOTS.Projectiles.Celestial
{
	public class SubspaceServant : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Servant");
			Main.projPet[projectile.type] = true;
			//Main.vanityPet[projectile.type] = true;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 44;
			projectile.height = 44;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.netImportant = true;
			projectile.hide = true;
		}
		Vector2[] trailPos = new Vector2[13];
		bool runOnce = true;
        public override bool PreAI()
		{
			if (Main.myPlayer != projectile.owner)
				projectile.timeLeft = 20;
			if (runOnce)
			{
				projectile.ai[1] = 80f;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			return base.PreAI();
		}
        public override bool? CanCutTiles() { return false; }
		public override bool MinionContactDamage() { return false; }
		public override bool ShouldUpdatePosition() { return false; }
		Item sItem;
		Vector2 itemLocation;
		Vector2 cursorArea;
		int UseTime = 0;
		int FullUseTime = 0;
		float itemRotation = 0;
		int direction = 0;
		int frame = 0;
		int trailingType = 0;
		Vector2 oldPosition;
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cursorArea.X);
			writer.Write(cursorArea.Y);
			writer.Write(oldPosition.X);
			writer.Write(oldPosition.Y);
			writer.Write(itemLocation.X);
			writer.Write(itemLocation.Y);
			writer.Write(itemRotation);
			writer.Write(direction);
			writer.Write(trailingType);
			base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			cursorArea.X = reader.ReadSingle();
			cursorArea.Y = reader.ReadSingle();
			oldPosition.X = reader.ReadSingle();
			oldPosition.Y = reader.ReadSingle();
			itemLocation.X = reader.ReadSingle();
			itemLocation.Y = reader.ReadSingle();
			itemRotation = reader.ReadSingle();
			direction = reader.ReadInt32();
			trailingType = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
        }
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SubspacePlayer subPlayer = SubspacePlayer.ModPlayer(player);
			Vector2 idlePosition = player.Center;
			Item item = player.inventory[49];
			sItem = item;
			int type = Item.type;
			if(cursorArea != null)
			{
				if (Main.myPlayer == player.whoAmI)
				{
					cursorArea = Main.MouseWorld;
					projectile.netUpdate = true;
				}
				if (trailingType == 0)
				{
					idlePosition.X -= player.direction * 64f;
				}
				if (trailingType == 1) //magic
				{
					idlePosition.Y -= 48f;
					Vector2 toCursor = cursorArea - player.Center;
					toCursor = toCursor.SafeNormalize(Vector2.Zero) * -128f;
					toCursor.Y *= 0.375f;
					toCursor.Y = -Math.Abs(toCursor.Y);
					idlePosition += toCursor;
				}
				if (trailingType == 2) //ranged
				{
					idlePosition.Y -= 64f;
					Vector2 toCursor = cursorArea - player.Center;
					toCursor = toCursor.SafeNormalize(Vector2.Zero) * 128f;
					toCursor.Y *= 0.4125f;
					idlePosition += toCursor;
				}
				if (trailingType == 3) //melee
				{
					//idlePosition.Y += 8f + (float)Math.Sqrt(Item.width * Item.height) * 0.5f;
					Vector2 toCursor = cursorArea - player.Center;
					float lengthToCursor = -32 + toCursor.Length();
					toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
					idlePosition += toCursor;
				}
				if (trailingType == 4) //melee, but no melee ?
				{
					idlePosition.Y += 32f;
					Vector2 toCursor = cursorArea - player.Center;
					float lengthToCursor = -32 + toCursor.Length() * 0.66f;
					toCursor.Y *= 0.7f;
					toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
					idlePosition += toCursor;
				}
				Vector2 toIdle = idlePosition - projectile.Center;
				float dist = toIdle.Length();
				float speed = 3 + (float)Math.Pow(dist, 1.45) * 0.002f;
				if (dist < speed)
				{
					speed = toIdle.Length();
				}
				projectile.velocity = toIdle.SafeNormalize(Vector2.Zero) * speed;
				if (direction == 1)
				{
					if (projectile.ai[0] < direction)
						projectile.ai[0] += 0.1f;
				}
				else
				{
					if (projectile.ai[0] > direction)
						projectile.ai[0] -= 0.1f;
				}
				oldPosition = projectile.Center + new Vector2(-5 * projectile.ai[0], 2);
				if (projectile.ai[1] >= 24)
				{
					projectile.ai[1] -= 24f;
				}
				Vector2 circular = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(15 * projectile.ai[1]));
				projectile.ai[1] += 0.75f;
				if (circular.Y > 0)
					circular.Y *= 0.5f;
				projectile.velocity.Y += circular.Y;
				projectile.position += projectile.velocity;
			}
			Lighting.AddLight(projectile.Center, new Vector3(75, 30, 75) * 1f / 255f);

			#region coolStuff
			//player.ItemCheck(player.whoAmI);
			//trailingType = 0;
			bool capable = false;
			Projectile proj = new Projectile();
			proj.SetDefaults(Item.shoot);
			if (proj.aiStyle == 19 || Item.ammo > 0 || Item.fishingPole > 0)
			{
				capable = true;
			}
			proj.active = false;
			proj.Kill();
			if (!capable && lastItem == Item.type && Item.active && !Item.IsAir && !Item.summon && !Item.thrown && !Item.channel && !SOTSPlayer.locketBlacklist.Contains(type) && (Item.useStyle == ItemUseStyleID.SwingThrow || Item.useStyle == ItemUseStyleID.HoldingOut) && !subPlayer.servantIsVanity && !Item.consumable)
			{
				subPlayer.foundItem = true;
				float allSpeed = ItemLoader.UseTimeMultiplier(item, player) * PlayerHooks.UseTimeMultiplier(player, item);
				FullUseTime = !sItem.melee ? (int)(sItem.useAnimation / (float)allSpeed) : (int)(sItem.useAnimation / (float)allSpeed * (double)player.meleeSpeed);
				int fireRate = !sItem.melee ? (int)(sItem.useTime / (float)allSpeed) : (int)(sItem.useTime / (float)allSpeed * (double)player.meleeSpeed);
				if (fireRate > FullUseTime)
                {
					fireRate = FullUseTime;
				}
				int extraSpeed = 1;
				if (Item.useStyle == ItemUseStyleID.SwingThrow)
					extraSpeed++;
				if (UseTime <= -Item.reuseDelay + extraSpeed || UseTime > FullUseTime + 1)
				{
					UseTime = FullUseTime + 1;
				}
				if (player.controlUseItem || (UseTime > -Item.reuseDelay && UseTime <= FullUseTime))
				{
					UseTime--;
					if(UseTime == FullUseTime)
					{
						bool can = ItemLoader.CanUseItem(item, player);
						if (Item.useAmmo > 0)
						{
							float shootSpeed = Item.shootSpeed;
							int shootType = Item.shoot;
							int damage = Item.damage;
							float knockBack = Item.knockBack;
							bool canShoot = false;
							player.PickAmmo(item, ref shootType, ref shootSpeed, ref canShoot, ref damage, ref knockBack, true);
							if (!canShoot)
								can = false;
						}
						if (sItem.mana > 0)
						{
							var flag2 = false;
							if (sItem.type == ItemID.LaserMachinegun)
								flag2 = true;
							if (sItem.shoot > ProjectileID.None && ProjectileID.Sets.TurretFeature[sItem.shoot] && player.altFunctionUse == 2)
								flag2 = true;
							if (sItem.shoot > ProjectileID.None && ProjectileID.Sets.MinionTargettingFeature[sItem.shoot] && player.altFunctionUse == 2)
								flag2 = true;
							if (sItem.type != sbyte.MaxValue || !player.spaceGun)
							{
								if (player.statMana >= (int)(sItem.mana * (double)player.manaCost))
								{
									if (!flag2)
										player.statMana -= (int)(sItem.mana * (double)player.manaCost);
								}
								else if (player.manaFlower)
								{
									player.QuickMana();
									if (player.statMana >= (int)(sItem.mana * (double)player.manaCost))
									{
										if (!flag2)
											player.statMana -= (int)(sItem.mana * (double)player.manaCost);
									}
									else
										can = false;
								}
								else
									can = false;
							}
						}
						if (!can)
						{
							UseTime = FullUseTime + 1;
							frame = projectile.velocity.Length() > 2f ? 5 : 0;
							if (player.whoAmI == Main.myPlayer)
								direction = player.direction;
							return;
						}
						ItemLoader.UseItem(item, player);
						Main.PlaySound(Item.UseSound, projectile.Center);
					}
					if (UseTime <= FullUseTime && (UseTime % fireRate == 0 || (UseTime == FullUseTime && fireRate > FullUseTime)))
					{
						trailingType = 0;
						if (UseTime > 0)
						{
							ModItem mItem = Item.modItem;
							if (mItem != null)
							{
								Vector2 position = projectile.Center;
								subPlayer.UseVanillaItemProjectile(position, item, out itemRotation, ref direction, true);
							}
							else
							{
								Vector2 position = projectile.Center;
								subPlayer.UseVanillaItemProjectile(position, item, out itemRotation, ref direction);
							}
						}
					}
					//direction = projectile.direction;
					Vector2 position2 = projectile.Center;
					if(Main.myPlayer == player.whoAmI)
					{
						itemLocation = subPlayer.UseVanillaItemAnimation(position2, item, UseTime, FullUseTime, ref direction, ref itemRotation);
						subPlayer.UseVanillaItemHitbox(itemLocation, projectile.Center, projectile.velocity, item, UseTime, FullUseTime, ref direction, ref itemRotation);
					}
					else
					{
						int fakeDirection = direction;
						itemLocation = subPlayer.UseVanillaItemAnimation(position2, item, UseTime, FullUseTime, ref fakeDirection, ref itemRotation);
						subPlayer.UseVanillaItemHitbox(itemLocation, projectile.Center, projectile.velocity, item, UseTime, FullUseTime, ref fakeDirection, ref itemRotation);
					}
					subPlayer.PickFrame(sItem, UseTime, FullUseTime, direction, itemRotation, ref frame);
					if(Item.melee || Item.type == ItemID.Toxikarp || Item.type == ItemID.SpiritFlame)
                    {
						if (Item.noMelee)
							trailingType = 4;
						else
							trailingType = 3;
                    }
					else if(Item.ranged)
                    {
						trailingType = 2;
					}
					else if(Item.magic)
					{
						trailingType = 1;
					}
				}
				else
				{
					trailingType = 0;
					frame = projectile.velocity.Length() > 2f ? 5 : 0;
					if (player.whoAmI == Main.myPlayer)
						direction = player.direction;
				}
			}
			else
			{
				trailingType = 0;
				frame = projectile.velocity.Length() > 2f ? 5 : 0;
				if(player.whoAmI == Main.myPlayer)
					direction = player.direction;
				UseTime = -1000;
				Texture2D texture = Main.itemTexture[Item.type];
				if (lastItem != sItem.type || itemTextureOutline == null)
				{
					itemTextureOutline = new Texture2D(Main.graphics.GraphicsDevice, texture.Width, texture.Height);
					itemTextureOutline.SetData(0, null, Greenify(texture, new Color(0, 255, 0)), 0, texture.Width * texture.Height);
					lastItem = sItem.type;
				}
			}
			#endregion
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Player player = Main.player[projectile.owner];
			//SubspacePlayer subspacePlayer = SubspacePlayer.ModPlayer(player);
			//if (subspacePlayer.subspaceServantShader != 0)
			//{
			//	Main.spriteBatch.End();
			//	Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			//	GameShaders.Armor.GetSecondaryShader(subspacePlayer.subspaceServantShader, player).Apply(null);
			//}
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantBody");
			Texture2D textureOutline = mod.GetTexture("Projectiles/Celestial/SubspaceServantBodyOutline");
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, -4);
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
			for (int k = 0; k < 4; k++)
			{
				Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
				spriteBatch.Draw(textureOutline, drawPos + circular, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			DrawItemAnimation(spriteBatch, true);
			DrawTail(spriteBatch, true);
			DrawWings(spriteBatch);
			DrawTail(spriteBatch, false);
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if(player.active)
            {
				//SubspacePlayer subspacePlayer = SubspacePlayer.ModPlayer(player);
				DrawItemAnimation(spriteBatch, false);
				Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantArms");
				Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, -4);
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
				spriteBatch.Draw(texture, drawPos, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			//if (subspacePlayer.subspaceServantShader != 0)
			//{
			//	Main.spriteBatch.End();
			//	Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			//}
		}
		public void DrawTail(SpriteBatch spriteBatch, bool outLine = false)
		{
			if (oldPosition == null)
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantTail");
			Texture2D textureOutline = mod.GetTexture("Projectiles/Celestial/SubspaceServantTailOutline");
			Texture2D texture2 = mod.GetTexture("Projectiles/Celestial/SubspaceServantTailScales");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 center = projectile.Center;
			Vector2 velo = new Vector2(0, 4f);
			float scale = projectile.scale;
			List<Vector2> positions = new List<Vector2>();
			List<float> rotations = new List<float>();
			for (int i = 0; i < 9; i++)
			{
				Vector2 toOldPosition = oldPosition - center;
				toOldPosition.SafeNormalize(Vector2.Zero);
				velo += toOldPosition * 0.333f;
				velo = velo.SafeNormalize(Vector2.Zero) * scale * 4;
				center += velo;
				Vector2 drawPos = center - Main.screenPosition + new Vector2(0, -16 + projectile.height / 2);
				positions.Add(drawPos);
				rotations.Add(velo.ToRotation() - MathHelper.ToRadians(90));
				scale -= 0.0725f;
			}
			if (outLine)
			{
				for (int i = 8; i >= 0; i--)
				{
					for (int k = 0; k < 4; k++)
					{
						Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
						spriteBatch.Draw(textureOutline, positions[i] + circular, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, (1 - i * 0.08f), direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
					}
				}
			}
			else
			{
				for (int i = 8; i >= 0; i--)
				{
					spriteBatch.Draw(texture, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, (1 - i * 0.08f), direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
				for (int i = 8; i >= 0; i--)
				{
					spriteBatch.Draw(texture2, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, (1 - i * 0.08f), direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
		}
		public void DrawWings(SpriteBatch spriteBatch)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantWings");
			Texture2D textureOutline = mod.GetTexture("Projectiles/Celestial/SubspaceServantWingsOutline");
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(-8 * direction, -4);
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
			int frame = (int)(projectile.ai[1] / 4);
			if(frame < 0)
            {
				frame = 0;
            }
			if(frame > 5)
            {
				frame = 5;
            }
			for (int k = 0; k < 4; k++)
			{
				Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
				spriteBatch.Draw(textureOutline, drawPos + circular, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		int lastItem = -1;
		Texture2D itemTextureOutline;
		public void DrawItemAnimation(SpriteBatch spriteBatch, bool outline = false)
        {
			Player player = Main.player[projectile.owner];
			if (sItem != null && !sItem.IsAir)
			{
				Item item = sItem;
				Texture2D texture = Main.itemTexture[Item.type];
				if(lastItem != sItem.type || itemTextureOutline == null)
				{
					itemTextureOutline = new Texture2D(Main.graphics.GraphicsDevice, texture.Width, texture.Height);
					itemTextureOutline.SetData(0, null, Greenify(texture, new Color(0, 255, 0)), 0, texture.Width * texture.Height);
					lastItem = sItem.type;
				}
				Texture2D textureOutline = itemTextureOutline;
				if (itemLocation != null && texture != null && UseTime != -1000 && UseTime <= FullUseTime && (!Item.noUseGraphic || Item.type == ModContent.ItemType<VibrantPistol>() || Item.type == ModContent.ItemType<StarcoreAssaultRifle>()))
				{
					Vector2 location = itemLocation;
					if (Item.useStyle == ItemUseStyleID.HoldingOut)
					{
						if (Item.staff[Item.type])
						{
							float rotation = itemRotation + 0.785f * direction;
							int width = 0;
							Vector2 origin = new Vector2(0f, Main.itemTexture[Item.type].Height);
							var num23 = 0;
							var num24 = 0;
							if (sItem.type == ItemID.Toxikarp)
							{
								num23 = 8 * -direction;
								num24 = 2;
							}

							if (sItem.type == ItemID.ApprenticeStaffT3)
							{
								num23 = 12 * -direction;
								num24 = 12;
							}

							if (sItem.type == ItemID.SkyFracture)
								num24 = (int)(8 * Math.Cos((double)rotation));
							if (direction == -1)
							{
								origin = new Vector2(Main.itemTexture[Item.type].Width, Main.itemTexture[Item.type].Height);
								width -= Main.itemTexture[Item.type].Width;
							}
							location += new Vector2(num23, num24);
							if(outline)
								for (int i = 0; i < 4; i++)
								{
									Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
									spriteBatch.Draw(textureOutline, new Vector2((int)(location.X - Main.screenPosition.X + origin.X + width), (int)(location.Y - Main.screenPosition.Y)) + circular, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[Item.type].Width, Main.itemTexture[Item.type].Height)),
										Color.White, rotation, origin, Item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
								}
							else
							spriteBatch.Draw(texture, new Vector2((int)(location.X - Main.screenPosition.X + origin.X + width), (int)(location.Y - Main.screenPosition.Y)), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[Item.type].Width, Main.itemTexture[Item.type].Height)), 
								Color.White, rotation, origin, Item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
						}
						else if (Item.type == ItemID.SpiritFlame)
						{
							var vector2_1 = location;
							var r = texture.Frame(1, 1, 0, 0);
							var vector2_3 = new Vector2((float)(r.Width / 2 * direction), 0.0f);
							var origin4 = r.Size() / 2f;
							if (outline)
								for (int i = 0; i < 4; i++)
								{
									Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
									spriteBatch.Draw(textureOutline, circular + (vector2_1 - Main.screenPosition + vector2_3).Floor(),
									new Microsoft.Xna.Framework.Rectangle?(r),
									Item.GetAlpha(Color.White),
									itemRotation, origin4, Item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
									0);
								}
							else 
								spriteBatch.Draw(texture, (vector2_1 - Main.screenPosition + vector2_3).Floor(),
							new Microsoft.Xna.Framework.Rectangle?(r),
							Item.GetAlpha(Color.White),
							itemRotation, origin4, Item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
							0);
						}
						else
						{
							SubspacePlayer subPlayer = SubspacePlayer.ModPlayer(player);
							Vector2 superAwesomeOffset = subPlayer.DrawPlayerItemPos(Item.type);
							Vector2 vector10 = new Vector2((float)(Main.itemTexture[Item.type].Width / 2), superAwesomeOffset.Y);
							Vector2 vector11 = new Vector2(superAwesomeOffset.X, texture.Height / 2);
							Vector2 offset = new Vector2(0, 0);
							ItemLoader.HoldoutOffset(1, Item.type, ref offset);
							if (offset.X != 0)
							{
								vector11.X = offset.X;
							}
							vector11.Y += offset.Y;
							int num107 = (int)vector11.X;
							vector10.Y = vector11.Y;
							Vector2 origin5 = new Vector2(-num107, (Main.itemTexture[Item.type].Height / 2));
							if (direction == -1)
							{
								origin5 = new Vector2((Main.itemTexture[Item.type].Width + num107), (float)(Main.itemTexture[Item.type].Height / 2));
							}
							if (outline)
								for (int i = 0; i < 4; i++)
								{
									Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
									spriteBatch.Draw(textureOutline, circular + new Vector2((int)(location.X - Main.screenPosition.X + vector10.X), (int)(location.Y - Main.screenPosition.Y + vector10.Y)), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[Item.type].Width, Main.itemTexture[Item.type].Height)),
									Color.White, itemRotation, origin5, Item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
								}
							else spriteBatch.Draw(texture, new Vector2((int)(location.X - Main.screenPosition.X + vector10.X), (int)(location.Y - Main.screenPosition.Y + vector10.Y)), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[Item.type].Width, Main.itemTexture[Item.type].Height)), 
							Color.White, itemRotation, origin5, Item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
						}
					}
					else
					{
						if (outline)
							for (int i = 0; i < 4; i++)
							{
								Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * i));
								spriteBatch.Draw(textureOutline, circular + new Vector2((float)((int)(location.X - Main.screenPosition.X)),
								(float)((int)(location.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, itemRotation,
								 new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * direction, texture.Height),
								Item.scale,
								direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
								0);
							}
						else spriteBatch.Draw(texture, new Vector2((float)((int)(location.X - Main.screenPosition.X)),
						(float)((int)(location.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, itemRotation,
						 new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * direction, texture.Height),
						Item.scale,
						direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
						0);
					}
				}
			}
		}
		public static Color[] Greenify(Texture2D texture, Color color)
        {
			int width = texture.Width;
			int height = texture.Height;
			Color[] data = new Color[width * height];
			texture.GetData(data);
			for(int i = 0; i < width * height; i++)
            {
				if(data[i].A >= 255)
					data[i] = color;
            }
			return data;
        }
    }
}