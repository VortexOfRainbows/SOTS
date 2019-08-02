using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	[AutoloadEquip(EquipType.Legs)]
	
	public class SaturnLeggings : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;
			item.vanity = true;
			item.value = 0;
			item.rare = -11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Saturn's Leggings");
			Tooltip.SetDefault("Great for impersonating SOTS devs!");
		}


	}
}