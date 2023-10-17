using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Permafrost;
using Terraria.DataStructures;

namespace SOTS.Items.Permafrost
{
	public class ShardstormSpell : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
            Item.width = 38;    
            Item.height = 42; 
            Item.useTime = 48; 
            Item.useAnimation = 48;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 7, 0, 0);
            Item.rare = ItemRarityID.Lime;
			Item.UseSound = SoundID.Item92;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 18f; //arbitrary 
			Item.shoot = ModContent.ProjectileType<Shardstorm>();
			Item.mana = 20;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AbsoluteBar>(), 10).AddIngredient(ModContent.ItemType<ShardStaff>(), 1).AddIngredient(ModContent.ItemType<StormSpell>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 cursorPos = Main.MouseWorld;
			Projectile.NewProjectile(source, cursorPos.X,  cursorPos.Y, 0, 0, type, damage, knockback, player.whoAmI);
            return false;
		}
	}
}
