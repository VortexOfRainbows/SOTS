using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.SpecialDrops
{
	public class BarbaricEssence : ModItem
	{
		int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Barbaric Essence");
			Tooltip.SetDefault("Summons a barbaric axe around you");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 4));
		}
		public override void SetDefaults()
		{
	
      
            item.width = 21;     
            item.height = 33;   
            item.value = 12500;
            item.rare = 4;

			item.accessory = true;
			item.defense = 12;
			
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.AnimatesAsSoul[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{	
			
			if(Probe == -1)
			{
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("BarbaricAxe"), 12, 0, player.whoAmI);
			}
			
			Probe++;
			if(Probe == 360)
			{
				Probe = 0;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("BarbaricAxe"), 12, 0, player.whoAmI);
			}
		}
	}
}