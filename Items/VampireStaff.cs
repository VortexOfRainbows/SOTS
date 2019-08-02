using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items
{  
    public class VampireStaff : ModItem
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampire Staff");
			Tooltip.SetDefault("Summons a Vampire Probe to fight for you");
		}
        public override void SetDefaults()
        {
           
            item.damage = 41;
            item.summon = true;
            item.mana = 10;
            item.width = 26;
            item.height = 28;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 0;
            item.rare = 6;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("DVP");
            item.shootSpeed = 7f;
			item.buffType = mod.BuffType("Phantasmal");
            item.buffTime = 3600;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bone, 40);
			recipe.AddIngredient(ItemID.Cobweb, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}