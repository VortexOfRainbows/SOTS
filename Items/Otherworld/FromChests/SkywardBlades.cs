using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Otherworld;
using System.IO;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SkywardBlades : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyward Blades");
			Tooltip.SetDefault("Arm yourself with several deadly throwing knives\nScoring a hit restores a dagger\n'Watch this!'");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 37;  
            item.ranged = true; 
            item.width = 56;    
            item.height = 52;  
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 1;   
            item.autoReuse = true; 
            item.knockBack = 3f;
			item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item44;
			item.crit = 2;
			item.shoot = mod.ProjectileType("SkywardBladeBeam");
			item.shootSpeed = 15f;
			item.noMelee = true;
			item.noUseGraphic = true;
		}
        public override bool BeforeUseItem(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.skywardBlades <= 0)
			{
				item.useTime = 48;
				item.useAnimation = 48;
				item.UseSound = SoundID.Item44;
			}
			else
			{
				item.useTime = 8;
				item.useAnimation = 8;
				item.UseSound = SoundID.Item71;
			}
			return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.skywardBlades <= 0)
			{
				modPlayer.skywardBlades = 5;
				modPlayer.SendClientChanges(modPlayer);
            }
			else if(modPlayer.skywardBlades > 0)
			{
				position += new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero) * 64;
				modPlayer.skywardBlades--;
				modPlayer.SendClientChanges(modPlayer);
				return true;
			}
			knockBack *= 3f;
			return false;
        }
        public override bool BeforeDrainMana(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			return modPlayer.skywardBlades <= 0;
        }
        public override void GetVoid(Player player)
		{
			voidMana = 60;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlatinumDart", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
