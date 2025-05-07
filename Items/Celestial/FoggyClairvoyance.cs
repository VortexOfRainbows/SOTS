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
			this.SetResearchCost(1);
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
			CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient<Fragments.PrecariousCluster>(1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(ModContent.BuffType<FluidCurse>(), 3);
			for(int i = 0; i < player.buffImmune.Length; i++)
				if(ValidImmunableDebuff(i))
					player.buffImmune[i] = true;
			player.GetDamage(DamageClass.Generic) += 0.15f;
		}
        public static List<int> bList = 
			[BuffID.PotionSickness, ModContent.BuffType<FluidCurse>(), ModContent.BuffType<VoidRecovery>(), 
			ModContent.BuffType<VoidShock>(), ModContent.BuffType<VoidSickness>(), BuffID.ManaSickness,
			ModContent.BuffType<Satiated>(), ModContent.BuffType<VoidMetamorphosis>(), BuffID.ChaosState];
		public static bool ValidImmunableDebuff(int i)
		{
			return Main.debuff[i] && !bList.Contains(i) && 
				(i < BuffID.Count || BuffLoader.GetBuff(i).Mod == SOTS.Instance); //Only effects vanilla and SOTS debuffs
        }
	}
}