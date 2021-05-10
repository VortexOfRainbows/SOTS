using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Celestial
{
	public class VoidspaceEmblem : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Emblem");
			Tooltip.SetDefault("Increases void damage and magic damage by 10%\nIncreases void crit by 10%\nCritical strikes heal small amounts of void\nReduces void cost by 8%\nIncreases void regen by 3.75 and max void by 50\nRegenerate void when hit\nImmunity to broken armor and ichor");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 32;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(null, "WormWoodParasite", 1);
			recipe.AddIngredient(null, "VoidenBracelet", 1);
			recipe.AddIngredient(null, "SkywareBattery", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.1f;
			voidPlayer.voidCost -= 0.08f;
			player.magicDamage += 0.1f;
			voidPlayer.voidRegen += 0.375f;
			voidPlayer.voidMeterMax2 += 50;
			voidPlayer.voidCrit += 10;

			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(modPlayer.onhit == 1)
			{
				voidPlayer.voidMeter += 3 + (modPlayer.onhitdamage / 9);
				VoidPlayer.VoidEffect(player, 3 + (modPlayer.onhitdamage / 9));
			}
			modPlayer.CritVoidsteal += 0.7f;

			player.buffImmune[BuffID.BrokenArmor] = true;
			player.buffImmune[BuffID.Ichor] = true;
		}
	}
}