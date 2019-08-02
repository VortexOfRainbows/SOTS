using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	[AutoloadEquip(EquipType.Head)]
	
	public class MeguminHat : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 38;
			item.height = 22;

			item.value = 125000;
			item.rare = 10;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archmage's Hat");
			Tooltip.SetDefault("Fire an additional projectile with less velocity when using magic weapons\nWhen mana is below 25% of its max, paralysis is induced\n25% increased magic speed\n33% increased mana usage\n33% decrease to all other damage types");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MeguminShirt") && legs.type == mod.ItemType("MeguminLeggings");
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Fatal hits will now return your health to your current mana\nEach time this happens, your max mana will be cut down depending on how much damage you take\nIf you take more damage than your max mana can handle, the fatal hit will not be cancelled\nYour max mana will return to normal after death or 5 minutes";
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.megSet = true;
				
			
		}
		

		public override void UpdateEquip(Player player)
		{
			for(player.statMana = player.statMana; player.statMana < 0; player.statMana++)
			{
				
			}
				player.manaCost += 0.33f;
				player.meleeDamage -= 0.33f;
				player.rangedDamage -= 0.33f;
				player.minionDamage -= 0.33f;
				player.thrownDamage -= 0.33f;
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.megHat = true;
				
			if(player.statMana < (player.statManaMax2 + player.statManaMax) * 0.25f)
			{
				player.AddBuff(mod.BuffType("FrozenThroughTime"), 30, false);
				
			}
		}
		public override void AddRecipes()
		{
			/*
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VampireStaff", 1);
			recipe.AddIngredient(null, "GelBar", 24);
			recipe.AddIngredient(ItemID.Leather, 20);
			recipe.AddIngredient(null, "GoblinRockBar", 28);
			recipe.AddIngredient(ItemID.Bone, 40);

			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
			*/
		}

	}
}