using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class PlanetaryWarper : ModItem
	{ 	int damageBuff = 0;
		int swingActive = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planetary Warper");
			Tooltip.SetDefault("Critical hits will hit every enemy on screen instead of doing bonus damage");
		}
		public override void SetDefaults()
		{
            item.damage = 42;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 52;     //gun image width
            item.height = 52;   //gun image  height
            item.useTime = 26;  //how fast 
            item.useAnimation = 26;
            item.useStyle = 1;   
            item.autoReuse = true; 
			item.useTurn = true;
            item.knockBack = 1.55f;
            item.value = 150000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
			item.crit = 6;

			
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if(crit == true)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("PlanetariumDetonate"), item.damage, 0, Main.myPlayer, 0f, 0f);
				damage = damage / 2;
			}
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
