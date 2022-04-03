using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;
using Terraria.ModLoader;
using SOTS.Projectiles.Crushers;

namespace SOTS.Items.Celestial
{
	public class SubspaceScissors : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Scissors");
			Tooltip.SetDefault("'Assistance from purgatory'");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 190;
            item.melee = true;  
            item.width = 46;
            item.height = 46;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 5f;
            item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SubspaceCrusher>(); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
		public override int GetVoid(Player player)
		{
			return 17;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
