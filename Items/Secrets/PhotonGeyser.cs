using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Laser;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	public class PhotonGeyser : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photon Geyser");
			Tooltip.SetDefault("Unleash a helix of homing rainbow light");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Secrets/PhotonGeyser_Glow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			position += new Vector2(37 * scale, 18 * scale);
			float counter = Main.GlobalTime * 160;
			//int bonus = (int)(counter / 360f);
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 140, 0, 0);
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						break;
					case 4:
						color = new Color(0, 0, 255, 0);
						break;
					case 5:
						color = new Color(140, 0, 255, 0);
						break;
				}
				Rectangle frame = new Rectangle(0, 0, 74, 36);
				Vector2 rotationAround = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, position + rotationAround, frame, color, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Secrets/PhotonGeyser_Glow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float counter = Main.GlobalTime * 160;
			//int bonus = (int)(counter / 360f);
			for (int i = 0; i < 6; i++)
			{
				Color color = new Color(255, 0, 0, 0);
				switch (i)
				{
					case 0:
						color = new Color(255, 0, 0, 0);
						break;
					case 1:
						color = new Color(255, 140, 0, 0);
						break;
					case 2:
						color = new Color(255, 255, 0, 0);
						break;
					case 3:
						color = new Color(0, 255, 0, 0);
						break;
					case 4:
						color = new Color(0, 0, 255, 0);
						break;
					case 5:
						color = new Color(140, 0, 255, 0);
						break;
				}
				Rectangle frame = new Rectangle(0, 0, 74, 36);
				Vector2 rotationAround = new Vector2(4 * scale, 0).RotatedBy(MathHelper.ToRadians(60 * i + counter));
				Main.spriteBatch.Draw(texture, Item.Center + rotationAround - Main.screenPosition + new Vector2(0, 2), frame, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void SetDefaults()
		{
			Item.damage = 42;
			Item.magic = true;
			Item.width = 74;
			Item.height = 36;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			//Item.UseSound = SoundID.Item5;
			Item.autoReuse = false;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Laser.PhotonGeyser>();
			Item.shootSpeed = 22f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.mana = 50;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<PrismOrb>(), damage, knockBack, player.whoAmI, 0, ModContent.ProjectileType<PrismLaser>());
			return true;
		}
	}
}