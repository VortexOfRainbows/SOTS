using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Earth;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class Earthshaker : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Tools/EarthshakerGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Launch a long ranged pickaxe which is capable of breaking many blocks at a time");
		}
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Melee;
			Item.width = 66;
			Item.height = 34;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 7.5f;
            Item.value = Item.sellPrice(0, 2, 25, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item61;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.Earthshaker>(); 
            Item.shootSpeed = 16f;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			velocity = velocity * 0.25f;
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EarthshakerPickaxe>(), damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
			return true; 
		}
    }
}
	
