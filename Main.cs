using HarmonyLib;
using MelonLoader;
using Ships.Interface.Model.Parts.State;
using Ships.Interface.Model.Parts.StateTypes;
using Ships.Interface.Model.Placement;
using Ships.Interface.Model.State;
using Ships.Physics;
using Ships.Physics.Forces;
using Ships.Physics.Model;
using System;
using UnityEngine;

namespace RealisticThrusters
{
    public static class BuildInfo
    {
        public const string Name = "Realistic Thrusters";
        public const string Description = "Makes thrusters push from where they’re mounted, not from the center of mass";
        public const string Author = "jollyname";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = null;
    }

    public class RealisticThrusters : MelonMod
    {

    }

    [HarmonyPatch(typeof(ShipNewtonianPhysics), nameof(ShipNewtonianPhysics.ComputePhysicsForces))]
    public static class ThrusterTorquePatch
    {
        // Stock constants
        private const float ThrusterForcePerStrength = 7800f;
        private const float WoodThrusterForcePerLevel = 3000f;

        // 1: realistic, 0: stock (no torque!)
        public static float TorqueScale = 1f;

        // Private fields
        private static readonly AccessTools.FieldRef<ShipNewtonianPhysics, ShipThrust> ThrustRef =
            AccessTools.FieldRefAccess<ShipNewtonianPhysics, ShipThrust>("_thrust");
        private static readonly AccessTools.FieldRef<ShipThrust, IShipStateRead> StateRef =
            AccessTools.FieldRefAccess<ShipThrust, IShipStateRead>("_stateRead");

        /// <summary>
        /// Adds the torque of every part of thruster of type TState
        /// </summary>
        private static Vector3 TorqueOf<TState>(IShipStateRead ship, Vector3 com, Func<TState, float> thrustOf) where TState : IStatefulPartState
        {
            Vector3 torque = Vector3.zero;
            foreach (var part in ship.GetAllStatefulParts<TState>())
            {
                Vector3 dir = -PartOrientationHelper.GetVector3(part.Placement.UpOrientation); // Up! (or down Ig)
                Vector3 force = dir * thrustOf((TState)part.State);
                torque += Vector3.Cross(part.LocalPosition - com, force); 
            }
            return torque;
        }

        public static void Postfix(ShipNewtonianPhysics __instance, ShipPhysicsCalculationProperties calculationProperties, ref ShipPhysicsForcesResult __result)
        {
            if (TorqueScale == 0f) return;

            IShipStateRead ship = StateRef(ThrustRef(__instance));
            Vector3 com = calculationProperties.CenterOfMassIncludingConnectedShips;

            // The games calculates thrust weirdly, a loop for each thruster type in CalculateShipThrustForce, so I have to do the same :(
            // I made a helper method TorqueOf<TState> to make it less repetitive tho! :)
            Vector3 torque =
                  TorqueOf<ThrusterState>(ship, com, s => s.ThrusterStrength * ThrusterForcePerStrength)
                + TorqueOf<WoodThrusterState>(ship, com, s => s.NormalizedThrustLevel * WoodThrusterForcePerLevel);

            __result = new ShipPhysicsForcesResult(__result.ForceToAdd, __result.TorqueToAddInNewtonMeters + torque * TorqueScale, __result.TorqueToAddInDegreesSecond);
        }
    }
}