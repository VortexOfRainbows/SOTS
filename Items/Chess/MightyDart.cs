using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Chess
{
	public class MightyDart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mighty Dart");
			
			Tooltip.SetDefault("Steals life on hit\nS");
		}
		public override void SetDefaults()
		{
			item.damage = 50;  //gun damage
            item.thrown = true;   //its a gun so set this to true
            item.width = 18;     //gun image width
            item.height = 32;   //gun image  height
            item.useTime = 1;  //how fast 
            item.useAnimation = 2;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 125000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("MightDart"); 
            item.shootSpeed = 0.1f;
			item.noUseGraphic = true;

			
		}
          public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
					player.AddBuff(mod.BuffType("Needle"), 300);
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
			
                if(modPlayer.needle == true)
				{
			  return false;
				}
				else
				{
					
			  return true;
				}
					
          }  
	}
}