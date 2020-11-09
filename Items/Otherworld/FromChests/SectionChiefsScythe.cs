using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SectionChiefsScythe : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Section Chief's Scythe");
			Tooltip.SetDefault("Critical hits summon a Soul of Retaliation into the air\nEvery 10th void attack will release the soul in the form of a powerful laser");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 31;  
            item.melee = true; 
            item.width = 56;    
            item.height = 52;  
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 1;   
            item.autoReuse = true; 
			item.useTurn = true;
            item.knockBack = 3f;
			item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item71;
			item.crit = 11;
			item.shoot = mod.ProjectileType("ScytheSlash");
			item.shootSpeed = 15f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			knockBack *= 3f;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if(crit)
			{
				BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
				if (crit && Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulofRetaliation"), damage + modPlayer.soulDamage, 1f, player.whoAmI);
				}
			}
		}
		public override void GetVoid(Player player)
		{
			voidMana = 7;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlatinumScythe", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
