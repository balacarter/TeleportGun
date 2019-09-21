using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;

namespace TeleportGun
{
    public class TeleportGun : Script
    {
        /// These variables are for Mod Information on startup
        bool firstTime = true;
        string ModName = "Teleport Gun";
        string Developer = "scriptHijo";
        string Version = "1.10";

        Player player = Game.Player;
        Ped ped = Game.Player.Character;

        bool teleported = false;
        bool active = false;

        public TeleportGun()
        {
            Tick += onTick;
            KeyDown += OnKeyDown;
            Interval = 1;
        }

        public void OnKeyDown(Object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.J)
            {
                if(active)
                {
                    UI.Notify(ModName + " Deactivated");
                }
                    
                else
                {
                    UI.Notify(ModName + " Activated");
                }

                clearVehFlags();
                active = !active;
            }
        }

        private void onTick(object sender, EventArgs e)
        {
            // Mod info
            if (firstTime)
            {
                UI.Notify(ModName + " " + Version + " by " + Developer + " Loaded");
                firstTime = false;
            }
            // start your script here:
 
            if(active && ped.Weapons.Current.Hash == WeaponHash.SniperRifle)
            {
                if (ped.IsShooting)
                {
                    teleported = false;
                }
                    

                if(!teleported)
                {
                    Vehicle[] allVeh = GTA.World.GetAllVehicles();

                    foreach (Vehicle x in allVeh)
                    {
                        if (Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, x, new InputArgument(WeaponHash.SniperRifle), 0))
                        {
                            Function.Call(Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, x);
                            Function.Call(Hash.CLEAR_ENTITY_LAST_WEAPON_DAMAGE, x);
                            x.Driver.Delete();
                            Function.Call(Hash.SET_PED_INTO_VEHICLE, ped, x, -1);
                            UI.Notify("Hit: " + x.DisplayName);
                            
                            teleported = true;
                            break;
                        }
                    }

                    Vector3 bullet = ped.GetLastWeaponImpactCoords();
                    if (teleported == false && bullet.X != 0 && bullet.Y != 0 && bullet.Z != 0 )
                    {
                        UI.Notify("X: " + bullet.X.ToString("F") + " Y: " + bullet.Y.ToString("F") + " Z: " + bullet.Z.ToString("F"));
                        ped.Position = bullet;
                        teleported = true;
                    }
                }

                

                
            }
            
        }

        private void clearVehFlags()
        {
            Vehicle[] vehs = GTA.World.GetAllVehicles();
            foreach (Vehicle x in vehs)
            {
                if (Function.Call<bool>(Hash.HAS_ENTITY_BEEN_DAMAGED_BY_WEAPON, x, new InputArgument(WeaponHash.SniperRifle), 0))
                {
                    Function.Call(Hash.CLEAR_ENTITY_LAST_DAMAGE_ENTITY, x);
                    Function.Call(Hash.CLEAR_ENTITY_LAST_WEAPON_DAMAGE, x);

                }
            }
        }


    }
}
