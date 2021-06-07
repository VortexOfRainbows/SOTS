using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SOTS.Buffs;

namespace SOTS.Items.Celestial
{
	public class FoggyClairvoyance : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Foggy Clairvoyance");
			Tooltip.SetDefault("Increases damage by 35% and grants immunity to every debuff, but at a cost\n'Cursed'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 38;     
            item.height = 40;   
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(ModContent.ItemType<Fragments.PrecariousCluster>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(ModContent.BuffType<FluidCurse>(), 3);
			List<int> bList = new List<int>() { BuffID.PotionSickness, ModContent.BuffType<FluidCurse>(), ModContent.BuffType<VoidRecovery>(), ModContent.BuffType<VoidShock>(), ModContent.BuffType<VoidSickness>(), BuffID.ManaSickness, ModContent.BuffType<Satiated>() };
			for(int i = 0; i < player.buffImmune.Length; i++)
            {
				bool debuff = Main.debuff[i];
				if(debuff && !bList.Contains(i))
                {
					player.buffImmune[i] = true;
                }
            }
			player.allDamage += 0.35f;
		}
	}
}