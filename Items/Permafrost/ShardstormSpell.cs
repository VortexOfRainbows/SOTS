using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Permafrost;

namespace SOTS.Items.Permafrost
{
	public class ShardstormSpell : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Bombardment");
			Tooltip.SetDefault("Create an area of arctic obliteration targeted on your cursor");
		}
		public override void SetDefaults()
		{
            Item.damage = 36;
			Item.magic = true;
            Item.width = 38;    
            Item.height = 42; 
            Item.useTime = 55; 
            Item.useAnimation = 55;
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<ShardStaff>(), 1);
			recipe.AddIngredient(ModContent.ItemType<StormSpell>(), 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 cursorPos = Main.MouseWorld;
			Projectile.NewProjectile(cursorPos.X,  cursorPos.Y, 0, 0, type, damage, knockBack, player.whoAmI);
            return false;
		}
	}
}
