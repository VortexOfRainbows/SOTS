using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Earth;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Temple;
using Terraria.DataStructures;

namespace SOTS.Items.Temple
{
	public class SupernovaScatter : VoidItem
	{
        /*public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Temple/SupernovaScatterGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}*/
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Scatter");
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 47;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 70;
            Item.height = 34;
            Item.useTime = 60; 
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 3f;  
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SolarPetal>(); 
            Item.shootSpeed = 16;
			Item.useAmmo = AmmoID.Bullet;
			Item.noUseGraphic = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Temple/SupernovaScatter").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -28;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -3;
			}
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SolarPetal>(), damage, knockback, player.whoAmI, type, 1f);
			for(int i = 0; i < 4; i++)
            {
				Vector2 velocity2 = velocity.RotateRandom(MathHelper.ToRadians(Main.rand.NextFloat(-33, 33)));
				velocity2 *= Main.rand.NextFloat(0.6f, 1.3f);
				Projectile.NewProjectile(source, position, velocity2, ModContent.ProjectileType<SolarPetal>(), damage, knockback, player.whoAmI, type, 0.425f + 0.05f * i);
			}
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			position += velocity.SafeNormalize(Vector2.Zero) * 24;
        }
        public override int GetVoid(Player player)
		{
			return 60;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-28f, -3f);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LunarTabletFragment, 20).AddIngredient(ItemID.LihzahrdPowerCell, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
