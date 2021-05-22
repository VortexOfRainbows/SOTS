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
using SOTS.Items.Vibrant;

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
		int UseTime = 0;
		int FullUseTime = 0;
		float itemRotation = 0;
		int direction = 0;
		int frame = 0;
		int trailingType = 0;
		Vector2 oldPosition;
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SubspacePlayer subPlayer = SubspacePlayer.ModPlayer(player);
			Vector2 idlePosition = player.Center;
			Item item = player.inventory[49];
			sItem = item;
			int type = item.type;
			if (trailingType == 0)
			{
				idlePosition.X -= player.direction * 72;
			}
			if(trailingType == 1) //magic
			{
				idlePosition.Y -= 48f;
				Vector2 toCursor = Main.MouseWorld - player.Center;
				toCursor = toCursor.SafeNormalize(Vector2.Zero) * -128f;
				toCursor.Y *= 0.375f;
				toCursor.Y = -Math.Abs(toCursor.Y);
				idlePosition += toCursor;
			}
			if (trailingType == 2) //ranged
			{
				idlePosition.Y -= 64f;
				Vector2 toCursor = Main.MouseWorld - player.Center;
				toCursor = toCursor.SafeNormalize(Vector2.Zero) * 128f;
				toCursor.Y *= 0.4125f;
				idlePosition += toCursor;
			}
			if(trailingType == 3) //melee
			{
				idlePosition.Y += 8f + (float)Math.Sqrt(item.width * item.height) * 0.5f;
				Vector2 toCursor = Main.MouseWorld - player.Center;
				float lengthToCursor = -(8 + (float)Math.Sqrt(item.width * item.height) * 0.5f) + toCursor.Length();
				toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
				idlePosition += toCursor;
			}
			if (trailingType == 4) //melee, but no melee ?
			{
				idlePosition.Y += 32f;
				Vector2 toCursor = Main.MouseWorld - player.Center;
				float lengthToCursor = -32 + toCursor.Length() * 0.66f;
				toCursor.Y *= 0.7f;
				toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
				idlePosition += toCursor;
			}
			Vector2 toIdle = idlePosition - projectile.Center;
			float dist = toIdle.Length();
			float speed = 3 + (float)Math.Pow(dist, 1.5) * 0.002f;
			if(dist < speed)
            {
				speed = toIdle.Length();
            }
			projectile.velocity = toIdle.SafeNormalize(Vector2.Zero) * speed; 
			if(direction == 1)
            {
				if(projectile.ai[0] < direction)
					projectile.ai[0] += 0.1f;
			}
			else
			{
				if (projectile.ai[0] > direction)
					projectile.ai[0] -= 0.1f;
			}
			oldPosition = projectile.Center + new Vector2(-5 * projectile.ai[0], 2);
			projectile.ai[1]++;
			if(projectile.ai[1] > 24)
            {
				projectile.ai[1] = 0;
			}
			Vector2 circular = new Vector2(-2f, 0).RotatedBy(MathHelper.ToRadians(15 * projectile.ai[1]));
			if (circular.Y > 0)
				circular.Y *= 0.5f;
			projectile.velocity.Y += circular.Y;
			projectile.position += projectile.velocity;
			Lighting.AddLight(projectile.Center, new Vector3(75, 30, 75) * 1f / 255f);

			#region coolStuff
			List<int> blackList = new List<int>() { ItemID.BookStaff, ModContent.ItemType<LashesOfLightning>() };
			//player.ItemCheck(player.whoAmI);
			//trailingType = 0;
			if (item.active && !item.IsAir && !item.summon && !item.thrown && !item.channel && !blackList.Contains(type))
			{
				Projectile proj = new Projectile();
				proj.SetDefaults(item.shoot);
				if(proj.aiStyle == 19 || item.ammo > 0)
                {
					return;
				}
				proj.active = false;
				proj.Kill();
				float allSpeed = ItemLoader.UseTimeMultiplier(item, player) * PlayerHooks.UseTimeMultiplier(player, item);
				FullUseTime = !sItem.melee ? (int)(sItem.useAnimation / (float)allSpeed) : (int)(sItem.useAnimation / (float)allSpeed * (double)player.meleeSpeed);
				int fireRate = !sItem.melee ? (int)(sItem.useTime / (float)allSpeed) : (int)(sItem.useTime / (float)allSpeed * (double)player.meleeSpeed);
				if (fireRate > FullUseTime)
                {
					fireRate = FullUseTime;
				}
				int extraSpeed = 1;
				if (item.useStyle == 1)
					extraSpeed++;
				if (UseTime <= -item.reuseDelay + extraSpeed || UseTime > FullUseTime + 1)
				{
					UseTime = FullUseTime + 1;
				}
				if (player.controlUseItem || (UseTime > -item.reuseDelay && UseTime <= FullUseTime))
				{
					UseTime--;
					if(UseTime == FullUseTime)
					{
						bool can = ItemLoader.CanUseItem(item, player);
						if (item.useAmmo > 0)
						{
							float shootSpeed = item.shootSpeed;
							int shootType = item.shoot;
							int damage = item.damage;
							float knockBack = item.knockBack;
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
							if (sItem.shoot > 0 && ProjectileID.Sets.TurretFeature[sItem.shoot] && player.altFunctionUse == 2)
								flag2 = true;
							if (sItem.shoot > 0 && ProjectileID.Sets.MinionTargettingFeature[sItem.shoot] && player.altFunctionUse == 2)
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
							frame = projectile.velocity.Length() > 1f ? 5 : 0;
							direction = player.direction;
							return;
						}
						ItemLoader.UseItem(item, player);
						Main.PlaySound(item.UseSound, projectile.Center);
					}
					if (UseTime <= FullUseTime && (UseTime % fireRate == 0 || (UseTime == FullUseTime && fireRate > FullUseTime)))
					{
						trailingType = 0;
						if (UseTime > 0)
						{
							ModItem mItem = item.modItem;
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
					itemLocation = subPlayer.UseVanillaItemAnimation(position2, item, UseTime, FullUseTime, ref direction, ref itemRotation);
					subPlayer.UseVanillaItemHitbox(itemLocation, projectile.Center, projectile.velocity, item, UseTime, FullUseTime, ref direction, ref itemRotation);
					subPlayer.PickFrame(sItem, UseTime, FullUseTime, direction, itemRotation, ref frame);
					if(item.melee || item.type == ItemID.Toxikarp || item.type == ItemID.SpiritFlame)
                    {
						if (item.noMelee || item.shoot > 0)
							trailingType = 4;
						else
							trailingType = 3;
                    }
					else if(item.ranged)
                    {
						trailingType = 2;
					}
					else if(item.magic)
					{
						trailingType = 1;
					}
				}
				else
				{
					trailingType = 0;
					frame = projectile.velocity.Length() > 1f ? 5 : 0;
					direction = player.direction;
				}
			}
			else
			{
				trailingType = 0;
				frame = projectile.velocity.Length() > 1f ? 5 : 0;
				direction = player.direction;
				UseTime = -1000;
            }
			#endregion
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawTail(spriteBatch, Color.White);
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantBody");
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, -4);
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			DrawItemAnimation(spriteBatch, lightColor);
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantArms");
			Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, -4);
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, projectile.rotation, origin, projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public void DrawTail(SpriteBatch spriteBatch, Color lightColor)
		{
			if(oldPosition == null)
            {
				return;
            }
			Texture2D texture = mod.GetTexture("Projectiles/Celestial/SubspaceServantTail");
			Texture2D texture2 = mod.GetTexture("Projectiles/Celestial/SubspaceServantTailScales");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 center = projectile.Center;
			Vector2 velo = new Vector2(0, 4f);
			float scale = projectile.scale;
			List<Vector2> positions = new List<Vector2>();
			List<float> rotations = new List<float>();
			for (int i = 0; i < 8; i ++)
			{
				Vector2 toOldPosition = oldPosition - center;
				toOldPosition.SafeNormalize(Vector2.Zero);
				velo += toOldPosition * 0.345f;
				velo = velo.SafeNormalize(Vector2.Zero) * scale * 4;
				center += velo;
				Vector2 drawPos = center - Main.screenPosition + new Vector2(0, -14 + projectile.height / 2);
				positions.Add(drawPos);
				rotations.Add(velo.ToRotation() - MathHelper.ToRadians(90));
				scale -= 0.075f;
			}
			for(int i = 7; i >= 0; i--)
			{
				spriteBatch.Draw(texture, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, (1 - i * 0.08f), direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			for (int i = 7; i >= 0; i--)
			{
				spriteBatch.Draw(texture2, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, (1 - i * 0.08f), direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
		public void DrawItemAnimation(SpriteBatch spriteBatch, Color lightColor)
        {
			Player player = Main.player[projectile.owner];
			if (sItem != null && !sItem.IsAir)
			{
				Item item = sItem;
				Texture2D texture = Main.itemTexture[item.type];
				if (itemLocation != null && texture != null && UseTime != -1000 && UseTime <= FullUseTime && (!item.noUseGraphic || item.type == ModContent.ItemType<VibrantPistol>()))
				{
					Vector2 location = itemLocation;
					if (item.useStyle == 5)
					{
						if (Item.staff[item.type])
						{
							float rotation = itemRotation + 0.785f * direction;
							int width = 0;
							Vector2 origin = new Vector2(0f, Main.itemTexture[item.type].Height);

							if (direction == -1)
							{
								origin = new Vector2(Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height);
								width -= Main.itemTexture[item.type].Width;
							}
							spriteBatch.Draw(texture, new Vector2((int)(location.X - Main.screenPosition.X + origin.X + width), (int)(location.Y - Main.screenPosition.Y)), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)), 
								Color.White, rotation, origin, item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
						}
						else if (item.type == ItemID.SpiritFlame)
						{
							var vector2_1 = location;
							var texture2D = Main.itemTexture[item.type];
							var r = texture2D.Frame(1, 1, 0, 0);
							var vector2_3 = new Vector2((float)(r.Width / 2 * direction), 0.0f);
							var origin4 = r.Size() / 2f;
							var color20 = new Color(120, 40, 222, 0) * (float)(((double)((float)((double)player.miscCounter / 75.0 * 6.28318548202515)).ToRotationVector2().X * 1.0 + 0.0) / 2.0 * 0.300000011920929 + 0.850000023841858) * 0.5f;
							var num23 = 2f;
							for (var num24 = 0.0f; (double)num24 < 4.0; ++num24)
							{
								spriteBatch.Draw(Main.glowMaskTexture[218],
									(vector2_1 - Main.screenPosition + vector2_3).Floor() +
									(num24 * 1.570796f).ToRotationVector2() * num23, new Microsoft.Xna.Framework.Rectangle?(r),
									color20, itemRotation, origin4,
									item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
							}
							spriteBatch.Draw(texture2D, (vector2_1 - Main.screenPosition + vector2_3).Floor(),
								new Microsoft.Xna.Framework.Rectangle?(r),
								item.GetAlpha(Color.White)
									.MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))),
								itemRotation, origin4, item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
								0);
						}
						else
						{
							SubspacePlayer subPlayer = SubspacePlayer.ModPlayer(player);
							Vector2 superAwesomeOffset = subPlayer.DrawPlayerItemPos(item.type);
							Vector2 vector10 = new Vector2((float)(Main.itemTexture[item.type].Width / 2), superAwesomeOffset.Y);
							Vector2 vector11 = new Vector2(superAwesomeOffset.X, texture.Height / 2);
							Vector2 offset = new Vector2(0, 0);
							ItemLoader.HoldoutOffset(1, item.type, ref offset);
							if (offset.X != 0)
							{
								vector11.X = offset.X;
							}
							vector11.Y += offset.Y;
							int num107 = (int)vector11.X;
							vector10.Y = vector11.Y;
							Vector2 origin5 = new Vector2(-num107, (Main.itemTexture[item.type].Height / 2));
							if (direction == -1)
							{
								origin5 = new Vector2((Main.itemTexture[item.type].Width + num107), (float)(Main.itemTexture[item.type].Height / 2));
							}
							spriteBatch.Draw(texture, new Vector2((int)(location.X - Main.screenPosition.X + vector10.X), (int)(location.Y - Main.screenPosition.Y + vector10.Y)), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, 0, Main.itemTexture[item.type].Width, Main.itemTexture[item.type].Height)), 
								Color.White, itemRotation, origin5, item.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
						}
					}
					else
					{
						spriteBatch.Draw(texture, new Vector2((float)((int)(location.X - Main.screenPosition.X)),
							(float)((int)(location.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), Color.White, itemRotation,
							 new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * direction, texture.Height),
							item.scale,
							direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
							0);
					}
				}
			}
		}
    }
}