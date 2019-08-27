using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Items.Planetarium
{
	public class PlanetaryScepter : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Scepter");
			Tooltip.SetDefault("Fires an orbiting beam");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 32;
            item.width = 38;   
            item.height = 38;  
            item.useTime = 22; 
            item.useAnimation = 22;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PlanetaryBeam"); 
            item.shootSpeed = 8.5f;
			item.mana = 4;
			Item.staff[item.type] = true;
            item.value = 150000;
            item.rare = 9;
            item.UseSound = SoundID.Item8;
		}	
		public override void GetVoid(Player player)
		{
				voidMana = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlanetaryCore", 1);
			recipe.AddIngredient(null, "EmptyPlanetariumOrb", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
