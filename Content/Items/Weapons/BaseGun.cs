using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Weapons;

/// TODO: Major refactoring needed. Add way to easily set primary fire and alt fire settings
// MOVE REVOLVER AND OTHER SPECIFIC CODE TO INHERITED CLASS
public enum FireMode {
  Primary,
  Secondary,
  Bonus
}

public enum GunType {
  Rifle,
  Revolver,
  Sniper,
  Shotgun
}

public struct GunStats {
  public int damage = 10;
  public int crit = 10;
  public float knockback = 10;
  public float bullet_speed = 15;

  public int use_time = 5;
  public int fire_rate = 10;
  public SoundStyle use_sound = SoundID.Item11;

  public bool full_auto = false;

  public float spread_angle = 0;
  public int bullets_per_spread = 1;
  public int bullets_per_burst = 1;
  public int bursts_between_each_bonus = 0;
  public float ammo_save_chance = 0;

  public bool converts_bullets = false;
  public int converted_bullet_type = ProjectileID.Bullet;

  public GunStats() { }
  // public GunType type                 = GunType.Rifle;
}

public abstract class BaseGun : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Weapons/BaseGun";
  public abstract Dictionary<FireMode, GunStats> Stats { get; }

  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    SetGunData();
  }

  public void SetGunData() {
    SetGunDefaults();
    ApplyGunSettings(FireMode.Primary);
  }

  private void SetGunDefaults() {
    // these settings shouldn't change for any guns
    Item.useStyle = ItemUseStyleID.Shoot;
    Item.DamageType = DamageClass.Ranged;
    Item.useAmmo = AmmoID.Bullet;
    Item.shoot = ProjectileID.PurificationPowder; // why the fuck
    Item.noMelee = true;
  }

  private void ApplyGunSettings(FireMode mode) {
    // Stats
    Item.damage = Stats[mode].damage;
    Item.crit = Stats[mode].crit;
    Item.knockBack = Stats[mode].knockback;
    Item.shootSpeed = Stats[mode].bullet_speed;

    // Usage
    Item.useTime = Stats[mode].use_time;
    Item.useAnimation = GetUseAnimation(mode); // For burst weapons, useAnimation is non-trivial
    Item.reuseDelay = Stats[mode].fire_rate;

    Item.autoReuse = Stats[mode].full_auto;
    Item.UseSound = Stats[mode].use_sound;
  }

  //public bool isUsingPrimaryFire(Player player) => player.altFunctionUse == 1 && hasFireMode(FireMode.Primary);

  // UseAnimation for burst weapons: gunData_.isBurstFire ? Stats[mode].useTime * gunData_.burstCount : Stats[mode].useAnimationTime;
  private int GetUseAnimation(FireMode mode) {
    return Stats[mode].use_time * Stats[mode].bullets_per_burst;
  }

  public bool IsUsingAltFire(Player player) {
    return player.altFunctionUse == 2 && HasFireMode(FireMode.Secondary);
  }

  public bool HasFireMode(FireMode mode) {
    return Stats.ContainsKey(mode);
  }

  public FireMode CurrentFireMode(Player player) {
    return IsUsingAltFire(player) ? FireMode.Secondary : FireMode.Primary;
  }

  public bool IsBurstFiring(Player player) {
    return Stats[CurrentFireMode(player)].bullets_per_burst > 1;
  }

  public override void ModifyShootStats(
    Player player,
    ref Vector2 position,
    ref Vector2 velocity,
    ref int type,
    ref int damage,
    ref float knockback
  ) {
    var current_fire_mode = CurrentFireMode(player);

    // converts bullets
    if (type == ProjectileID.Bullet) {
      type = Stats[current_fire_mode].converted_bullet_type;
    }

    // inaccuracy
    // float spreadAngle = Stats[currentFireMode].spreadAngle;
    // if (_gunSettings.isRevolver && player.altFunctionUse == 2) {
    //     spreadAngle = 15;
    //     knockBack   =  item.knockBack * 2;
    // }

    // Shotguns should not add more spread (it's already added when the bullets are spawned)
    if (Stats[current_fire_mode].bullets_per_spread <= 1) {
      velocity = velocity.RotatedByRandom(MathHelper.ToRadians(Stats[current_fire_mode].spread_angle));
    }
  }

  public override bool Shoot(
    Player player,
    EntitySource_ItemUse_WithAmmo source,
    Vector2 position,
    Vector2 velocity,
    int type,
    int damage,
    float knockback
  ) {
    var mod_player = player.GetModPlayer<RangerGunPlayer>();
    var current_fire_mode = CurrentFireMode(player);
    ApplyGunSettings(current_fire_mode);

    // extra rocket that fires only during first burst
    if (!(player.itemAnimation < Item.useAnimation - 2)) {
      if (HasFireMode(FireMode.Bonus) && mod_player.num_bullets == Stats[current_fire_mode].bursts_between_each_bonus) {
        var bonus_velocity = velocity;
        bonus_velocity.Normalize();
        bonus_velocity =
          (bonus_velocity * Stats[FireMode.Bonus].bullet_speed).RotatedByRandom(
            MathHelper.ToRadians(Stats[FireMode.Bonus].spread_angle)
          );

        // muzzle offset
        var adjusted_position = position;
        var muzzle_offset = Vector2.Normalize(velocity) * (player.itemWidth + 1);
        if (Collision.CanHit(adjusted_position, 0, 0, adjusted_position + muzzle_offset, 0, 0)) {
          adjusted_position += muzzle_offset;
        }

        Projectile.NewProjectileDirect(
          source,
          adjusted_position,
          bonus_velocity,
          Stats[FireMode.Bonus].converted_bullet_type,
          Stats[FireMode.Bonus].damage,
          Stats[FireMode.Bonus].knockback,
          player.whoAmI
        );
      }

      // fires every couple bursts
      if (mod_player.num_bullets < Stats[current_fire_mode].bursts_between_each_bonus) {
        mod_player.num_bullets++;
      } else {
        mod_player.num_bullets = 0;
      }
    }


    for (var i = 0; i < Stats[current_fire_mode].bullets_per_spread - 1; i++) {
      // shotguns skip 1 bullet (original bullet)
      // Rotate the velocity randomly by 30 degrees at max.
      var new_velocity = velocity.RotatedByRandom(MathHelper.ToRadians(Stats[current_fire_mode].spread_angle));

      // Decrease velocity randomly for nicer visuals.
      new_velocity *= 1f - Main.rand.NextFloat(0.3f);

      // Create a projectile.
      Projectile.NewProjectileDirect(source, position, new_velocity, type, damage, knockback, player.whoAmI);
    }

    return true;
  }

  public override bool CanConsumeAmmo(Item ammo, Player player) {
    var wont_save_ammo = Main.rand.NextFloat() < 1f - Stats[CurrentFireMode(player)].ammo_save_chance;

    if (!IsBurstFiring(player)) {
      return wont_save_ammo;
    }

    var will_use_ammo_in_burst = !(player.itemAnimation < Item.useAnimation - 2);
    return will_use_ammo_in_burst && wont_save_ammo;
  }

  /// TODO: Needs to be redone to allow custom alt fire
  public override bool CanUseItem(Player player) {
    return base.CanUseItem(player);
  }

  public override bool AltFunctionUse(Player player) {
    return HasFireMode(FireMode.Secondary);
  }
}

public class RangerGunPlayer : ModPlayer {
  public int num_bullets;

  public override void ResetEffects() {
    if (num_bullets < 0) {
      num_bullets = 0;
    }
  }
}