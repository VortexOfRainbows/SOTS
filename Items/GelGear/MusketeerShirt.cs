using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Body)]
	public class MusketeerShirt : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 38;
			item.height = 28;

			item.value = 105000;
			item.rare = 5;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Musketeer's Cloak");
			Tooltip.SetDefault("5% increased ranged damage and 20% increased ranged crit chance\n34% decrease to all other damage types");
		}

		public override void UpdateEquip(Player player)
		{
				player.rangedDamage += .05f;
				player.rangedCrit += 20;
				player.meleeDamage -= 0.34f;
				player.magicDamage -= 0.34f;
				player.minionDamage -= 0.34f;
				player.thrownDamage -= 0.34f;
           
		}

	}
}