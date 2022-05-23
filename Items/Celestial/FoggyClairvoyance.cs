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
			Tooltip.SetDefault("Increases damage by 15% and grants immunity to almost every debuff, but at a cost\n'Cursed'");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 38;     
            Item.height = 40;   
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SanguiteBar>(), 15).AddIngredient(ModContent.ItemType<Fragments.PrecariousCluster>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(ModContent.BuffType<FluidCurse>(), 3);
			List<int> bList = new List<int>() { BuffID.PotionSickness, ModContent.BuffType<FluidCurse>(), ModContent.BuffType<VoidRecovery>(), ModContent.BuffType<VoidShock>(), ModContent.BuffType<VoidSickness>(), BuffID.ManaSickness, ModContent.BuffType<Satiated>(), ModContent.BuffType<VoidMetamorphosis>(), BuffID.ChaosState };
			Mod catalyst = ModLoader.GetMod("Catalyst");
			if(catalyst != null)
            {
				bList.Add(catalyst.Find<ModBuff>("InfluxCoreCooldown").Type);
            }
			for(int i = 0; i < player.buffImmune.Length; i++)
            {
				bool debuff = Main.debuff[i];
				if(debuff && !bList.Contains(i))
                {
					player.buffImmune[i] = true;
                }
            }
			player.GetDamage(DamageClass.Generic) += 0.15f;
		}
	}
}