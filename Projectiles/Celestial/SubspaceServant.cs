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
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SubspacePlayer subPlayer = SubspacePlayer.ModPlayer(player);
			Vector2 idlePosition = player.Center + new Vector2(-32, 0);
			Vector2 toCursor = Main.MouseWorld - player.Center;
			float rotationToCursor = toCursor.ToRotation();
			toCursor = toCursor.SafeNormalize(Vector2.Zero) * 128f;
			toCursor.Y *= 0.4f;
			//idlePosition += toCursor;
			Vector2 toIdle = idlePosition - projectile.Center;
			float dist = toIdle.Length();
			float speed = 8 + (float)Math.Pow(dist, 1.25) * 0.005f;
			if(dist < speed)
            {
				speed = toIdle.Length();
            }
			projectile.velocity = toIdle.SafeNormalize(Vector2.Zero) * speed;
			projectile.position += projectile.velocity;
			Lighting.AddLight(projectile.Center, new Vector3(75, 30, 75) * 1f / 255f);

			#region coolStuff
			List<int> blackList = new List<int>() { ItemID.BookStaff, ModContent.ItemType<LashesOfLightning>() };
            Item item = player.inventory[49];
			sItem = item;
			int type = item.type;
			//player.ItemCheck(player.whoAmI);
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
				FullUseTime = item.useAnimation;
				int fireRate = item.useTime;
				if (fireRate > item.useAnimation)
                {
					fireRate = item.useAnimation;
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
					if (UseTime <= FullUseTime && UseTime % fireRate == 0)
					{
						if (UseTime > 0)
						{
							ModItem mItem = item.modItem;
							if (mItem != null)
							{
								Vector2 toCursor2 = Main.MouseWorld - projectile.Center;
								float shootSpeed = item.shootSpeed;
								Vector2 position = projectile.Center;
								int shootType = item.shoot;
								int damage = item.damage;
								float knockBack = item.knockBack;
								bool canShoot = true;
								if (item.useAmmo > 0)
									player.PickAmmo(item, ref shootType, ref shootSpeed, ref canShoot, ref damage, ref knockBack, !mItem.ConsumeAmmo(player));
								if (canShoot)
								{
									subPlayer.UseVanillaItemProjectile(position, item, out itemRotation, ref direction, true);
									Vector2 shootTo = toCursor2.SafeNormalize(Vector2.Zero) * shootSpeed;
									bool Shoot = mItem.Shoot(player, ref position, ref shootTo.X, ref shootTo.Y, ref shootType, ref damage, ref knockBack);
									if (Shoot)
									{
										Projectile.NewProjectile(position, shootTo, shootType, damage, knockBack, player.whoAmI);
									}
								}
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
				}
			}
			else
            {
				UseTime = -1000;
            }
            #endregion
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			DrawItemAnimation(spriteBatch, lightColor);
            base.PostDraw(spriteBatch, lightColor);
        }
		public void DrawItemAnimation(SpriteBatch spriteBatch, Color lightColor)
        {
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Mod mod = ModLoader.GetMod("SOTS");
			if (sItem != null && !sItem.IsAir)
			{
				Item item = sItem;
				Texture2D texture = Main.itemTexture[item.type];
				Vector2 zero2 = Vector2.Zero;
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
						else
						{
							Vector2 vector10 = new Vector2((float)(Main.itemTexture[item.type].Width / 2), (float)(Main.itemTexture[item.type].Height / 2));
							Vector2 vector11 = new Vector2(10, texture.Height / 2);
							if (item.GetGlobalItem<ItemUseGlow>().glowOffsetX != 0)
							{
								vector11.X = item.GetGlobalItem<ItemUseGlow>().glowOffsetX;
							}
							vector11.Y += item.GetGlobalItem<ItemUseGlow>().glowOffsetY;
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
						spriteBatch.Draw(texture,
							new Vector2((float)((int)(location.X - Main.screenPosition.X)),
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