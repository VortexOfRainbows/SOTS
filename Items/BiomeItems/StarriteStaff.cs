using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.BiomeItems
{  
    public class StarriteStaff : ModItem
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Star Staff");
			Tooltip.SetDefault("Summons a Starrite to fight for you");
		}
        public override void SetDefaults()
        {
           
            item.damage = 19;
            item.summon = true;
            item.mana = 10;
            item.width = 44;
            item.height = 42;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 45000;
            item.rare = 6;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("LavaStar");
            item.shootSpeed = 7f;
			item.buffType = mod.BuffType("Starfire");
            item.buffTime = 3600;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ObsidianScale", 14);
			recipe.AddIngredient(ItemID.MeteoriteBar, 14);
			recipe.AddIngredient(ItemID.Obsidian, 33);
			recipe.AddIngredient(ItemID.HellstoneBar, 11);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
    }
}