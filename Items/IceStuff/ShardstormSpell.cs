using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Projectiles.Permafrost;

namespace SOTS.Items.IceStuff
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
            item.damage = 36;
			item.magic = true;
            item.width = 38;    
            item.height = 42; 
            item.useTime = 55; 
            item.useAnimation = 55;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 7, 0, 0);
            item.rare = ItemRarityID.Lime;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 18f; //arbitrary 
			item.shoot = ModContent.ProjectileType<Shardstorm>();
			item.mana = 20;
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
