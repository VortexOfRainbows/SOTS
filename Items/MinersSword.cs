using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MinersSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Sword");
            Tooltip.SetDefault("Critically strikes while falling");
        }
		public override void SetDefaults()
		{
            item.damage = 20; 
            item.melee = true;  
            item.width = 32;   
            item.height = 32;
            item.useTime = 16; 
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
		}
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (player.velocity.Y > 0)
                crit = true;
            base.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
        }
    }
}
