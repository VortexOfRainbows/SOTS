using System;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items.Permafrost
{  
    public class ShardStaff : ModItem
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard Staff");
			Tooltip.SetDefault("Summons shards around the player");
		}
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Magic;
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 20;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
			Item.shootSpeed = 9;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item43;
            Item.mana = 12;
			Item.shoot = mod.ProjectileType("IceShard");
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(modPlayer.shardSpellExtra != 0)
			{
				Item.useTime = 12;
			}
			else
			{
				Item.useTime = 20;
			}
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 toPos = Main.MouseWorld;
			for(int i = 0; i < 1; i++)
			{
				Vector2 newPos = position + new Vector2(Main.rand.Next(-48, 49), Main.rand.Next(-48, 19));
				float speed = new Vector2(speedX, speedY).Length();
				Vector2 speed2 = new Vector2(speed, 0).RotatedBy(Math.Atan2(toPos.Y - newPos.Y, toPos.X - newPos.X));
				if(Item.crit + player.magicCrit + 4 >= Main.rand.Next(100) + 1)
				{
					Projectile.NewProjectile(newPos.X, newPos.Y, speed2.X, speed2.Y, type, damage, knockBack, player.whoAmI, 5, 2);
				}
				else
				{
					Projectile.NewProjectile(newPos.X, newPos.Y, speed2.X, speed2.Y, type, damage, knockBack, player.whoAmI, Main.rand.Next(5), 2);
				}
			}
			return false;
		}
    }
}