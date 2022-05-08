using Microsoft.Xna.Framework;
using SOTS.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class OlympianAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olympian Waraxe");
            Tooltip.SetDefault("Enter a 3 second Frenzy after killing an enemy, massively increasing melee attack speed\nRight click to toss the axe for 60% damage\nCan toss two axes when under Frenzy");
        }
		public override void SetDefaults()
		{
            Item.damage = 21; 
            Item.melee = true;  
            Item.width = 38;   
            Item.height = 38;
            Item.useTime = 24; 
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.SwingThrow;    
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 16;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.OlympianAxe>();
            Item.shootSpeed = 8.5f;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (target.life <= 0)
            {
                Main.PlaySound(SoundID.MaxMana, player.Center);
                player.AddBuff(ModContent.BuffType<Frenzy>(), 190);
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            damage = (int)(damage * 0.6f);
            speedX *= (SOTSPlayer.ModPlayer(player).attackSpeedMod);
            speedY *= (SOTSPlayer.ModPlayer(player).attackSpeedMod);
            return player.ownedProjectileCounts[Item.shoot] <= (player.HasBuff(ModContent.BuffType<Frenzy>()) ? 1 : 0) && player.altFunctionUse == 2;
        }
        public override float UseTimeMultiplier(Player player)
        {
            return player.altFunctionUse == 2 ? 1.1f : 1;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item19;
                Item.axe = 0;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.UseSound = SoundID.Item1;
                Item.axe = 16;
            }
            return player.ownedProjectileCounts[Item.shoot] <= (player.HasBuff(ModContent.BuffType<Frenzy>()) ? 1 : 0);
        }
    }
}
