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
			Item.maxStack = 1;
            Item.width = 32;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SanguiteBar>(), 15).AddIngredient(ModContent.ItemType<WormWoodParasite>(), 1).AddIngredient(ModContent.ItemType<VoidenBracelet>(), 1).AddIngredient(ModContent.ItemType<SkywareBattery>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.1f;
			voidPlayer.voidCost -= 0.08f;
			player.GetDamage(DamageClass.Magic) += 0.1f;
			voidPlayer.bonusVoidGain += 4;
			voidPlayer.voidMeterMax2 += 50;
			voidPlayer.voidCrit += 10;

			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
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