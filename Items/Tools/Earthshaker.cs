using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class Earthshaker : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Tools/EarthshakerGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Launch a long ranged pickaxe which is capable of breaking many blocks at a time");
		}
		public override void SetDefaults()
		{
			item.damage = 11;
			item.melee = true;
			item.width = 66;
			item.height = 34;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 7.5f;
            item.value = Item.sellPrice(0, 2, 25, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item61;
			item.shoot = ModContent.ProjectileType<Projectiles.Earth.Earthshaker>(); 
            item.shootSpeed = 16f;
			item.noUseGraphic = true;
			item.channel = true;
			item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 velocity = new Vector2(speedX, speedY) * 0.25f;
			Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<EarthshakerPickaxe>(), damage, knockBack, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
			return true; 
		}
    }
}
	
