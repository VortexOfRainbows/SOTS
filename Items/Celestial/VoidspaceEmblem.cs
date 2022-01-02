using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Slime;
using SOTS.Items.Pyramid;
using SOTS.Items.Otherworld.FromChests;

namespace SOTS.Items.Celestial
{
	public class VoidspaceEmblem : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidspace Emblem");
			Tooltip.SetDefault("Increases void damage and magic damage by 10%\nIncreases void crit by 10%\nCritical strikes heal small amounts of void\nReduces void cost by 8%\nIncreases void gain by 4 and max void by 50\nRegenerate void when hit\nImmunity to broken armor and ichor");
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
			recipe.AddIngredient(ModContent.ItemType<SanguiteBar>(), 15);
			recipe.AddIngredient(ModContent.ItemType<WormWoodParasite>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VoidenBracelet>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SkywareBattery>(), 1);
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
			voidPlayer.bonusVoidGain += 4;
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