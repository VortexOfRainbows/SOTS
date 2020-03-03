using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class MinersSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Sword");
		}
		public override void SetDefaults()
		{
            item.damage = 20; 
            item.melee = true;  
            item.width = 32;   
            item.height = 32;
            item.useTime = 16; 
            item.useAnimation = 16;
            item.useStyle = 1;    
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
			item.crit = 6;
		}
	}
}
