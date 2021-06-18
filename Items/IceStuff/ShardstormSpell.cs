using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.IceStuff
{
	public class ShardstormSpell : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shardstorm Spell");
			Tooltip.SetDefault("Create an area of arctic obliteration targeted on your cursor");
		}
		public override void SetDefaults()
		{
            item.damage = 36;
			item.magic = true;
            item.width = 34;    
            item.height = 36; 
            item.useTime = 55; 
            item.useAnimation = 55;
            item.useStyle = 5;    
            item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 7, 0, 0);
            item.rare = 7;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = true;
            item.shootSpeed = 18f; //arbitrary 
			item.shoot = mod.ProjectileType("Shardstorm");
			item.mana = 29;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AbsoluteBar", 10);
			recipe.AddIngredient(null, "ShardStaff", 1);
			recipe.AddIngredient(null, "StormSpell", 1);
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
