using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	[AutoloadEquip(EquipType.Head)]
	
	public class SaturnHelmet : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 28;
			item.height = 20;
			item.vanity = true;
			item.value = 0;
			item.rare = -11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Saturn's Helmet");
			Tooltip.SetDefault("Great for impersonating SOTS devs!");
		}

	}
}