using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class HeartyEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hearty Relic");
			Tooltip.SetDefault("Increases all damage done by 7%\nGive more max minions\nIncreases pickaxe speed by 35%\nGives permanent lifeforce, swiftness, feather fall, magic power, and gills potion effects\nReduces damage taken by 12%\nGrants the ability to dash\nDeals double damage back to the attacker");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 32;   
            item.value = 50000;
            item.rare = 6;
			item.expert = true;
			item.accessory = true;
			item.defense = 65;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"AntimaterialMandible", 5);
			recipe.AddIngredient(null,"BarrierEnchant", 1);
			recipe.AddIngredient(null,"GlassEnchant", 1);
			recipe.AddIngredient(null,"ShardEnchant", 1);
			recipe.AddIngredient(null,"DrugEnchant", 1);
			recipe.AddIngredient(null,"TheHardCore", 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.pickSpeed -= 0.35f;
			player.maxMinions += 2;
			player.dash = 1;
			
			player.minionDamage += 0.07f;
			player.magicDamage += 0.07f;
			player.meleeDamage += 0.07f;
			player.rangedDamage += 0.07f;
			player.thrownDamage += 0.07f;
			
			player.endurance += 0.12f;
			
			player.thorns = 2f;
			
			player.AddBuff(BuffID.Gills, 300);
			player.AddBuff(BuffID.MagicPower, 300);
			player.AddBuff(BuffID.Lifeforce, 300);
			player.AddBuff(BuffID.Swiftness, 300);
			player.AddBuff(BuffID.Featherfall, 300);
		}
		
	}
}
