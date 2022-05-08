using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Pyramid;
using SOTS.Items.Fragments;

namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class BulwarkOfTheAncients : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bulwark Of The Ancients");
			Tooltip.SetDefault("Grants immunity to knockback and fire blocks\nGrants immunity to most debuffs\nIncreases void gain by 2, life regen by 1, reduces damage taken by 5%, and increases crit chance by 4%\nSurrounds you with 4 orbital projectiles\nProjectiles disabled when hidden");
		}
		public override void SetDefaults()
		{
			Item.damage = 34;
			Item.magic = true;
            Item.width = 42;     
            Item.height = 46;   
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
			Item.defense = 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OlympianAegis>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ChiseledBarrier>(), 1);
			recipe.AddIngredient(ItemID.AnkhShield, 1);
			recipe.AddIngredient(ModContent.ItemType<TerminalCluster>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			//modPlayer.PushBack = true; //removing this effect
			
			//Grants immunity to knockback and fire blocks, Grants immunity to most debuffs
			player.noKnockback = true;
			player.buffImmune[BuffID.Burning] = true; 
            player.buffImmune[BuffID.BrokenArmor] = true; 
            player.buffImmune[BuffID.Weak] = true; 
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
            player.buffImmune[BuffID.Slow] = true; 
            player.buffImmune[BuffID.Confused] = true; 
            player.buffImmune[BuffID.Silenced] = true; 
            player.buffImmune[BuffID.Cursed] = true; 
            player.buffImmune[BuffID.Darkness] = true; 
            player.buffImmune[BuffID.Chilled] = true;

			//Increases void gain by 2, life regen by 1, reduces damage taken by 5%, and increases crit chance by 4%
			voidPlayer.bonusVoidGain += 2f;
			player.lifeRegen += 1;
			player.endurance += 0.05f;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
			player.magicCrit += 4;
			player.thrownCrit += 4;

			//Surrounds you with 4 orbital projectiles
			if(Main.myPlayer == player.whoAmI && !hideVisual)
			{
				int damage = (int)(Item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f)));
				modPlayer.tPlanetDamage += damage;
				modPlayer.tPlanetNum += 2;
				modPlayer.aqueductDamage += damage;
				modPlayer.aqueductNum += 2;
			}
		}
	}
}