using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Minions;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{	[AutoloadEquip(EquipType.Shield)]
	public class ShardGuard : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Summon;
			Item.maxStack = 1;
            Item.width = 38;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Lime;
			Item.defense = 1;
			Item.accessory = true;
		}
		public void ProbesGen(Player player)
        {
            player.sotsPlayer().artifactProbeDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
            player.sotsPlayer().artifactProbeNum += 8;
        }
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int rand = Main.rand.Next(10);
			if (rand >= 0 && rand <= 1) //0,1,2 20%
				modPlayer.shardOnHit += 3;
			if (rand >= 2 && rand <= 4) //3,4 30%
				modPlayer.shardOnHit += 4;
			if (rand >= 5) //5,6,7,8,9 50%
				modPlayer.shardOnHit += 5;

			modPlayer.bonusShardDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
			player.statLifeMax2 += 20;

			if ((double)player.statLife <= (double)player.statLifeMax * 0.5)
			{
				player.AddBuff(BuffID.IceBarrier, 600, true);
			}

			ProbesGen(player);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<DissolvingAurora>(1).AddIngredient(ItemID.FrozenTurtleShell, 1).AddIngredient<PermafrostMedallion>(1).AddIngredient<ShatterHeartShield>(1).AddTile(TileID.TinkerersWorkbench).Register();
        }
        public override bool WeaponPrefix()
        {
            return false;
        }
    }
}