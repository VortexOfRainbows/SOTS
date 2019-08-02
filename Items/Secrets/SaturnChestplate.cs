using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	[AutoloadEquip(EquipType.Body)]
	
	public class SaturnChestplate : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 20;
			item.vanity = true;
			item.value = 0;
			item.rare = -11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Saturn's Chestplate");
			Tooltip.SetDefault("Great for impersonating SOTS devs!");
		}


	}
}