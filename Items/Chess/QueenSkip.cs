using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Chess
{
	public class QueenSkip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Skip");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
			Tooltip.SetDefault("Skips to wave 5");
		}
		public override void SetDefaults()
		{
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 1;
			item.shoot = 1;
			item.shootSpeed = 1;
			item.width = 24;
			item.height = 24;
			item.consumable = true;
			item.value = 125000;
			item.rare = 7;
			item.maxStack = 99;
			 ItemID.Sets.ItemNoGravity[item.type] = true; 
			 ItemID.Sets.AnimatesAsSoul[item.type] = false;
			 item.noUseGraphic = true;


			
		} public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			 SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                modPlayer.chessSkip = true;
				
					player.AddBuff(mod.BuffType("WaveSkipped"), 360000);
			  
					return false;
					
          } 
	}
}