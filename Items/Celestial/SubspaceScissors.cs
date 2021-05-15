using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using SOTS.Void;

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
            item.damage = 180;
            item.melee = true;  
            item.width = 52;
            item.height = 52;  
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 5f;
            item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SubspaceCrusher"); 
            item.shootSpeed = 18f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
			item.expert = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return player.ownedProjectileCounts[type] <= 0; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 17;
		}
		public override void AddRecipes()
		{
			/*
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe(); */
		}
	}
}
