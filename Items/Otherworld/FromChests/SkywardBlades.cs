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
            Item.damage = 37;  
            Item.DamageType = DamageClass.Ranged; 
            Item.width = 56;    
            Item.height = 52;  
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;   
            Item.autoReuse = true; 
            Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item44;
			Item.crit = 2;
			Item.shoot = Mod.Find<ModProjectile>("SkywardBladeBeam").Type;
			Item.shootSpeed = 5.5f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}
        public override bool BeforeUseItem(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.skywardBlades <= 0)
			{
				Item.useTime = 48;
				Item.useAnimation = 48;
				Item.UseSound = SoundID.Item44;
			}
			else
			{
				Item.useTime = 8;
				Item.useAnimation = 8;
				Item.UseSound = SoundID.Item71;
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
        public override int GetVoid(Player player)
		{
			return  60;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "PlatinumDart", 1).AddIngredient(null, "HardlightAlloy", 12).AddTile(Mod.Find<ModTile>("HardlightFabricatorTile").Type).Register();
		}
	}
}
