using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class MinersPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Pickaxe");
			Tooltip.SetDefault("Throws a wall breaking pickaxe");
		}
		public override void SetDefaults()
		{
            item.damage = 12;  //gun damage
            item.thrown = true;   //its a gun so set this to true
            item.width = 28;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 38;  //how fast 
            item.useAnimation = 38;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000;
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.shoot =  mod.ProjectileType("Pick"); 
            item.shootSpeed = 3.5f;
			item.consumable = true;
			item.noUseGraphic = true;
			item.maxStack = 99;
		}
	}
}
