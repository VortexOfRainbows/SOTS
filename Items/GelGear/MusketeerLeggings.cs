using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Legs)]
	public class MusketeerLeggings : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = 105000;
			item.rare = 5;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Musketeer's Leggings");
			Tooltip.SetDefault("Killed enemies can drop wooden arrows and musket balls\n5% increased ranged damage\n33% decrease to all other damage types");
		}
		public override void UpdateEquip(Player player)
		{
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.ammoRecover = true;
				
				player.rangedDamage += 0.05f;
				player.meleeDamage -= 0.33f;
				player.magicDamage -= 0.33f;
				player.minionDamage -= 0.33f;
				player.thrownDamage -= 0.33f;
				
		}

	}
}