using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class ThundershockShortbow : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/ThundershockShortbowGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thundershock Shortbow"); //This in the second of the pre-0.18 legendary weapons to be added back!
			Tooltip.SetDefault("Fires powerful bolts of lightning instead of arrows");
		}
		public override void SetDefaults()
		{
			item.damage = 30;
			item.ranged = true;
			item.width = 30;
			item.height = 62;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5f;
			item.value = Item.sellPrice(0, 3, 50, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.channel = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Otherworld.ThundershockShortbow>();
			item.shootSpeed = 20f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.crit = 4;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			return false;
        }
	}
}