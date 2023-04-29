using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Earth;
using SOTS.Items.Permafrost;

namespace SOTS.Items.Pyramid
{
	public class BrachialLance : VoidItem
	{
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 70;  
            Item.DamageType = DamageClass.Magic;  
            Item.width = 90;    
            Item.height = 92;
			Item.useAnimation = 36;
			Item.useTime = 36;
			Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5.25f;
			Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Pyramid.BrachialLance>(); 
            Item.shootSpeed = 16f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 16;
			Item.channel = true;
			Item.useTurn = true;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
            return false;
        }
        public override bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public override int GetVoid(Player player)
		{
			return 10;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
		public override void AddRecipes()
		{
			//CreateRecipe(1).AddIngredient<FrigidJavelin>(1).AddIngredient<CurseballTome>(1).AddIngredient<Geostorm>(1).AddIngredient<CursedMatter>(5).AddIngredient(ItemID.SoulofNight, 5).AddIngredient(ItemID.SoulofMight, 5).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
